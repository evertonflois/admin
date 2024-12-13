using Admin.Application.Dto;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Application.Interfaces.Services;
using Admin.Application.Auth;
using Microsoft.Extensions.Options;
using Admin.Application.Helpers;
using Admin.Domain.Entities.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using AutoMapper;

namespace Admin.Application.Services;

public class AuthAppService : IAuthAppService
{
    #region declarations
    private IUnitOfWorkRepository _uoW;
    private IMapper _mapper;
    private ITransactionRepository _transactionRepository;
    private ITransactionActionsRepository _transactionActionsRepository;
    private IMenuRepository _menuRepository;
    private IUserRepository _repository;
    private IRefreshTokenRepository _refreshTokenRepository;
    private IJwtUtils _jwtUtils;
    private readonly AppSettings _appSettings;
    #endregion declarations

    #region constructor
    public AuthAppService(IUnitOfWorkRepository uoW, IMapper mapper, ITransactionRepository transactionRepository, ITransactionActionsRepository transactionActionsRepository, IMenuRepository menuRepository, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtUtils jwtUtils, IOptions<AppSettings> appSettings)
    {
        _uoW = uoW;
        _mapper = mapper;
        _transactionRepository = transactionRepository;
        _transactionActionsRepository = transactionActionsRepository;
        _menuRepository = menuRepository;
        _repository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
    }
    #endregion constructor

    #region public methods
    public async Task<AuthViewModel> Authenticate(AuthInputModel model, string ipAddress)
    {
        var result = new AuthViewModel() { Success = false, Login = model.Login };        

        using (await _uoW.BeginTransactionAsync())
        {            
            User? user = null;
            if (model.Login.Equals(_appSettings.Admin.Login))
            {
                if (!BCrypt.Net.BCrypt.Verify(model.Password, _appSettings.Admin.Password))
                {
                    result.Message = "Invalid Login/Password";
                    return result;
                }

                user = new User() { SubscriberId = Guid.NewGuid().ToString(), Login = model.Login };                
            }
            else
            {
                var userSearch = await _repository.GetAllAsync(new { model.Login });
                user = userSearch.FirstOrDefault();

                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    result.Message = "Invalid Login/Password";
                    return result;
                }
            }

            result.Token = _jwtUtils.GenerateJwtToken(model);
            
            var refreshToken = await _jwtUtils.GenerateRefreshToken(ipAddress);
            refreshToken.SubscriberId = user.SubscriberId;            
            refreshToken.Login = user.Login;

            _ = await _refreshTokenRepository.CreateAsync(refreshToken);

            _ = await _refreshTokenRepository.RemoveOldByUserAsync(model.Login, _appSettings.RefreshTokenTTL);
                        
            result.RefreshToken = refreshToken.Token;
            result.Success = true;
            if (!string.IsNullOrEmpty(user.ProfileCode))
            {
                result.Transactions = await getTransactions(user.ProfileCode);
                result.Menu = await getMenu(user.ProfileCode);
            }

            await _uoW.SaveChangesAsync();
        }

        return result;
    }

    public async Task<AuthViewModel> RefreshToken(string token, string ipAddress)
    {
        var result = new AuthViewModel() { Success = false };

        using (await _uoW.BeginTransactionAsync())
        {
            var refreshTokenSearch = await _refreshTokenRepository.GetAllAsync(new { Token = token });
            if (refreshTokenSearch.Any())
            {
                var refreshToken = refreshTokenSearch.First();

                result.Login = refreshToken.Login;

                if (refreshToken.IsRevoked)
                    await revokeDescendantRefreshTokens(refreshToken, refreshToken.Login, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");

                if (!refreshToken.IsActive)
                    throw new AppException("Invalid token");

                var userSearch = await _repository.GetAllAsync(new { cd_user = refreshToken.Login });
                var user = userSearch.FirstOrDefault();

                if (user == null)
                    throw new AppException("Invalid token user");

                // replace old refresh token with a new one (rotate token)
                var newRefreshToken = await rotateRefreshToken(refreshToken, ipAddress);
                newRefreshToken.SubscriberId = user.SubscriberId;                
                newRefreshToken.Login = user.Login;
                _ = await _refreshTokenRepository.CreateAsync(newRefreshToken);

                _ = await _refreshTokenRepository.RemoveOldByUserAsync(refreshToken.Login, _appSettings.RefreshTokenTTL);

                // generate new jwt
                result.Token = _jwtUtils.GenerateJwtToken(new AuthInputModel() { Login = refreshToken.Login });
                result.RefreshToken = newRefreshToken.Token;
                result.Success = true;                
            }

            await _uoW.SaveChangesAsync();
        }

        return result;
    }

    public async Task RevokeToken(string token, string ipAddress)
    {
        using (await _uoW.BeginTransactionAsync())
        {
            var refreshTokenSearch = await _refreshTokenRepository.GetAllAsync(new { Token = token });
            if (refreshTokenSearch.Any())
            {
                var refreshToken = refreshTokenSearch.First();

                if (!refreshToken.IsActive)
                    throw new AppException("Invalid token");

                await revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            }

            await _uoW.SaveChangesAsync();
        }
    }
    #endregion public methods

    #region private methods
    private async Task revokeDescendantRefreshTokens(RefreshToken refreshToken, string cd_user, string ipAddress, string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
        {
            var childToken = (await _refreshTokenRepository.GetAllAsync(new { Token = refreshToken.ReplacedByToken })).SingleOrDefault();
            if (childToken != null)
            {
                if (childToken.IsActive)
                    await revokeRefreshToken(childToken, ipAddress, reason);
                else
                    await revokeDescendantRefreshTokens(childToken, cd_user, ipAddress, reason);
            }
        }
    }

    private async Task revokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null, string? replacedByToken = null)
    {
        token.Revoked = DateTime.Now;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;

        await _refreshTokenRepository.ChangeAsync(token);        
     }

    private async Task<RefreshToken> rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = await _jwtUtils.GenerateRefreshToken(ipAddress);
        await revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    private async Task<IEnumerable<TransactionViewModel>> getTransactions(string profileCode)
    {
        var transactions = _mapper.Map<IEnumerable<TransactionViewModel>>(await _transactionRepository.GetByProfile(profileCode));

        var actions = await _transactionActionsRepository.GetByProfile(profileCode);

        if (actions != null)
        {
            foreach (var item in transactions)
            {
                item.Permissions = actions.Where(a => a.TransactionCode.Equals(item.Code)).Select(x => x.ActionCode).ToArray();
            }
        }

        return transactions;
    }

    private async Task<IEnumerable<MenuViewModel>> getMenu(string cd_perf)
    {
        var menus = await _menuRepository.GetByProfile(cd_perf);

        var menusVM = new List<MenuViewModel>();

        // Adiciona nível 0
        menusVM.AddRange(_mapper.Map<IEnumerable<MenuViewModel>>(menus.Where(m => m.Level == 0)));

        // Adiciona nível 1
        foreach (var menu0 in menusVM)
        {            
            var items1 = _mapper.Map<IEnumerable<MenuViewModel>>(menus.Where(m => m.GroupingCode.StartsWith(menu0.CdAgrpMenu[..3]) && m.Level == 1));
            if (items1 != null && items1.Count() > 0)
            {
                menu0.Items = items1.ToArray();

                // Adiciona nível 2
                foreach (var menu1 in menu0.Items)
                {
                    var items2 = _mapper.Map<IEnumerable<MenuViewModel>>(menus.Where(m => m.GroupingCode.StartsWith(menu1.CdAgrpMenu[..6]) && m.Level == 2));
                    if (items2 != null && items2.Count() > 0)
                    {
                        menu1.Items = items2.ToArray();
                    }
                }
            }
        }

        return menusVM;
    }
    #endregion private methods
}

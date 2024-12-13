using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Admin.Application.Common.Models;
using Admin.Domain.Interfaces.Repositories.Autorizacao;

namespace Admin.Application.Areas.Autorizacao.E_710_User.Queries.ObterUsuarios;

public record ObterUsuariosQuery : IRequest<ListaVm<UsuarioDto>>;

public class ObterUsuariosQueryHandler : IRequestHandler<ObterUsuariosQuery, ListaVm<UsuarioDto>>
{
    private readonly I_710_UserRepository _repository;
    private readonly IMapper _mapper;

    public ObterUsuariosQueryHandler(I_710_UserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ListaVm<UsuarioDto>> Handle(ObterUsuariosQuery request, CancellationToken cancellationToken)
    {
        var filter = new { };

        var result = await _repository.GetAllAsync(filter);

        return new ListaVm<UsuarioDto>
        {
            Lista = await result.
                        AsQueryable().
                        AsNoTracking().
                        ProjectTo<UsuarioDto>(_mapper.ConfigurationProvider).
                        ToListAsync(cancellationToken)
        };        
    }
}

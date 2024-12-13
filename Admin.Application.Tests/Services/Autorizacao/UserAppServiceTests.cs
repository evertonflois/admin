using AutoMapper;

using Microsoft.Extensions.Options;

using Moq;

using Admin.Application.Common.Mappings;
using Admin.Application.Helpers;
using Admin.Application.Services.Authorization;
using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Domain.Interfaces.Repositories.Authorization;

using Xunit;

namespace Admin.Application.Tests.Services.Authorization;

public class UserAppServiceTests
{
    private UserAppService userAppService;
    private IMapper _mapper;

    public UserAppServiceTests()
    {
        userAppService = new UserAppService(new Mock<IUnitOfWorkRepository>().Object,
                                                  new Mock<IUserRepository>().Object,
                                                  new Mock<IMapper>().Object,
                                                  new Mock<IOptions<AppSettings>>().Object);
        if (_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }
    }

    [Fact]
    public async void Create_Usuario()
    {
        // Arrange
        var successMessage = "Record has been created successfully.";
        var repoMock = new Mock<IUserRepository>();
        var userMock = new Dto.Authorization.User.UserCreateInputModel()
        {
            SubscriberId = Guid.NewGuid().ToString(),
            ProfileCode = "ADM",
            CreationUser = "TEST",
            ChangeUser = "TEST",
            Login = "newUser",
            Email = "newuser@hotmail.com",
            Password = "123456",
            Active = "Y",
            Name = "New User",
            CreationDate = DateTime.Now,
            ChangeDate = DateTime.Now
        };
        repoMock.Setup(s => s.CreateAsync(It.IsAny<User>())).Returns(Task.FromResult(new ResponseBase(0, successMessage)));        

        userAppService = new UserAppService(new Mock<IUnitOfWorkRepository>().Object,
                                                  repoMock.Object,
                                                  _mapper,
                                                  new Mock<IOptions<AppSettings>>().Object);

        // Act
        var result = await userAppService.Create(userMock);        

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Equal(successMessage, result.Description);
        repoMock.Verify(rm => rm.CreateAsync(It.IsAny<User>()), Times.Once());
    }

    [Fact]
    public void Create_AlreadyUser()
    {
        // Arrange
        var repoMock = new Mock<IUserRepository>();
        var usersMock = new List<User>();
        usersMock.Add(new User() { Login = "already" });
        repoMock.Setup(s => s.GetAllAsync(It.IsAny<object>())).Returns(Task.FromResult(usersMock.AsEnumerable()));

        userAppService = new UserAppService(new Mock<IUnitOfWorkRepository>().Object,
                                                  repoMock.Object,
                                                  _mapper,
                                                  new Mock<IOptions<AppSettings>>().Object);

        // Act
        var exception = Assert.ThrowsAsync<AppException>(async ()  =>
                                         await userAppService.Create(new Dto.Authorization.User.UserCreateInputModel() { Login = "already" }));

        // Assert
        Assert.Contains("has being used", exception.Result.Message);
    }
}

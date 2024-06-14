using Moq;
using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.InOut;
using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.AppSessionService;
using ShortageManager.ConsoleApp.Services.Authentication;

namespace ShortageManager.ConsoleApp.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IFileManager> _fileManagerMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _fileManagerMock = new Mock<IFileManager>();
        _userService = new UserService(_userRepositoryMock.Object, _fileManagerMock.Object);
    }

    [Fact]
    public void Login_WhenUsernameIsNullOrEmpty_ShouldReturnFalse()
    {
        var resultWhenNullUsername = _userService.Login(null);
        var resultWhenEmptyUsername = _userService.Login(string.Empty);

        Assert.False(resultWhenNullUsername);
        Assert.False(resultWhenEmptyUsername);
    }

    [Fact]
    public void Login_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        var username = "nonexistentUser";
        _userRepositoryMock.Setup(r => r.GetUser(username)).Returns((User)null);

        var result = _userService.Login(username);

        Assert.False(result);
    }

    [Fact]
    public void Login_WhenUserExists_ShouldReturnTrue()
    {
        var username = "existingUser";
        var user = new User(username, UserRole.User);
        _userRepositoryMock.Setup(r => r.GetUser(username)).Returns(user);

        var result = _userService.Login(username);

        Assert.True(result);
        Assert.Equal(username, AppSession.UserName);
    }

    [Fact]
    public void Register_WhenUserAlreadyExists_ShouldNotAddToDatabaseAgain()
    {
        var username = "existingUser";
        var role = UserRole.User;
        var existingUser = new User(username, role);
        _userRepositoryMock.Setup(r => r.GetUser(username)).Returns(existingUser);
        _userRepositoryMock.Setup(r => r.GetUsers()).Returns(new List<User> { existingUser });

        _userService.Register(username, role);

        _userRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
        _fileManagerMock.Verify(fm => fm.Write(FilePaths.Users, It.IsAny<IEnumerable<User>>()), Times.Never);
    }

    [Fact]
    public void Register_WhenUserSuccessfullyRegisters_ShouldAddUserToDatabase()
    {
        var username = "newUser";
        var role = UserRole.User;
        _userRepositoryMock.Setup(r => r.GetUser(username)).Returns((User)null);
        _userRepositoryMock.Setup(r => r.GetUsers()).Returns(new List<User>());

        _userService.Register(username, role);

        _userRepositoryMock.Verify(r => r.Add(It.Is<User>(u => u.UserName == username && u.Role == role)), Times.Once);
        _fileManagerMock.Verify(fm => fm.Write(FilePaths.Users, It.IsAny<IEnumerable<User>>()), Times.Once);
    }
}
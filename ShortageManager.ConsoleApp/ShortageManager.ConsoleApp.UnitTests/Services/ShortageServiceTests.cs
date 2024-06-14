using Moq;
using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.InOut;
using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.ShortageFilter;
using ShortageManager.ConsoleApp.Services.ShortageService;

namespace ShortageManager.ConsoleApp.UnitTests.Services;

public class ShortageServiceTests
{
    private readonly Mock<IShortageRepository> _shortageRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IFileManager> _fileManagerMock;
    private readonly ShortageService _shortageService;

    public ShortageServiceTests()
    {
        _shortageRepositoryMock = new Mock<IShortageRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _fileManagerMock = new Mock<IFileManager>();
        _shortageService = new ShortageService(_shortageRepositoryMock.Object, _userRepositoryMock.Object, _fileManagerMock.Object);
    }

    [Fact]
    public void GetShortagesByPermissionsAndFilters_WhenUserNotFound_ShouldThrowInvalidOperationException()
    {
        _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<string>())).Returns((User)null);

        Assert.Throws<InvalidOperationException>(() => _shortageService.GetShortagesByPermissionsAndFilters(new Dictionary<ShortageFilterType, List<string>>()));
    }

    [Fact]
    public void GetShortagesByPermissionsAndFilters_WhenUserExists_ShouldReturnFilteredShortages()
    {
        var user = new User("existingUser", UserRole.User);
        var filters = new Dictionary<ShortageFilterType, List<string>>();
        var expectedShortages = new List<Shortage>
        {
            new Shortage("Shortage1", "Description1", RoomType.MeetingRoom, ShortageCategory.Electronics, 1, user)
        };

        _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<string>())).Returns(user);
        _shortageRepositoryMock.Setup(r => r.GetShortagesByPermissionsAndFilters(user, filters)).Returns(expectedShortages);

        var result = _shortageService.GetShortagesByPermissionsAndFilters(filters);

        Assert.Equal(expectedShortages, result);
    }

    [Fact]
    public void Register_WhenShortageDoesNotExist_ShouldAddShortage()
    {
        var shortageToAdd = new Shortage("NewShortage", "Description", RoomType.MeetingRoom, ShortageCategory.Electronics, 1, new User("user1", UserRole.User));
        var shortages = new List<Shortage>();

        _shortageRepositoryMock.Setup(r => r.GetShortages()).Returns(shortages);

        var result = _shortageService.Register(shortageToAdd);

        Assert.True(result);
        _shortageRepositoryMock.Verify(r => r.Add(shortageToAdd), Times.Once);
        _fileManagerMock.Verify(fm => fm.Write(FilePaths.Shortages, shortages), Times.Once);
    }

    [Fact]
    public void Register_WhenExistingShortageHasLowerPriority_ShouldUpdateShortage()
    {
        var user = new User("user1", UserRole.User);
        var existingShortage = new Shortage("ExistingShortage", "Description", RoomType.MeetingRoom, ShortageCategory.Electronics, 1, user);
        var shortageToAdd = new Shortage("ExistingShortage", "Description", RoomType.MeetingRoom, ShortageCategory.Electronics, 2, user);
        var shortages = new List<Shortage> { existingShortage };

        _shortageRepositoryMock.Setup(r => r.GetShortages()).Returns(shortages);

        var result = _shortageService.Register(shortageToAdd);

        Assert.True(result);
        _shortageRepositoryMock.Verify(repo => repo.Update(existingShortage, shortageToAdd), Times.Once);
        _fileManagerMock.Verify(fm => fm.Write(FilePaths.Shortages, shortages), Times.Once);
    }

    [Fact]
    public void Register_WhenExistingShortageHasHigherOrEqualPriority_ShouldNotUpdateShortage()
    {
        var user = new User("user1", UserRole.User);
        var existingShortage = new Shortage("ExistingShortage", "Description", RoomType.MeetingRoom, ShortageCategory.Electronics, 1, user);
        var shortageToAdd = new Shortage("ExistingShortage", "Description", RoomType.MeetingRoom, ShortageCategory.Electronics, 1, user);
        var shortages = new List<Shortage> { existingShortage };

        _shortageRepositoryMock.Setup(r => r.GetShortages()).Returns(shortages);

        var result = _shortageService.Register(shortageToAdd);

        Assert.False(result);
        _shortageRepositoryMock.Verify(repo => repo.Update(It.IsAny<Shortage>(), It.IsAny<Shortage>()), Times.Never);
        _fileManagerMock.Verify(fm => fm.Write(FilePaths.Shortages, shortages), Times.Never);
    }

    [Fact]
    public void Delete_WhenUserIsCreatorOfShortage_ShouldRemoveShortage()
    {
        var user = new User("existingUser", UserRole.User);
        var existingShortage = new Shortage("ShortageToDelete", "Description", RoomType.MeetingRoom, ShortageCategory.Electronics, 1, user);
        var shortages = new List<Shortage> { existingShortage };

        _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<string>())).Returns(user);
        _shortageRepositoryMock.Setup(r => r.GetShortages()).Returns(shortages);

        var result = _shortageService.Delete("ShortageToDelete", RoomType.MeetingRoom);

        Assert.True(result);
        _shortageRepositoryMock.Verify(repo => repo.Delete(existingShortage), Times.Once);
        _fileManagerMock.Verify(fm => fm.Write(FilePaths.Shortages, shortages), Times.Once);
    }

    [Fact]
    public void Delete_WhenUserIsNotCreatorOfShortageButAdministrator_ShouldRemoveShortage()
    {
        var existingShortage = new Shortage("ShortageToDelete", "Description", RoomType.MeetingRoom, ShortageCategory.Electronics, 1, new User("creator", UserRole.User));
        var shortages = new List<Shortage> { existingShortage };

        _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<string>())).Returns(new User("admin", UserRole.Administrator));
        _shortageRepositoryMock.Setup(r => r.GetShortages()).Returns(shortages);

        var result = _shortageService.Delete("ShortageToDelete", RoomType.MeetingRoom);

        Assert.True(result);
        _shortageRepositoryMock.Verify(repo => repo.Delete(existingShortage), Times.Once);
        _fileManagerMock.Verify(fm => fm.Write(FilePaths.Shortages, shortages), Times.Once);
    }

    [Fact]
    public void Delete_WhenUserIsNotCreatorOfShortageNorAdministrator_ShouldNotRemoveShortage()
    {
        var existingShortage = new Shortage("ShortageToDelete", "Description", RoomType.MeetingRoom, ShortageCategory.Electronics, 1, new User("anotherUser", UserRole.User));
        var shortages = new List<Shortage> { existingShortage };

        _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<string>())).Returns(new User("existingUser", UserRole.User));
        _shortageRepositoryMock.Setup(r => r.GetShortages()).Returns(shortages);

        var result = _shortageService.Delete("ShortageToDelete", RoomType.MeetingRoom);

        Assert.False(result);
        _shortageRepositoryMock.Verify(r => r.Delete(It.IsAny<Shortage>()), Times.Never);
        _fileManagerMock.Verify(fm => fm.Write(FilePaths.Shortages, shortages), Times.Never);
    }
}

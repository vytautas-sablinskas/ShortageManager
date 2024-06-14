using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;

namespace ShortageManager.ConsoleApp.UnitTests.DataAccess.Repositories;

public class UserRepositoryTests
{
    private UserRepository _userRepository;

    public UserRepositoryTests()
    {
        var fakeUsers = new List<User>
        {
            new User("Bob", UserRole.User),
            new User("Charlie", UserRole.Administrator)
        };

        _userRepository = new UserRepository(fakeUsers);
    }


    [Fact]
    public void GetUser_WhenGivenUsernameExists_ReturnsCorrectUser()
    {
        var validUsernameWithInvalidCasing = "bob";

        var result = _userRepository.GetUser(validUsernameWithInvalidCasing);

        Assert.Multiple(() =>
        {
            Assert.Equal("Bob", result.UserName);
            Assert.Equal(UserRole.User, result.Role);
        });
    }

    [Fact]
    public void GetUser_WhenGivenNonExistingUsername_ReturnsNull()
    {
        var invalidUsername = "invalid username";

        var result = _userRepository.GetUser(invalidUsername);

        Assert.Null(result);
    }

    [Fact]
    public void GetUsers_ShouldReturnAllUsers()
    {
        var expectedCountOfUsers = 2;

        var result = _userRepository.GetUsers();

        Assert.Equal(expectedCountOfUsers, result.Count());
    }

    [Fact]
    public void Add_ShouldAddUserToRepository()
    {
        var newUser = new User("David", UserRole.Administrator);

        _userRepository.Add(newUser);
        var result = _userRepository.GetUser("David");

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal("David", result.UserName);
            Assert.Equal(UserRole.Administrator, result.Role);
        });
    }
}
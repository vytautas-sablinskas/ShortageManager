using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.ShortageFilter;

namespace ShortageManager.ConsoleApp.UnitTests.DataAccess.Repositories;

public class ShortageRepositoryTests
{
    private readonly static User adminUser = new("user1", UserRole.Administrator);
    private readonly static User simpleUser = new("user2", UserRole.User);
    List<Shortage> shortages = new()
    {
        new Shortage("wireless speaker", "name", RoomType.Kitchen, ShortageCategory.Electronics, -1, adminUser),
        new Shortage("Shortage2", "name", RoomType.Bathroom, ShortageCategory.Food, 11, simpleUser),
        new Shortage("Shortage3", "name", RoomType.Bathroom, ShortageCategory.Food, 3, adminUser),
    };

    private readonly ShortageRepository shortageRepository;

    public ShortageRepositoryTests()
    {
        

        shortageRepository = new ShortageRepository(shortages);
    }

    [Fact]
    public void GetShortagesByPermissionsAndFilters_WhenTitleFilterContainsCorrectTextButDifferentCasing_ReturnsMatchingShortage()
    {
        var filters = new Dictionary<ShortageFilterType, List<string>>
        {
            { ShortageFilterType.Title, new List<string> { "Speaker" }}
        };

        var result = shortageRepository.GetShortagesByPermissionsAndFilters(adminUser, filters);

        Assert.Multiple(() =>
        {
            Assert.Single(result);
            Assert.Equal("wireless speaker", result.First().Title);
        });
    }

    [Fact]
    public void GetShortagesByPermissionsAndFilters_WhenCreatedOnFilterIsInRange_ShouldReturnMatchingShortages()
    {
        var filters = new Dictionary<ShortageFilterType, List<string>>
        {
            { ShortageFilterType.CreatedOn, new List<string> { shortages[1].CreatedOn.AddMinutes(-1).ToString(), shortages[1].CreatedOn.AddMinutes(1).ToString() } }
        };

        var result = shortageRepository.GetShortagesByPermissionsAndFilters(simpleUser, filters);

        Assert.Multiple(() =>
        {
            Assert.Single(result);
            Assert.Contains(result, s => s.Title == "Shortage2");
        });
    }

    [Fact]
    public void GetShortagesByPermissionsAndFilters_WhenCreatedOnFilterIsNotInRange_ShouldNotReturnAnyShortages()
    {
        var filters = new Dictionary<ShortageFilterType, List<string>>
        {
            { ShortageFilterType.CreatedOn, new List<string> { shortages[1].CreatedOn.AddMinutes(1).ToString(), shortages[1].CreatedOn.AddMinutes(2).ToString() } }
        };

        var result = shortageRepository.GetShortagesByPermissionsAndFilters(adminUser, filters);

        Assert.Empty(result);
    }

    [Fact]
    public void GetShortagesByPermissionsAndFilters_WhenCategoryFilterIsValid_ShouldReturnMatchingShortages()
    {   var filters = new Dictionary<ShortageFilterType, List<string>>
        {
            { ShortageFilterType.Category, new List<string> { "Food" } }
        };

        var result = shortageRepository.GetShortagesByPermissionsAndFilters(adminUser, filters);

        Assert.Multiple(() =>
        {
            Assert.True(result.Count() == 2);
            Assert.All(result, s => Assert.True(s.Category == ShortageCategory.Food));
        });
    }

    [Fact]
    public void GetShortagesByPermissionsAndFilters_WhenRoomFilterIsValid_ShouldReturnMatchingShortages()
    {
        var filters = new Dictionary<ShortageFilterType, List<string>>
        {
            { ShortageFilterType.Room, new List<string> { "Kitchen" } }
        };

        var result = shortageRepository.GetShortagesByPermissionsAndFilters(adminUser, filters);

        Assert.Multiple(() =>
        {
            Assert.Single(result);
            Assert.Equal(RoomType.Kitchen, result.First().Room);
        });
    }

    [Fact]
    public void GetShortagesByPermissionsAndFilters_GivenAdminUser_ShouldReturnAllShortages()
    {
        var filters = new Dictionary<ShortageFilterType, List<string>>();

        var result = shortageRepository.GetShortagesByPermissionsAndFilters(adminUser, filters);

        Assert.True(result.Count() == 3);
    }

    [Fact]
    public void GetShortagesByPermissionsAndFilters_GivenSimpleUser_ShouldReturnOnlyUsersCreatedShortages()
    {
        var filters = new Dictionary<ShortageFilterType, List<string>>();

        var result = shortageRepository.GetShortagesByPermissionsAndFilters(simpleUser, filters);

        Assert.Multiple(() =>
        {
            Assert.True(result.Count() == 1);
            Assert.True(result.First().Creator == simpleUser);
        });
    }

    [Fact]
    public void GetShortagesByPermissionsAndFilters_ShouldReturnShortagesOrderedByPriorityInDescendingOrder()
    {
        var filters = new Dictionary<ShortageFilterType, List<string>>();

        var result = shortageRepository.GetShortagesByPermissionsAndFilters(adminUser, filters);

        Assert.Multiple(() =>
        {
            Assert.True(result.Count() == 3);
            Assert.True(result.ElementAt(0).Priority == 10);
            Assert.True(result.ElementAt(1).Priority == 3);
            Assert.True(result.ElementAt(2).Priority == 1);
        });
    }

    [Fact]
    public void GetShortages_ShouldReturnAllShortages()
    {
        var result = shortageRepository.GetShortages();

        Assert.True(result.Count() == 3);
    }

    [Fact]
    public void Add_ShouldAddNewShortage()
    {
        var newShortage = new Shortage("new title", "new name", RoomType.MeetingRoom, ShortageCategory.Electronics, 1, adminUser);

        shortageRepository.Add(newShortage);
        var result = shortageRepository.GetShortages();

        Assert.Multiple(() =>
        {
            Assert.True(result.Count() == 4);
            Assert.Contains(result, r => r.Title == "new title");
        });
    }

    [Fact]
    public void Remove_ShouldRemoveShortage()
    {
        var shortages = shortageRepository.GetShortages();
        var shortageToRemove = shortages.First();

        shortageRepository.Delete(shortageToRemove);

        Assert.DoesNotContain(shortageToRemove, shortages);
    }

    [Fact]
    public void Update_ShouldUpdateShortage()
    {
        var shortages = shortageRepository.GetShortages();
        var oldShortage = shortages.First();
        var updatedShortage = new Shortage("updated title", oldShortage.Name, oldShortage.Room, oldShortage.Category, oldShortage.Priority, oldShortage.Creator);

        shortageRepository.Update(oldShortage, updatedShortage);

        Assert.Multiple(() =>
        {
            Assert.DoesNotContain(oldShortage, shortages);
            Assert.Contains(updatedShortage, shortages);
        });
    }
}

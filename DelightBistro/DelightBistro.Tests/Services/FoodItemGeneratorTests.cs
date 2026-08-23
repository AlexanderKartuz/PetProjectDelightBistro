using DelightBistroMvc.Data.DataModels;
using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Models.DelightBistro;
using DelightBistroMvc.Services.DelightBistro;
using DelightBistroMvc.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;


namespace DelightBistro.Tests.Services
{
    public class FoodItemGeneratorTests
    {
        private FoodItemGenerator _foodItemGenerator; // real object
        private Mock<IFoodItemRepository> _foodItemRepositoryMock;
        private Mock<IMenuRepository> _menuRepositoryMock;
        private Mock<IIngredientGenerator> _ingredientGeneratorMock;
        private Mock<IAuthService> _authServiceMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        [SetUp]
        public void Setup()
        {
            _foodItemRepositoryMock = new Mock<IFoodItemRepository>();
            _menuRepositoryMock = new Mock<IMenuRepository>();
            _ingredientGeneratorMock = new Mock<IIngredientGenerator>();
            _authServiceMock = new Mock<IAuthService>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            _foodItemGenerator = new FoodItemGenerator(
                _foodItemRepositoryMock.Object,
                _menuRepositoryMock.Object,
                _ingredientGeneratorMock.Object,
                _authServiceMock.Object,
                _webHostEnvironmentMock.Object
                );
        }

        [Test]
        public void GetFoodsWithPermission_Admin_CanDeleteAll()
        {
            // Prepare
            _authServiceMock.Setup(x => x.GetUserAsync()).Returns(new UserData
            {
                Id = 1,
                Name = "admin",
                Role = UserRole.Admin,
            });

            var foods = new List<FoodItemViewModel>
            {
                new FoodItemViewModel(){ Id=1, CreatorId=10},
                new FoodItemViewModel(){ Id=2, CreatorId=1},
            };

            // Act
            var result = _foodItemGenerator.GetFoodsWithPermission(foods);

            Assert.That(result.IsAdmin, Is.True);
            Assert.That(result.FoodItems.All(x => x.CanDelete), Is.True);
        }

        [Test]
        public void GetFoodsWithPermission_Creator_CanDeleteOwn()
        {
            // Prepare
            _authServiceMock.Setup(x => x.GetUserAsync()).Returns(new UserData
            {
                Id = 10,
                Name = "creator",
                Role = UserRole.Moderator,
            });

            var foods = new List<FoodItemViewModel>
            {
                new FoodItemViewModel(){ Id=1, CreatorId=10},
                new FoodItemViewModel(){ Id=2, CreatorId=1},
            };

            // Act
            var result = _foodItemGenerator.GetFoodsWithPermission(foods);

            Assert.That(result.IsAdmin, Is.False);
            Assert.That(result.FoodItems[0].CanDelete, Is.True);
            Assert.That(result.FoodItems[1].CanDelete, Is.False);
        }

        [Test]
        public void DeleteFoodItem_CallsDelete()
        {
            const int id = 10;

            _foodItemGenerator.DeleteFoodItem(id);

            _foodItemRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once());
            _foodItemRepositoryMock.Verify(r => r.RemoveAsync(It.IsAny<FoodItemData>()), Times.Never());
            _foodItemRepositoryMock.Verify(r => r.GetAsync(It.IsAny<int>()), Times.Never());
        }

        [Test]
        public void ChangeFoodItemData_IfNotFound()
        {
            var changedFoodItem = new CreateFoodItemViewModel()
            {
                Id = 1,
                Name = "Salad",
            };

            _ingredientGeneratorMock.Setup(ig =>
            ig.GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(changedFoodItem))
                .Returns(new List<FoodItemIngredientData>());

            _foodItemRepositoryMock.Setup(r =>
            r.GetByIdIncludeMenuAndIngredientsLinks(changedFoodItem.Id))
                .Returns((FoodItemData?)null);

            Assert.Throws<InvalidOperationException>(new TestDelegate(() =>
            _foodItemGenerator.ChangeFoodItemData(changedFoodItem)));
        }

        [Test]
        public void ConvertToFoodItemVM()
        {
            // Prepare
            var foodData = new FoodItemData
            {
                Id = 10,
                Name = "Salad",
                Price = 10,
                ImgURL = "/img.png",
                MenuData = null,
                Creator = new UserData { Name = "admin" },
                CreatorId = 1,
            };

            _ingredientGeneratorMock.Setup(ig =>
                ig.MapSelectedIngredients(foodData))
                .Returns(new List<CreateIngredientViewModel>
                {
                    new CreateIngredientViewModel(){ Id = 1, IsSelected = true},
                    new CreateIngredientViewModel(){ Id = 3, IsSelected = true},
                });

            // Act
            var result = _foodItemGenerator.ConvertToFoodItemVM(foodData);

            // Assert
            Assert.That(result.Id, Is.EqualTo(10));
            Assert.That(result.MenuType, Is.EqualTo("Общее меню"));
            Assert.That(result.IngredientsList, Has.Count.EqualTo(2));
            _ingredientGeneratorMock.Verify(ig => ig.MapSelectedIngredients(foodData), Times.Once);
            _ingredientGeneratorMock.Verify(
                ig => ig.GenerateIngredientsViewModelFromFoodItemData(It.IsAny<FoodItemData>()),
                Times.Never);
        }

        [Test]
        public void GetFoodItemStatsViewModels_RepositoryMap()
        {
            _foodItemRepositoryMock.Setup(fr => fr.GetFoodItemStats())
                .Returns(new List<FoodItemStatsDataModel>
                {
                    new FoodItemStatsDataModel
                    {
                        FoodItemName="Foo",
                        IngredientCount=5,
                        FoodItemPrice=20,
                        TotalPriceIngredient=10,
                        Profit=10,
                    },
                });

            var result = _foodItemGenerator.GetFoodItemStatsViewModels();

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].FoodItemName, Is.EqualTo("Foo"));
            Assert.That(result[0].IngredientCount, Is.EqualTo(5));
            Assert.That(result[0].FoodItemPrice, Is.EqualTo(20));
            Assert.That(result[0].TotalPriceIngredient, Is.EqualTo(10));
            Assert.That(result[0].Profit, Is.EqualTo(10));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ConvertToCreateFoodItemVM(bool exist)
        {
            FoodItemData? foodData = exist ? (new FoodItemData
            {
                Id = 1,
                Name = "Foo",
                Price = 20,
                ImgURL = "/img.png",
            }) : null;

            _ingredientGeneratorMock.Setup(ig =>
            ig.GenerateIngredientsViewModelFromFoodItemData(foodData))
                .Returns(new List<CreateIngredientViewModel>
                {
                    new CreateIngredientViewModel(){ Id=1, Name ="Foo2"},
                    new CreateIngredientViewModel(){ Id=2, Name ="Foo3"},

                });
            _menuRepositoryMock.Setup(mr => mr.GetAllAsync())
                .Returns(new List<MenuData>
                {
                    new MenuData { Id = 1, Name = "Soops" },
                    new MenuData { Id = 2, Name = "Soops2" },
                    new MenuData { Id = 3, Name = "Soops3" }

                });

            // Act
            var result = _foodItemGenerator.ConvertToCreateFoodItemVM(foodData);

            // Assert
            if (exist)
            {
                Assert.That(result.Id, Is.EqualTo(1));
                Assert.That(result.Name, Is.EqualTo("Foo"));
                Assert.That(result.Price, Is.EqualTo(20));
                Assert.That(result.IngredientsList, Has.Count.EqualTo(2));
                Assert.That(result.Menus, Has.Count.EqualTo(3));
            }
            else
            {
                Assert.That(result.Id, Is.EqualTo(0));
                Assert.That(result.Name, Is.Null);
                Assert.That(result.IngredientsList, Has.Count.EqualTo(2));
                Assert.That(result.Menus, Has.Count.EqualTo(3));
            }
        }
    }
}

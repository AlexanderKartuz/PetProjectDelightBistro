using DelightBistroMvc.Data.DataModels;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Models.DelightBistro;
using DelightBistroMvc.Services.DelightBistro;
using DelightBistroMvc.Services.Interfaces;
using Moq;

namespace DelightBistro.Tests.Services
{
    public class IngredientGeneratorTests
    {
        private IngredientGenerator _ingredientGenerator;
        private Mock<IIngredientsRepository> _ingredientRepositoryMock;
        private Mock<IAuthService> _authServiceMock;

        [SetUp]
        public void Setup()
        {
            _ingredientRepositoryMock = new Mock<IIngredientsRepository>();
            _authServiceMock = new Mock<IAuthService>();

            _ingredientGenerator = new IngredientGenerator(
                _ingredientRepositoryMock.Object,
                _authServiceMock.Object);
        }

        [Test]
        public void GetSelected_ReturnOnlySelected()
        {
            // Prepare
            var ingredients = new List<CreateIngredientViewModel>
            {
                new CreateIngredientViewModel(){Id=1, Name="Lime", IsSelected=true},
                new CreateIngredientViewModel(){Id=2, Name="Lime", IsSelected=false},
                new CreateIngredientViewModel(){Id=3, Name="Lime", IsSelected=true},
            };

            // Act
            var result = _ingredientGenerator
                .GetSelectedCreateIngredientViewModelFromIngredientsList(ingredients);

            // Assert
            Assert.That(result.Select(x => x.Id), Is.EqualTo(new[] { 1, 3 }));
        }

        [Test]
        public void GetSelectedIngredient_ReturnEmptyList()
        {
            var result = _ingredientGenerator
                .GetSelectedCreateIngredientViewModelFromIngredientsList(new List<CreateIngredientViewModel>());

            Assert.That(result, Is.Empty);
        }

        [Test]
        [TestCase(5, 5)]
        [TestCase(0, 10)]
        [TestCase(-6, 10)]
        public void GetLinks_SetsQuatityCorrectly(decimal quantity, decimal expectedQuantity)
        {
            // Prepare
            var createFoodItemViewModel = new CreateFoodItemViewModel
            {
                IngredientsList =
                {
                    new(){ Id=1, IsSelected=true, Quantity=quantity},
                    new(){ Id=2, IsSelected=false, Quantity=100}, // not selected
                }
            };

            // Act
            var links = _ingredientGenerator
                .GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(createFoodItemViewModel);

            // Assert
            Assert.That(links, Has.Count.EqualTo(1));
            Assert.That(links[0].IngredientDataId, Is.EqualTo(1));
            Assert.That(links[0].QuantityOfIngredients, Is.EqualTo(expectedQuantity));
        }

        [Test]
        public void CreateIngredientData_WithAuthUser()
        {
            //Prepare
            var user = new UserData { Id = 1, Name = "admin" };
            _authServiceMock.Setup(x => x.GetUserAsync()).Returns(user); // admin

            var createIngredientVm = new CreateIngredientViewModel
            {
                Name = "Cheese",
                Price = 20,
            };

            // Act
            _ingredientGenerator.CreateIngredientData(createIngredientVm);

            // Verify
            _ingredientRepositoryMock.Verify(x =>
                x.AddAsync(It.Is<IngredientData>(i =>
                    i.Name == "Cheese"
                    && i.Price == 20
                    && i.Creator == user)),
                Times.Once);
        }

        [Test]
        public void GenerateIngredientsViewModelFromFoodItemData_WhenFoodItemIsNool()
        {
            var ingredientDatas = new List<IngredientData>
            {
                new IngredientData() {Id = 1, Name = "ingredient1",},
                new IngredientData() {Id = 2, Name = "ingredient2",},
            };

            _ingredientRepositoryMock.Setup(ir => ir.GetAllAsync())
                .Returns(ingredientDatas);

            FoodItemData? foodData = null;

            var result = _ingredientGenerator
                .GenerateIngredientsViewModelFromFoodItemData(foodData);

            Assert.Multiple(new Action(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));
                Assert.That(result.All(i => i.IsSelected == false), Is.True);
                Assert.That(result.All(i => i.Quantity == 10), Is.True);
            }));
        }

        [Test]
        public void GenerateIngredientsViewModelFromFoodItemData_WhenFoodExists()
        {
            var ingredientDatas = new List<IngredientData>
            {
                new IngredientData() {Id = 1, Name = "ingredient1",},
                new IngredientData() {Id = 2, Name = "ingredient2",},
                new IngredientData() {Id = 3, Name = "ingredient3",},
            };

            _ingredientRepositoryMock
                .Setup(ir => ir.GetAllAsync())
                .Returns(ingredientDatas);

            // IsSelected только из FoodItemIngredientDatas (IngredientsList не учитывается)
            var foodData = new FoodItemData
            {
                Id = 5,
                Name = "Food1",
                IngredientsList = new List<IngredientData>
                {
                    ingredientDatas[0],
                    ingredientDatas[1],
                },
                FoodItemIngredientDatas = new List<FoodItemIngredientData>
                {
                    new FoodItemIngredientData(){IngredientDataId = 1, QuantityOfIngredients = 20,},
                }
            };

            var result = _ingredientGenerator.GenerateIngredientsViewModelFromFoodItemData(foodData);

            Assert.Multiple(new Action(() =>
            {
                Assert.That(result, Has.Count.EqualTo(3));

                var ingredient1 = result.Single(i => i.Id == 1);
                Assert.That(ingredient1.IsSelected, Is.True);
                Assert.That(ingredient1.Quantity, Is.EqualTo(20));
                Assert.That(ingredient1.Name, Is.EqualTo("ingredient1"));

                var ingredient2 = result.Single(i => i.Id == 2);
                Assert.That(ingredient2.IsSelected, Is.False);
                Assert.That(ingredient2.Quantity, Is.EqualTo(10));
                Assert.That(ingredient2.Name, Is.EqualTo("ingredient2"));

                var ingredient3 = result.Single(i => i.Id == 3);
                Assert.That(ingredient3.IsSelected, Is.False);
                Assert.That(ingredient3.Quantity, Is.EqualTo(10));
                Assert.That(ingredient3.Name, Is.EqualTo("ingredient3"));
            }));
        }

        [Test]
        public void MapSelectedIngredients_ReturnsOnlyLinks()
        {
            var shrimp = new IngredientData { Id = 1, Name = "Креветки" };
            var lime = new IngredientData { Id = 2, Name = "Лайм" };

            var foodData = new FoodItemData
            {
                Id = 5,
                FoodItemIngredientDatas = new List<FoodItemIngredientData>
                {
                    new()
                    {
                        IngredientDataId = 1,
                        IngredientData = shrimp,
                        QuantityOfIngredients = 50,
                    },
                    new()
                    {
                        IngredientDataId = 2,
                        IngredientData = lime,
                        QuantityOfIngredients = 10,
                    },
                }
            };

            var result = _ingredientGenerator.MapSelectedIngredients(foodData);

            Assert.Multiple(new Action(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));
                Assert.That(result.All(i => i.IsSelected), Is.True);
                Assert.That(result[0].Name, Is.EqualTo("Креветки"));
                Assert.That(result[0].Quantity, Is.EqualTo(50));
                Assert.That(result[1].Name, Is.EqualTo("Лайм"));
                Assert.That(result[1].Quantity, Is.EqualTo(10));
            }));

            _ingredientRepositoryMock.Verify(r => r.GetAllAsync(), Times.Never);
        }

        [Test]
        public void GenerateIngredientsViewModel_WhenIngredientsListIsEmpty_ReturnEmptyList()
        {
            _ingredientRepositoryMock
                .Setup(ir => ir.GetAllAsync())
                .Returns(new List<IngredientData>());

            var result = _ingredientGenerator
                .GenerateIngredientsViewModelFromFoodItemData();

            Assert.That(result, Is.Empty);
        }
    }
}

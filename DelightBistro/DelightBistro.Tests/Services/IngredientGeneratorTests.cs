using DelightBistroMvc.Data.DataModels;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces;
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
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void Setup()
        {
            _ingredientRepositoryMock = new Mock<IIngredientsRepository>();
            _authServiceMock = new Mock<IAuthService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _ingredientGenerator = new IngredientGenerator(
                _ingredientRepositoryMock.Object,
                _authServiceMock.Object,
                _unitOfWorkMock.Object);
        }

        [Test]
        public void GetSelected_ReturnOnlySelected()
        {
            var ingredients = new List<CreateIngredientViewModel>
            {
                new CreateIngredientViewModel(){Id=1, Name="Lime", IsSelected=true},
                new CreateIngredientViewModel(){Id=2, Name="Lime", IsSelected=false},
                new CreateIngredientViewModel(){Id=3, Name="Lime", IsSelected=true},
            };

            var result = _ingredientGenerator
                .GetSelectedCreateIngredientViewModelFromIngredientsList(ingredients);

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
            var createFoodItemViewModel = new CreateFoodItemViewModel
            {
                IngredientsList =
                {
                    new(){ Id=1, IsSelected=true, Quantity=quantity},
                    new(){ Id=2, IsSelected=false, Quantity=100},
                }
            };

            var links = _ingredientGenerator
                .GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(createFoodItemViewModel);

            Assert.That(links, Has.Count.EqualTo(1));
            Assert.That(links[0].IngredientDataId, Is.EqualTo(1));
            Assert.That(links[0].QuantityOfIngredients, Is.EqualTo(expectedQuantity));
        }

        [Test]
        public async Task CreateIngredientData_WithAuthUser()
        {
            var user = new UserData { Id = 1, Name = "admin" };
            _authServiceMock
                .Setup(x => x.GetUserAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var createIngredientVm = new CreateIngredientViewModel
            {
                Name = "Cheese",
                Price = 20,
            };

            await _ingredientGenerator.CreateIngredientDataAsync(createIngredientVm);

            _ingredientRepositoryMock.Verify(x =>
                x.AddAsync(It.Is<IngredientData>(i =>
                    i.Name == "Cheese"
                    && i.Price == 20
                    && i.Creator == user),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            _unitOfWorkMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task GenerateIngredientsViewModelFromFoodItemData_WhenFoodItemIsNool()
        {
            var ingredientDatas = new List<IngredientData>
            {
                new IngredientData() {Id = 1, Name = "ingredient1",},
                new IngredientData() {Id = 2, Name = "ingredient2",},
            };

            _ingredientRepositoryMock
                .Setup(ir => ir.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(ingredientDatas);

            FoodItemData? foodData = null;

            var result = await _ingredientGenerator
                .GenerateIngredientsViewModelFromFoodItemDataAsync(foodData);

            Assert.Multiple(new Action(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));
                Assert.That(result.All(i => i.IsSelected == false), Is.True);
                Assert.That(result.All(i => i.Quantity == 10), Is.True);
            }));
        }

        [Test]
        public async Task GenerateIngredientsViewModelFromFoodItemData_WhenFoodExists()
        {
            var ingredientDatas = new List<IngredientData>
            {
                new IngredientData() {Id = 1, Name = "ingredient1",},
                new IngredientData() {Id = 2, Name = "ingredient2",},
                new IngredientData() {Id = 3, Name = "ingredient3",},
            };

            _ingredientRepositoryMock
                .Setup(ir => ir.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(ingredientDatas);

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

            var result = await _ingredientGenerator
                .GenerateIngredientsViewModelFromFoodItemDataAsync(foodData);

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

            _ingredientRepositoryMock.Verify(
                r => r.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Test]
        public async Task GenerateIngredientsViewModel_WhenIngredientsListIsEmpty_ReturnEmptyList()
        {
            _ingredientRepositoryMock
                .Setup(ir => ir.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IngredientData>());

            var result = await _ingredientGenerator
                .GenerateIngredientsViewModelFromFoodItemDataAsync();

            Assert.That(result, Is.Empty);
        }
    }
}

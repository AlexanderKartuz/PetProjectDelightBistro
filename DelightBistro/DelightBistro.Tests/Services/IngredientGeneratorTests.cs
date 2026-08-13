using Moq;
using Newtonsoft.Json.Bson;
using NUnit.Framework.Internal;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Models.DelightBistro;
using DelightBistroMvc.Services.DelightBistro;
using DelightBistroMvc.Services.Interfaces;

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

            Assert.That(result, Is.Empty);//
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
                    new(){ Id=1, IsSelected=false, Quantity=100}, // not selected
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
            _authServiceMock.Setup(x => x.GetUser()).Returns(user); // admin

            var createIngredientVm = new CreateIngredientViewModel
            {
                Name = "Cheese",
                Price = 20,
            };

            // Act
            _ingredientGenerator.CreateIngredientData(createIngredientVm);

            // Verify
            _ingredientRepositoryMock.Verify(x =>
                x.Add(It.Is<IngredientData>(i =>
                    i.Name == "Cheese"
                    && i.Price == 20
                    && i.Creator == user)),
                Times.Once);
        }
    }
}
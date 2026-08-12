using MazeCore.Cells;
using MazeCore.Cells.Interfaces;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;
using Moq;
using NUnit.Framework;

namespace FirstConsoleApp.Tests.MazeStuff.Cells
{
    public class RestTest
    {
        private Rest _rest;
        private Mock<IBaseCharacter> _baseCharacterMock;
        private Mock<IMaze> _mazeMock;

        [SetUp]
        public void Setup()
        {
            _mazeMock = new Mock<IMaze>();
            _mazeMock
                .Setup(x => x.Surface.Remove(It.IsAny<IBaseCell>()));
            _mazeMock
                .Setup(x => x.Surface.Add(It.IsAny<IBaseCell>()));
            _mazeMock
                .Setup(x => x.EventHistory.Add(It.IsAny<string>()));
            var maze = _mazeMock.Object;

            _baseCharacterMock = new Mock<IBaseCharacter>();
            _baseCharacterMock.SetupProperty(x => x.Hp);

            _rest = new Rest(maze); // ONE REAL OBJECT
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(12, 13)]
        [TestCase(100, 101)]
        public void Interaction_CoinAreIncrease(int initialRest, int resultHP)
        {
            // Prepare
            var baseCharacter = _baseCharacterMock.Object;
            baseCharacter.Hp = initialRest;

            // Act
            _rest.Interaction(baseCharacter);

            // Assert
            Assert.That(baseCharacter.Hp, Is.EqualTo(resultHP), $"HP count must be {resultHP}");
        }

        [Test]
        public void Interaction_CanStepOnCoin()
        {
            // Prepare

            // Act
            var result = _rest.Interaction(_baseCharacterMock.Object);

            // Assert
            Assert.That(result, Is.EqualTo(true), $"You can step on Rest");
        }

        [Test]
        public void Interaction_CoinWasReplacedByGround()
        {
            // Prepare

            // Act
            _rest.Interaction(_baseCharacterMock.Object);

            // Assert
            _mazeMock.Verify(x => x.Surface.Remove(_rest), Times.Once());
            _mazeMock.Verify(x => x.Surface.Add(It.IsAny<Ground>()), Times.Once());
        }
    }
}
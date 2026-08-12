using MazeCore.Cells;
using MazeCore.Cells.Interfaces;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;
using Moq;
using NUnit.Framework;

namespace FirstConsoleApp.Tests.MazeStuff.Cells
{
    public class CoinTest
    {
        private Coin _coin;
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
            _baseCharacterMock.SetupProperty(x => x.Coins);

            _coin = new Coin(maze); // ONE REAL OBJECT
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(12, 13)]
        [TestCase(100, 101)]
        public void Interaction_CoinAreIncrease(int initialCoins, int resultCoins)
        {
            // Prepare
            var baseCharacter = _baseCharacterMock.Object;
            baseCharacter.Coins = initialCoins;

            // Act
            _coin.Interaction(baseCharacter);

            // Assert
            Assert.That(baseCharacter.Coins, Is.EqualTo(resultCoins), $"Coins count must be {resultCoins}");
        }

        [Test]
        public void Interaction_CanStepOnCoin()
        {
            // Prepare

            // Act
            var result = _coin.Interaction(_baseCharacterMock.Object);

            // Assert
            Assert.That(result, Is.EqualTo(true), $"You can step in Coin");
        }

        [Test]
        public void Interaction_CoinWasReplacedByGround()
        {
            // Prepare

            // Act
            _coin.Interaction(_baseCharacterMock.Object);

            // Assert
            _mazeMock.Verify(x => x.Surface.Remove(_coin), Times.Once());
            _mazeMock.Verify(x => x.Surface.Add(It.IsAny<Ground>()), Times.Once());
        }
    }
}

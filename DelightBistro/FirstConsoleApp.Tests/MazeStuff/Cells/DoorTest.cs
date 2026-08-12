using MazeCore.Cells;
using MazeCore.Cells.Interfaces;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

using Moq;

using NUnit.Framework;


namespace FirstConsoleApp.Tests.MazeStuff.Cells
{
    public class DoorTest
    {
        private Doors _door;
        private Mock<IBaseCharacter> _baseCharacterMock;
        private Mock<IMaze> _mazeMock;
        private Mock<IInputReader> _inputReaderMock;

        private const string OPTION_OPEN_WITH_KEY = "1";
        private const string OPTION_OPEN_WITH_COIN = "2";
        private const string OPTION_CANCEL = "3";

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

            _inputReaderMock = new Mock<IInputReader>();
            var _inputReader = _inputReaderMock.Object;

            _mazeMock
                .Setup(x => x.InputReader)
                .Returns(_inputReader);

            var _maze = _mazeMock.Object;


            _baseCharacterMock = new Mock<IBaseCharacter>();

            _door = new Doors(_maze); // real object
        }

        [Test]
        public void Interaction_UserCancels_ReturnsFalse()
        {
            //arrange
            var _baseCharacter = _baseCharacterMock.Object;
            _inputReaderMock
                .Setup(x => x.ReadLine())
                .Returns(OPTION_CANCEL);

            //act
            var result = _door.Interaction(_baseCharacter);

            //assert
            Assert.That(result, Is.False);

            _mazeMock.Verify(m => m.Surface.Remove(_door), Times.Never);
            _mazeMock.Verify(m => m.Surface.Add(It.IsAny<Ground>()), Times.Never);
            _mazeMock.Verify(m => m.EventHistory.Add(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Interaction_WhenOpenedWithKey_DoorWasReplacedByGround_ReturnsTrue()
        {
            // Arrange
            _baseCharacterMock.Setup(b => b.HasKey(It.IsAny<int>())).Returns(true);
            _baseCharacterMock.Setup(b => b.UseKey(It.IsAny<int>()))
                .Callback<int>(amount => _baseCharacterMock.Object.Keys -= amount);

            _inputReaderMock
                .Setup(x => x.ReadLine())
                .Returns(OPTION_OPEN_WITH_KEY);

            // Act
            var result = _door.Interaction(_baseCharacterMock.Object);

            // Assert
            Assert.That(result, Is.True);

            _baseCharacterMock.Verify(b => b.UseKey(It.IsAny<int>()), Times.Once);

            _mazeMock.Verify(m => m.Surface.Remove(_door), Times.Once);
            _mazeMock.Verify(m => m.Surface.Add(It.Is<Ground>(g => g.X == _door.X && g.Y == _door.Y)), Times.Once);
            _mazeMock.Verify(m => m.EventHistory.Add(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Interaction_WhenNoKey_ReturnsFalse()
        {
            //arrange
            _baseCharacterMock.SetupProperty(b => b.Keys);
            _baseCharacterMock.Setup(b => b.HasKey(It.IsAny<int>())).Returns(false);
            var _baseCharacter = _baseCharacterMock.Object;

            _inputReaderMock
                .SetupSequence(x => x.ReadLine())
                .Returns(OPTION_OPEN_WITH_KEY)
                .Returns(OPTION_CANCEL);

            //act
            var result = _door.Interaction(_baseCharacter);

            //assert
            Assert.That(result, Is.False);

            _baseCharacterMock.Verify(b => b.UseKey(It.IsAny<int>()), Times.Never);

            _mazeMock.Verify(m => m.Surface.Remove(It.IsAny<IBaseCell>()), Times.Never);
            _mazeMock.Verify(m => m.Surface.Add(It.IsAny<Ground>()), Times.Never);
            _mazeMock.Verify(m => m.EventHistory.Add(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [TestCase(1, 0)]
        [TestCase(5, 4)]
        [TestCase(21, 20)]
        public void Interaction_WhenOpenedWithKeys_DecreaseKeys(int initialKeys, int resultKeys)
        {
            //arrange
            _baseCharacterMock.SetupProperty(b => b.Keys, initialKeys);
            _baseCharacterMock.Setup(b => b.HasKey(It.IsAny<int>())).Returns(true);
            _baseCharacterMock
                .Setup(b => b.UseKey(It.IsAny<int>()))
                .Callback<int>(amount => _baseCharacterMock.Object.Keys -= amount);

            var _baseCharacter = _baseCharacterMock.Object;
            
            _inputReaderMock
                .Setup(x => x.ReadLine())
                .Returns(OPTION_OPEN_WITH_KEY);

            //act
            var result = _door.Interaction(_baseCharacter);

            //assert
            Assert.That(_baseCharacter.Keys, Is.EqualTo(resultKeys));
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void Interaction_WhenOpenedWithCoins_DoorWasReplacedByGround_ReturnsTrue(int initialCoins)
        {
            // Arrange
            _baseCharacterMock.SetupProperty(b => b.Coins, initialCoins);
            _baseCharacterMock.Setup(b => b.HasKey(It.IsAny<int>())).Returns(false);
            _baseCharacterMock.Setup(b => b.SpendCoins(It.IsAny<int>()))
                .Callback<int>(amount => _baseCharacterMock.Object.Coins -= amount);

            var character = _baseCharacterMock.Object;

            _inputReaderMock
                .Setup(x => x.ReadLine())
                .Returns(OPTION_OPEN_WITH_COIN);

            // Act
            var result = _door.Interaction(character);

            // Assert
            Assert.That(result, Is.True);

            _mazeMock.Verify(m => m.Surface.Remove(_door), Times.Once());
            _mazeMock.Verify(m => m.Surface.Add(It.Is<Ground>(g => g.X == _door.X && g.Y == _door.Y)), Times.Once);
            _mazeMock.Verify(m => m.EventHistory.Add(It.IsAny<string>()), Times.Once);

            _baseCharacterMock.Verify(b => b.SpendCoins(It.IsAny<int>()), Times.Once);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Interaction_OpenWithCoins_NotEnoughCoins_ReturnsFalse(int initialCoins)
        {
            //arrange
            _baseCharacterMock.SetupProperty(b => b.Coins, initialCoins);
            _baseCharacterMock
                .Setup(b => b.SpendCoins(It.IsAny<int>()))
                .Callback<int>(amount => _baseCharacterMock.Object.Coins -= amount);

            var _baseCharacter = _baseCharacterMock.Object;

            _inputReaderMock
                .SetupSequence(x => x.ReadLine())
                .Returns(OPTION_OPEN_WITH_COIN)
                .Returns(OPTION_CANCEL);

            //act
            var result = _door.Interaction(_baseCharacter);

            //assert
            Assert.That(result, Is.False);
            _baseCharacterMock.Verify(b => b.SpendCoins(It.IsAny<int>()), Times.Never);

            _mazeMock.Verify(m => m.Surface.Remove(It.IsAny<IBaseCell>()), Times.Never);
            _mazeMock.Verify(m => m.Surface.Add(It.IsAny<Ground>()), Times.Never);

            _mazeMock.Verify(m => m.EventHistory.Add(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [TestCase(2, 1)]
        [TestCase(10, 9)]
        [TestCase(6, 5)]
        public void Interaction_WhenOpenedWithCoins_DecreaseCoins(int initialCoins, int resultCoins)
        {
            //arrange
            _baseCharacterMock.SetupProperty(b => b.Coins, initialCoins);
            _baseCharacterMock
                .Setup(b => b.SpendCoins(It.IsAny<int>()))
                .Callback<int>(amount => _baseCharacterMock.Object.Coins -= amount);

            var _baseCharacter = _baseCharacterMock.Object;
  
            _inputReaderMock
                .Setup(x => x.ReadLine())
                .Returns(OPTION_OPEN_WITH_COIN);
            //act
            var result = _door.Interaction(_baseCharacter);

            //assert
            Assert.That(result, Is.True);
            Assert.That(_baseCharacter.Coins, Is.EqualTo(resultCoins));
        }
    }
}

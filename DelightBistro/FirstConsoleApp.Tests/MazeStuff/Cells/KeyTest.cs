using MazeCore.Cells;
using MazeCore.Cells.Interfaces;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;
using Moq;
using NUnit.Framework;


namespace FirstConsoleApp.Tests.MazeStuff.Cells
{
    public class KeyTest
    {
        private Key _key;
        private Mock<IBaseCharacter> _baseCharacterMock;
        private Mock<IMaze> _mazeMock;
        private Mock<IMazeSoundPlayer> _soundPlayerMock;

        [SetUp]
        public void Setup()
        {
            _mazeMock = new Mock<IMaze>();
            _soundPlayerMock = new Mock<IMazeSoundPlayer>();
            _mazeMock
                .Setup(x => x.Surface.Remove(It.IsAny<IBaseCell>()));
            _mazeMock
                .Setup(x => x.Surface.Add(It.IsAny<IBaseCell>()));
            _mazeMock
                .Setup(x => x.EventHistory.Add(It.IsAny<string>()));
            _soundPlayerMock
                .Setup(x => x.PlayMusic(It.IsAny<string>(), It.IsAny<float>(), It.IsAny<bool>()));
            var maze = _mazeMock.Object;
            _key = new Key(maze);
            _baseCharacterMock = new Mock<IBaseCharacter>();
        }

        [Test]
        public void Iteraction_CanStepOnKey()
        {
            // Prepare

            // Act
            var result = _key.Interaction(_baseCharacterMock.Object);

            // Assert
            Assert.That(result, Is.EqualTo(true), $"You can step on Key");
        }

        [Test]
        public void Iteraction_CanHearMusic()
        {

            // Prepare

            // Act
            _key.Interaction(_baseCharacterMock.Object);

            // Assert
            _soundPlayerMock.Verify(x => x.PlayMusic(It.IsAny<string>(), It.IsAny<float>(), It.IsAny<bool>()), Times.Never());
        }

        [Test]
        public void Iteraction_CanStoreEventHistory()
        {

            // Prepare

            // Act
            _key.Interaction(_baseCharacterMock.Object);

            // Assert
            _mazeMock.Verify(x => x.EventHistory.Add(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Collect_CanObtainKey()
        {
            // Prepare

            // Act
            var result = _key.Collect(_baseCharacterMock.Object);

            // Assert
            Assert.That(result, Is.EqualTo(true), $"You can collect on Key");
        }

        [Test]
        public void Collect_TryingObtainKey()
        {
            // Prepare

            // Act
            _key.Interaction(_baseCharacterMock.Object);

            // Assert
            _baseCharacterMock.Verify(x => x.CollectKey(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void ReplaceKeyToGround_KeyWasReplaceToGround()
        {
            // Prepare

            // Act
            _key.Interaction(_baseCharacterMock.Object);

            // Assert
            _mazeMock.Verify(x => x.Surface.Remove(_key), Times.Once());
            _mazeMock.Verify(x => x.Surface.Add(It.IsAny<Ground>()), Times.Once());
        }
    }
}

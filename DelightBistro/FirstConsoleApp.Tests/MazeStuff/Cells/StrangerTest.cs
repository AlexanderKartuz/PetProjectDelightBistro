using MazeCore.Cells;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;
using Moq;
using NUnit.Framework;


namespace FirstConsoleApp.Tests.MazeStuff.Cells
{
    public class StrangerTest
    {
        private Stranger _stranger;
        private Mock<IBaseCharacter> _baseCharacterMock;
        private Mock<IMaze> _mazeMock;
        private Mock<Random> _randomMock;

        [SetUp]
        public void Setup()
        {
            _mazeMock = new Mock<IMaze>();
            _mazeMock
                .Setup(x => x.EventHistory.Add(It.IsAny<string>()));
            var maze = _mazeMock.Object;
            _randomMock = new Mock<Random>();
            Random random = new Random();
            _stranger = new Stranger(maze, _randomMock.Object);
            _baseCharacterMock = new Mock<IBaseCharacter>();
        }

        [Test]
        public void Iteraction_CanMeetStranger()
        {
            // Prepare

            // Act
            var result = _stranger.Interaction(_baseCharacterMock.Object);

            // Assert
            Assert.That(result, Is.EqualTo(true), $"You can iteract with Stranger");
        }

        [Test]
        public void Iteraction_CanStoreEventHistory()
        {

            // Prepare

            // Act
            _stranger.Interaction(_baseCharacterMock.Object);

            // Assert
            _mazeMock.Verify(x => x.EventHistory.Add(It.IsAny<string>()), Times.Exactly(3));
        }
    }
}

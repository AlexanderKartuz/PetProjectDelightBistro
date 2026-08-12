
using MazeCore.Cells;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;
using Moq;
using NUnit.Framework;

namespace FirstConsoleApp.Tests.MazeStuff.Cells
{
    public class ExitSecretRoomTest
    {
        private ExitSecretRoom _exitSecretRoom;
        private Mock<IBaseCharacter> _baseCharacterMock;
        private Mock<IMaze> _mazeMock;

        [SetUp]
        public void ExitSecretRoomSetup()
        {
            _mazeMock = new Mock<IMaze>();
            _baseCharacterMock = new Mock<IBaseCharacter>();

            var maze = _mazeMock.Object;
            _exitSecretRoom = new ExitSecretRoom(maze);

            _mazeMock.Setup(x => x.EventHistory.Add(It.IsAny<string>()));
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        [TestCase(4, false)]

        public void Interaction_ReturnCorrectValueBasedOnCallCount(int countCall, bool expected)
        {
            // Prepare
            var baseCharacter = _baseCharacterMock.Object;

            // Act
            bool? actual = null;
            for (int i = 0; i < countCall; i++)
            {
                actual = _exitSecretRoom.Interaction(baseCharacter);
            }

            // Assert
            Assert.That(expected, Is.EqualTo(actual),
                $"Interacton must be {expected} if called {countCall} times");
        }

        [Test]
        [TestCase(1, 0)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(4, 2)]
        [TestCase(5, 2)]
        public void Interaction_AddEventHistoryBasedOnCallCount(int countCall, int countEventHistoreAdd)
        {
            // Prepare
            var baseCharacter = _baseCharacterMock.Object;

            // Act
            for (int i = 0; i < countCall; i++)
            {
                _exitSecretRoom.Interaction(baseCharacter);
            }

            // Assert
            _mazeMock.Verify(x => x.EventHistory.Add("You left the secret room."),
                Times.Exactly(countEventHistoreAdd),
                $"If Interaction was called {countCall} times, EvetnHisory mus be Add {countEventHistoreAdd} times");
        }
    }
}

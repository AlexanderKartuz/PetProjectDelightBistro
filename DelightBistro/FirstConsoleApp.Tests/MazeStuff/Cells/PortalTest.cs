using MazeCore.Cells;
using MazeCore.Cells.Interfaces;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;
using Moq;
using NUnit.Framework;

namespace FirstConsoleApp.Tests.MazeStuff.Cells;

public class PortalTest
{
    private Portal _portal;
    private Portal _linkedPortal;
    private Mock<IBaseCharacter> _baseCharacterMock;
    private Mock<IMaze> _mazeMock;
    private Mock<IInputReader> _inputReaderMock;

    [SetUp]
    public void Setup()
    {
        _mazeMock = new Mock<IMaze>();
        _mazeMock
            .Setup(x => x.EventHistory.Add(It.IsAny<string>()));
        _mazeMock
            .Setup(x => x.Surface.Add(It.IsAny<IBaseCell>()));
        _mazeMock
            .Setup(x => x.Surface.Remove(It.IsAny<IBaseCell>()));
        var maze = _mazeMock.Object;

        _baseCharacterMock = new Mock<IBaseCharacter>();
        _baseCharacterMock
            .SetupProperty(x => x.X);
        _baseCharacterMock
            .SetupProperty(y => y.Y);
        
        
        /*Как проверять Readkey?
         * я столкнулась с проблемой, что в Portal.Interaction был вызов Console.ReadKey который нельзя замокать.
         *Я создала отдельный интерфейс IInputReader и класс ConsoleInputReader
         *В классе Portal добавила поле _inputReader и передачу через конструктор — теперь
         *портал не создаёт ввод сам, а получает его снаружи.
         *В тесте передаём мок, в игре передаём ConsoleInputReader
         */
        _inputReaderMock  = new Mock<IInputReader>();
        var inputReader = _inputReaderMock.Object;
        _inputReaderMock
            .Setup(x => x.ReadKey()).Returns(ConsoleKey.Y);

        _portal = new Portal(maze, inputReader);
        _linkedPortal = new Portal(maze, inputReader);
        _portal.LinkedPortal = _linkedPortal;
    }
    
    [Test]
    [TestCase(0, 1, 5, 5)]
    public void Interaction_CharacterWasTeleported(int xOld, int yOld, int xNew, int yNew)
    {
        //Prepare
        _portal.X = xOld;
        _portal.Y = yOld;
        _linkedPortal.X = xNew;
        _linkedPortal.Y = yNew;
        
        //Act
        var baseCharacter = _baseCharacterMock.Object;
        _portal.Interaction(baseCharacter);
        
        //Assert
        Assert.That(baseCharacter.X, Is.EqualTo(xNew));
        Assert.That(baseCharacter.Y, Is.EqualTo(yNew));
        
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void Interaction_AddToEventHistory(bool usePortal)
    {
        //Prepare
        if (!usePortal)
        {
            _inputReaderMock
                .Setup(x => x.ReadKey()).Returns(ConsoleKey.N);
        }
        //Act
        _portal.Interaction(_baseCharacterMock.Object);
        
        //Assert
        _mazeMock.Verify(x => x.EventHistory.Add(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void Interaction_CanTeleport()
    {
        //Prepare
        
        //Act
        var result = _portal.Interaction(_baseCharacterMock.Object);
        
        //Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Interaction_CanIgnoreTeleport()
    {
        //Prepare
        _inputReaderMock
            .Setup(x => x.ReadKey()).Returns(ConsoleKey.N);
        
        //Act
        var result = _portal.Interaction(_baseCharacterMock.Object);
        
        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void Iteraction_ReplacedByGroundIfSingleUse(bool isSingleUse)
    {
        //Prepare
        _portal.IsSingleUse = isSingleUse;
        
        //Act
        _portal.Interaction(_baseCharacterMock.Object);
        
        //Assert
        if (isSingleUse)
        {
            _mazeMock.Verify(x => x.Surface.Remove(It.IsAny<Portal>()), Times.Exactly(2));
            _mazeMock.Verify(x => x.Surface.Add(It.IsAny<Ground>()), Times.Exactly(2));
        }
        else
        {
            _mazeMock.Verify(x => x.Surface.Remove(It.IsAny<Portal>()), Times.Never());
            _mazeMock.Verify(x => x.Surface.Add(It.IsAny<Ground>()), Times.Never());
        }
    }
}
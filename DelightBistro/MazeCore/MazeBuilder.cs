using MazeCore.Cells;
using MazeCore.Cells.ConsoleInputReader;
using MazeCore.Cells.Shopkeeper;
using MazeCore.Characters;
using MazeCore.Extensions;
using MazeCore.Interfaces;
using MazeCore.Cells.Interfaces;

namespace MazeCore
{
    public class MazeBuilder : IMazeBuilder
    {
        private const int MIN_PORTAL_PAIRS = 2;
        private const int MAX_PORTAL_PAIRS = 5;
        private const double SINGLE_USE_PORTAL_CHANCE = 0.3;
        private const int _MAX_DOORS_COUNT = 5;
        private const int MAX_ICE = 15;
        private const int MAX_FIRE = 5;

        private IMaze _maze;
        private Random _random;

        public IMaze Build(int width, int height, int? seed = null, bool isSecretMaze = false, Hero inputHero = null)
        {
            _maze = new Maze
            {
                Width = width,
                Height = height,
                Seed = seed ?? DateTime.Now.Millisecond,
                InputReader = new ConsoleInputReader()
            };

            _random = new Random(_maze.Seed);


            if (inputHero == null)
            {
                var hero = GenerateHero();
                _maze.Hero = hero;
            }
            else
            {
                _maze.Hero = inputHero;
            }

            if (isSecretMaze)
            {
                GenerateSecretMaze();

                return _maze;
            }

            GenerateWall();
            GenerateGround(_maze.Hero.X, _maze.Hero.Y);// Genrate path
            GenerateCoins();
            GenerateSuperPower();
            GenerateTrap();
            GeneratePortals();
            GenerateRest();
            GenerateDoors();
            GenerateMimics();
            GenerateLava();
            // Generate other cells
            GenerateIce(10);
            GenerateSpeedPotions();
            GenerateSkipingMove();
            GenerateFire();
            GenerateGhost(4);
            GenerateShopkeeper();
            GenerateSecretRoom();
            return _maze;
        }

        private Hero GenerateHero()
        {
            return new Hero(_maze)
            {
                X = 0,
                Y = 0,
            };
        }

        private void GenerateDoors()
        {
            var doorAvailableCells = _maze.Surface
                .Where(cell => cell is Ground)
                .Where(cell => IsSuitableDoorPosition(cell.X, cell.Y))
                .ToList();
            var maxAvailableDoorsCount = doorAvailableCells.Count;
            if (maxAvailableDoorsCount <= 0)
            {
                return;
            }
            if (maxAvailableDoorsCount > _MAX_DOORS_COUNT)
            {
                maxAvailableDoorsCount = _MAX_DOORS_COUNT;
            }
            var selectedCells = SelectRandomDoorPositions(doorAvailableCells, maxAvailableDoorsCount);

            GenerateKeysForDoors(selectedCells.Count);

            for (int i = 0; i < selectedCells.Count; i++)
            {
                var cell = selectedCells[i];
                var door = new Doors(_maze)
                {
                    X = cell.X,
                    Y = cell.Y,
                };

                ReplaceCell(door);
            }

        }


        private bool IsSuitableDoorPosition(int x, int y)
        {
            var isHorizontalPassage = IsWallOrBoundary(x - 1, y) && IsWallOrBoundary(x + 1, y);

            var isVerticalPassage = IsWallOrBoundary(x, y - 1) && IsWallOrBoundary(x, y + 1);

            return isHorizontalPassage || isVerticalPassage;
        }
        private bool IsWallOrBoundary(int x, int y)
        {
            if (x < 0 || x >= _maze.Width || y < 0 || y >= _maze.Height)
            {
                return true;
            }

            return _maze[x, y] is Wall;
        }

        private List<IBaseCell> SelectRandomDoorPositions(List<IBaseCell> doorAvailablePositions, int maxDoorsCount)
        {
            var shuffledDoors = doorAvailablePositions
                .OrderBy(_ => _random.Next())
                .Take(maxDoorsCount)
                .ToList();
            return shuffledDoors;
        }

        private void GenerateKeysForDoors(int doorCount)
        {
            var availablePositions = _maze.Surface
                .Where(cell => cell is Ground)
                .OrderBy(_ => _random.Next())
                .Take(doorCount)
                .ToList();

            for (int i = 0; i < availablePositions.Count; i++)
            {
                var position = availablePositions[i];
                var key = new Key(_maze)
                {
                    X = position.X,
                    Y = position.Y,
                };
                ReplaceCell(key);
            }

        }

        private void GenerateMimics()
        {
            GenerateMimic(Coin.SYMBOL, 4);
            GenerateMimic(Doors.SYMBOL, 2);
        }

        private void GenerateMimic(char symbol, int maxMimicCount = 2)
        {
            var freeCells = _maze
                .Surface
                .Where(cell => cell is Ground)
                .Where(x => symbol == Coin.SYMBOL && GetNearCells<Ground>(x).Count() == 1 ||
                    symbol == Doors.SYMBOL && GetNearCells<Ground>(x).Count() > 1)
                .ToList();
            TryReplaceMimic(maxMimicCount, freeCells, symbol);
        }

        private void TryReplaceMimic(int maxMimicCount, List<IBaseCell> cells, char symbol)
        {
            if (!cells.Any())
            {
                return;
            }

            for (int i = 0; i < maxMimicCount; i++)
            {
                var randomIndex = _random.Next(cells.Count());
                var randomCell = cells[randomIndex];
                var mimic = new Mimic(_maze, symbol)
                {
                    X = randomCell.X,
                    Y = randomCell.Y,
                };
                ReplaceCell(mimic);
            }
        }

        private void GenerateCoins(int maxCoinCount = 4)
        {
            var deadends = GetDeadends();

            for (int i = 0; i < maxCoinCount; i++)
            {
                var deadend = deadends[i];
                var coin = new Coin(_maze)
                {
                    X = deadend.X,
                    Y = deadend.Y,
                };
                ReplaceCell(coin);
            }
        }

        private void GenerateSuperPower(int maxSuperPowerCount = 2)
        {
            /*
             размещение клеток супер силы:
             взяла все клетки, расстояние до которых 50% и выше от максимального расстояния
             нашла в них тупики
             выбрала рандомные клетки
             если тупиков нет, то выбрала рандомно две клетки среди отдаленных от начала лабиринта на 50%
             */


            var queue = new Queue<IBaseCell>();
            var dictionaryCellsAndDistance = new Dictionary<IBaseCell, int>();

            queue.Enqueue(_maze[0, 0]);
            dictionaryCellsAndDistance.Add(_maze[0, 0], 0);

            while (queue.Any())
            {
                var cell = queue.Dequeue();
                var groundNearCurrentCell = GetNearCells<Ground>(cell);

                foreach (var nearCell in groundNearCurrentCell)
                {
                    if (!dictionaryCellsAndDistance.ContainsKey(nearCell))
                    {
                        queue.Enqueue(nearCell);
                        var distance = dictionaryCellsAndDistance[cell];
                        dictionaryCellsAndDistance.Add(nearCell, distance + 1);
                    }
                }
            }

            var maxDistance = dictionaryCellsAndDistance
                .Values
                .Max();

            var minDistanceToGenerate = maxDistance * 0.5;

            var suitableCellsToGenerate = dictionaryCellsAndDistance
                .Keys
                .Where(cell => dictionaryCellsAndDistance[cell] >= minDistanceToGenerate)
                .ToList();

            var suitableDeadends = suitableCellsToGenerate
                .Where(cell => GetNearCells<Ground>(cell).Count() == 1)
                .ToList();

            IBaseCell chosenCell;


            for (int i = 0; i < maxSuperPowerCount; i++)
            {
                if (suitableDeadends.Any())
                {
                    chosenCell = GetRandomCell(suitableDeadends);
                }
                else
                {
                    chosenCell = GetRandomCell(suitableCellsToGenerate);
                }

                var superPower = new SuperPower(_maze)
                {
                    X = chosenCell.X,
                    Y = chosenCell.Y,
                };

                ReplaceCell(superPower);

                suitableCellsToGenerate.Remove(chosenCell);
                suitableDeadends.Remove(chosenCell);
            }
        }

        private void GenerateGround(int startX = 0, int startY = 0)
        {
            var miner = _maze[startX, startY];

            var wallsToDestroy = new List<IBaseCell>();

            do
            {
                ReplaceCellToGround(miner); // Destoy wall 

                var nears = GetNearCells<Wall>(miner);

                wallsToDestroy.AddRange(nears);

                wallsToDestroy = wallsToDestroy
                    // .Where(cell => AllowToDestroy(cell))
                    .Where(AllowToDestroy)
                    .ToList();
                if (wallsToDestroy.Any())
                {
                    miner = GetRandomCell(wallsToDestroy);
                }
            } while (wallsToDestroy.Any());
        }

        private IBaseCell GetRandomCell(List<IBaseCell> wallsToDestroy)
        {
            var randomIndex = _random.Next(wallsToDestroy.Count);
            return wallsToDestroy[randomIndex];
        }

        private bool AllowToDestroy(IBaseCell cell)
        {
            return _maze[cell.X, cell.Y] is Wall
                 && GetNearCells<Ground>(cell)
                .Count() < 2;
        }

        private IEnumerable<IBaseCell> GetNearCells<TypeOfOurCell>(IBaseCell miner)
            where TypeOfOurCell : IBaseCell
        {
            return _maze.Surface
                .Where(cell => cell is TypeOfOurCell)
                .Where(cell =>
                    cell.Y == miner.Y && Math.Abs(cell.X - miner.X) == 1
                    || cell.X == miner.X && Math.Abs(cell.Y - miner.Y) == 1);
        }

        private void GenerateSkipingMove(int maxPitCount = 5)
        {
            var grounds = _maze
            .Surface
            .Where(x => x is Ground)
            .ToList();

            for (int i = 0; i < maxPitCount; i++)
            {
                var freeCell = grounds[_random.Next(grounds.Count)];

                var pit = new SkipingMove(_maze)
                {
                    X = freeCell.X,
                    Y = freeCell.Y,
                };

                ReplaceCell(pit);
            }
        }

        private void GenerateWall()
        {
            for (int y = 0; y < _maze.Height; y++)
            {
                for (int x = 0; x < _maze.Width; x++)
                {
                    var wall = new Wall(_maze)
                    {
                        X = x,
                        Y = y
                    };
                    _maze.Surface.Add(wall);
                }
            }
        }

        private void GenerateTrap()
        {
            var nearcoins = _maze
                .Surface
                .Where(x => x is Ground)
                .Where(x => GetNearCells<Coin>(x).Count() == 1)
                .ToList();

            foreach (var nearCoin in nearcoins)
            {
                var trap = new Trap(_maze)
                {
                    X = nearCoin.X,
                    Y = nearCoin.Y,
                };
                ReplaceCell(trap);
            }
        }


        private void GeneratePortals()
        {
            var deadends = GetDeadends();
            var intersections = GetIntersections();
            var corners = GetAvailableCorners();

            var potentialCellsForPortals = new List<IBaseCell>();
            potentialCellsForPortals.AddRange(deadends);
            potentialCellsForPortals.AddRange(intersections);
            potentialCellsForPortals.AddRange(corners);

            potentialCellsForPortals = potentialCellsForPortals
                .Distinct()
                .ToList();

            var requestedPortalPairsCount = _random.Next(MIN_PORTAL_PAIRS, MAX_PORTAL_PAIRS + 1);
            var maxPortalPairsCount = potentialCellsForPortals.Count / 2;
            var pairsCount = Math.Min(requestedPortalPairsCount, maxPortalPairsCount);

            if (pairsCount == 0)
            {
                return;
            }

            var totalPortals = pairsCount * 2;

            potentialCellsForPortals.Shuffle(_random);
            var selectedCells = potentialCellsForPortals
                .Take(totalPortals)
                .ToList();

            var portals = new List<Portal>();

            foreach (var cell in selectedCells)
            {
                var portal = new Portal(_maze, new ConsoleInputReader())
                {
                    X = cell.X,
                    Y = cell.Y,
                    IsSingleUse = _random.NextDouble() < SINGLE_USE_PORTAL_CHANCE
                };

                ReplaceCell(portal);
                portals.Add(portal);
            }

            LinkPortals(portals);
        }

        private void LinkPortals(List<Portal> portals)
        {
            var remainingPortals = new List<Portal>(portals);

            while (remainingPortals.Count >= 2)
            {
                var currentPortal = remainingPortals[0];
                remainingPortals.RemoveAt(0);

                Portal farthestPortal = null;
                var maxDistance = 0;

                foreach (var portal in remainingPortals)
                {
                    var distance = GetManhattanDistance(currentPortal, portal);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        farthestPortal = portal;
                    }
                }

                remainingPortals.Remove(farthestPortal);

                currentPortal.LinkedPortal = farthestPortal;
                farthestPortal.LinkedPortal = currentPortal;
            }
        }

        private int GetManhattanDistance(IBaseCell a, IBaseCell b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private List<IBaseCell> GetIntersections()
        {
            var intersections = _maze
                 .Surface
                 .Where(c => c is Ground)
                 .Where(c => GetNearCells<Ground>(c).Count() >= 3)
                 .ToList();

            return intersections;
        }

        private List<IBaseCell> GetAvailableCorners()
        {
            var corners = _maze.Surface
               .Where(cell => cell is Ground)
               .Where(cell =>
                   cell.X == _maze.Width - 1 && cell.Y == 0
                   || cell.X == 0 && cell.Y == _maze.Height - 1
                   || cell.X == _maze.Width - 1 && cell.Y == _maze.Height - 1)
               .ToList();

            return corners;
        }

        private List<IBaseCell> GetDeadends()
        {
            var deadends = _maze
                .Surface
                .Where(c => c is Ground)
                .Where(c => GetNearCells<Ground>(c).Count() == 1)
                .ToList();

            return deadends;
        }

        private void GenerateRest(int maxRestCount = 5)
        {
            var deadends = _maze
              .Surface
              .Where(x => x is Ground)
              .Where(x => GetNearCells<Ground>(x).Count() >= 3)
              .ToList();
            for (int i = 0; i < maxRestCount; i++)
            {
                var deadend = deadends[i];
                var rest = new Rest(_maze)
                {
                    X = deadend.X,
                    Y = deadend.Y,
                };
                ReplaceCell(rest);
            }
        }

        private void ReplaceCell(IBaseCell newCell) // coin [1,1]
        {
            var oldCell = _maze
                .Surface
                .First(c => c.X == newCell.X && c.Y == newCell.Y);
            _maze.Surface.Remove(oldCell);

            _maze.Surface.Add(newCell); // replace
        }

        private void ReplaceCellToGround(IBaseCell oldCell)
        {
            _maze.Surface.Remove(oldCell);

            var ground = new Ground(_maze)
            {
                X = oldCell.X,
                Y = oldCell.Y,
            };

            _maze.Surface.Add(ground);
        }

        private void ReplaceCellToIce(IBaseCell oldCell)
        {
            _maze.Surface.Remove(oldCell);

            var ice = new Ice(_maze)
            {
                X = oldCell.X,
                Y = oldCell.Y,
            };

            _maze.Surface.Add(ice);
        }

        private void GenerateIce(int countIce = 5)
        {
            countIce = Math.Min(MAX_ICE, countIce);

            var friendlyCells = _maze.Surface.Where(cell => cell.IsBonusCell).ToList();

            var nearCellsFromList = GetNearCellsFromList(friendlyCells);
            var uniqueCellsFromList = GetUniqueCellsFromList(nearCellsFromList.ToList());

            var maxCountIce = Math.Min(uniqueCellsFromList.Count, countIce);
            var randomCount = _random.Next(1, maxCountIce);

            for (int i = 0; i < randomCount; i++)
            {
                var oldCell = GetRandomCell(uniqueCellsFromList);
                ReplaceCellToIce(oldCell);
            }
        }

        /// <summary>
        /// Get near cells from cells in input List 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBaseCell> GetNearCellsFromList(List<IBaseCell> inputCellList)
        {
            foreach (var cell in inputCellList)
            {
                var nearOneCell = GetNearCells<IBaseCell>(cell);
                foreach (var oneCell in nearOneCell)
                {
                    yield return oneCell;
                }
            }
        }

        /// <summary>
        /// Get list with unique cells from List
        /// </summary>
        /// <returns></returns>
        public List<IBaseCell> GetUniqueCellsFromList(List<IBaseCell> inputCellList)
        {
            return inputCellList.Distinct().ToList();
        }

        public void GenerateIceNearHero()
        {
            var cellsNearHero = GetNearCells<IBaseCell>(_maze.Hero).ToList();
            var availableCells = cellsNearHero.Where(cell => cell is Ground).ToList();

            if (!availableCells.Any())
            {
                return;
            }

            var randomIceNearHero = GetRandomCell(availableCells);

            if (randomIceNearHero != null)
            {
                ReplaceCellToIce(randomIceNearHero);
            }
        }
        private void GenerateLava(int maxLavaCount = 2)
        {
            var walls = _maze.Surface
                .OfType<Wall>()
                .ToList();

            var lavaLake = walls
                .Where(cell =>
                    walls.Any(x => x.X == cell.X + 1 && x.Y == cell.Y) &&
                    walls.Any(x => x.X == cell.X && x.Y == cell.Y + 1) &&
                    walls.Any(x => x.X == cell.X + 1 && x.Y == cell.Y + 1))
                .ToList();

            if (lavaLake.Count > 0)
            {
                for (int i = 0; i < maxLavaCount && i < lavaLake.Count; i++)
                {
                    var corner = lavaLake[i];

                    ReplaceCell(new Lava(_maze) { X = corner.X, Y = corner.Y });
                    ReplaceCell(new Lava(_maze) { X = corner.X + 1, Y = corner.Y });
                    ReplaceCell(new Lava(_maze) { X = corner.X, Y = corner.Y + 1 });
                    ReplaceCell(new Lava(_maze) { X = corner.X + 1, Y = corner.Y + 1 });
                }

                return;
            }

            var surroundedWalls = walls
                .Where(x => GetNearCells<Ground>(x).Count() == 4)
                .ToList();

            for (int i = 0; i < maxLavaCount && i < surroundedWalls.Count; i++)
            {
                var wall = surroundedWalls[i];

                var lava = new Lava(_maze)
                {
                    X = wall.X,
                    Y = wall.Y,
                };

                ReplaceCell(lava);
            }
        }
        private void GenerateSpeedPotions(int maxSpeedPotionsCount = 3)
        {
            var deadendsWallsAround = _maze
                .Surface
                .Where(x => x is Ground)
                .Where(x => GetNearCells<Wall>(x).Count() == 3)
                .ToList();

            for (int i = 0; i < maxSpeedPotionsCount; i++)
            {
                var deadendWallsAround = deadendsWallsAround[i];
                var speedPotion = new SpeedPotions(_maze)
                {
                    X = deadendWallsAround.X,
                    Y = deadendWallsAround.Y,
                };
                ReplaceCell(speedPotion);
            }
        }

        private void GenerateFire()
        {

            for (int i = 0; i < MAX_FIRE; i++)
            {
                var x = _random.Next(0, _maze.Width);
                var y = _random.Next(0, _maze.Height);

                var fire = new Fire(_maze)
                {
                    X = x,
                    Y = y,
                };

                ReplaceCell(fire);
            }
        }

        private void GenerateShopkeeper()
        {
            var doorsMaze = _maze
                .Surface
                .Where(cell => cell is Doors)
                .ToList();

            var firstGroundNearDoors = GetNearCellsFromList(doorsMaze)
                .FirstOrDefault(cell => cell is Ground);

            var x = new int();
            var y = new int();
            if (firstGroundNearDoors != null)
            {
                x = firstGroundNearDoors.X;
                y = firstGroundNearDoors.Y;
            }
            else
            {
                var startHeroAreaX = 5;
                var startHeroAreaY = 5;
                var groundsMaze = _maze.Surface
                    .Where(cell => cell is Ground)
                    .Where(cell => cell.X >= startHeroAreaX && cell.Y >= startHeroAreaY).ToList();
                var indexRandomGround = _random.Next(0, groundsMaze.Count() - 1);
                x = groundsMaze[indexRandomGround].X;
                y = groundsMaze[indexRandomGround].Y;
            }

            var shopkeeper = new Shopkeeper(_maze, _random)
            {
                X = x,
                Y = y
            };
            ReplaceCell(shopkeeper);
        }
        private void GenerateGhost(int countOfGhost = 2)
        {
            var cellToGenerateGhost = _maze
                .Surface
                .Where(x => x is Ground)
                .Where(x => GetNearCells<Wall>(x).Count() == 2)
                .ToList();

            if (countOfGhost > cellToGenerateGhost.Count)
            {
                countOfGhost = 2;
            }
            for (int i = 0; i < countOfGhost; i++)
            {

                var cellToReplace = cellToGenerateGhost[i];
                var ghost = new Ghost(_maze)
                {
                    X = cellToReplace.X,
                    Y = cellToReplace.Y,
                };
                ReplaceCell(ghost);
            }
        }
        private void GenerateSecretMaze()
        {
            for (int y = 0; y < _maze.Height; y++)
            {
                for (int x = 0; x < _maze.Width; x++)
                {
                    IBaseCell cell;

                    if (x == 0 && y == 1)
                    {
                        cell = new ExitSecretRoom(_maze)
                        {
                            X = x,
                            Y = y
                        };
                    }
                    else if (x == 0 || x == _maze.Width - 1 || y == 0 || y == _maze.Height - 1)
                    {
                        cell = new Wall(_maze)
                        {
                            X = x,
                            Y = y
                        };
                    }
                    else
                    {
                        cell = new Coin(_maze)
                        {
                            X = x,
                            Y = y
                        };
                    }
                    _maze.Surface.Add(cell);
                }
            }

            var exitSecretRoomCell = _maze.Surface.OfType<ExitSecretRoom>().FirstOrDefault();
            _maze.Hero.X = exitSecretRoomCell.X;
            _maze.Hero.Y = exitSecretRoomCell.Y;
        }

        private void GenerateSecretRoom()
        {
            var secretRoom = new SecretRoom(_maze)
            {
                X = 2,
                Y = 2
            };
            ReplaceCell(secretRoom);
        }

    }
}


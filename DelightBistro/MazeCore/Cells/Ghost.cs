using MazeCore.Cells;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;
internal class Ghost:BaseCell
{
    const int CHACNE_TO_KILL_GHOST = 5;
    private int BagWithCoins = 3;
    public Ghost(IMaze maze) :base(maze) 
    {
    }
    public override char Symbol => 'G';

    public override bool Interaction(IBaseCharacter character)
    {
        var random = new Random();
        var ghostDieOrNot = random.Next(0, CHACNE_TO_KILL_GHOST) == CHACNE_TO_KILL_GHOST - 1;//The chance to kill the ghost is 20%. 
        var ground = new Ground(Maze)
        {
            X = X,
            Y = Y,
        };
        Maze.Surface.Add(ground);
        if (ghostDieOrNot)//If we kill a ghost, we take his money and the ghost disappears.
        {
            character.Coins += BagWithCoins;
            character.EnemiesKilled++;
            Maze.Surface.Remove(this);
            Maze.EventHistory.Add($"The Ghost is dead. Your reward - {BagWithCoins} coins");
            return true;
        }
        if (character.Hp <= 0 && character.Coins < 0) //If we have 1 health or less left and no money, Ghost disappears from the labyrinth.
        {
            Maze.EventHistory.Add("Ghost: You are poor and weak. I have nothing more to do in this labyrinth, goodbye.");
            Maze.Surface.Remove(this);
            return true;
        }
        else//If we miss and have HP or coins
        {
            if(character.Coins > 0) //Ghost takes our coin and runs away.
            {
                character.Coins--;
                BagWithCoins++;
                Maze.EventHistory.Add("Ghost: Heheheh. Your coin - my coin. hehehe");
            }
            else //If we poor
            {

                character.Hp--;
                Maze.EventHistory.Add("Ghost: Are you poor? I take your soul.");

            }
            var runAwauFromHero = Maze.Surface.Where(cell => cell is Ground)
                                               .ToList();
            var groundForReplace = runAwauFromHero[random.Next(0, runAwauFromHero.Count)];
            X = groundForReplace.X;
            Y = groundForReplace.Y;
            Maze.Surface.Remove(groundForReplace);
        }
        return true;
    }
}
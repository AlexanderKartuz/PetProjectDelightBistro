using MazeCore.Cells;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

public class Key : BaseCell
{
    public Key(IMaze maze) : base(maze) { }

    public override char Symbol => 'K';

    private const int _KEY_COLLECT = 1;

    public bool Collect(IBaseCharacter character)
    {
        character.CollectKey(_KEY_COLLECT);
        return true;
    }
    public override bool Interaction(IBaseCharacter character)
    {
        MazeSoundPlayer soundPlayer = new MazeSoundPlayer();
        soundPlayer.PlayMusic("key_sound.wav", 0.6f);
        Maze.EventHistory.Add("Look, it's a key");
        Collect(character);
        ReplaceKeyToGround();
        return true;
    }
    private void ReplaceKeyToGround()
    {
        Maze.Surface.Remove(this);
        var ground = new Ground(Maze)
        {
            X = X,
            Y = Y,
        };
        Maze.Surface.Add(ground);
    }
}
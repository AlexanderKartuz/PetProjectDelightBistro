using MazeCore.Cells.Interfaces;
using MazeCore.Characters;

namespace MazeCore.Interfaces
{
    public interface IMazeBuilder
    {
        IMaze Build(int width, int height, int? seed = null, bool isSecretMaze = false, Hero inputHero = null);
        void GenerateIceNearHero();
        IEnumerable<IBaseCell> GetNearCellsFromList(List<IBaseCell> inputCellList);
        List<IBaseCell> GetUniqueCellsFromList(List<IBaseCell> inputCellList);
    }
}
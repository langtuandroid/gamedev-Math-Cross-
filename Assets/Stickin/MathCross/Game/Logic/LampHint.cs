
using stickin;

namespace stickin.mathcross
{
    public class LampHint : Hint
    {
        private Board _board;
        private Pocket _pocket;
        
        public LampHint(Board board, Pocket pocket)
        {
            _board = board;
            _pocket = pocket;
        }
        
        public override bool Run(Game g)
        {
            if (_pocket.Cells.Count > 0)
            {
                var cell = _pocket.Cells.GetRandom();
                if (_board.TryAddedCell(cell, cell.CorrectIndex))
                {
                    cell.IsLocked = true;
                    return true;
                }
            }
            else if (_board.Cells.Count > 0)
            {
                foreach (var cell in _board.Cells)
                {
                    if (!cell.IsCorrect && cell.CorrectIndex != cell.CurrentIndex)
                    {
                        if (_board.TryAddedCell(cell, cell.CorrectIndex))
                        {
                            cell.IsLocked = true;
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }
    }
}
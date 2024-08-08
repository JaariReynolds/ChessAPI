using Chess.Classes;

namespace ChessAPI.Models
{
    public class GameboardAndActionsDto
    {
        public Gameboard Gameboard { get; set; }
        public List<PieceActionDto> Actions { get; set; }
    }
}

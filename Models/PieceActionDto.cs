using Chess.Classes;

namespace ChessAPI.Models
{
    public class PieceActionDto
    {
        public Piece Piece { get; set; }
        public List<Action> Actions { get; set; } = new List<Action>();
    }
}

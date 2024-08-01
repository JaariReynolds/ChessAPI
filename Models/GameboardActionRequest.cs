using Chess.Classes;

namespace ChessAPI.Models
{
    public class GameboardActionRequest
    {
        public Gameboard Gameboard { get; set; }
        public Action Action { get; set; }
    }
}

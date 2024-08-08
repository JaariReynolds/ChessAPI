using Chess.Classes;

namespace ChessAPI.Models
{
    public class PerformGameboardActionRequest
    {
        public Gameboard Gameboard { get; set; }
        public Action Action { get; set; }
    }
}

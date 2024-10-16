using Chess.Classes;
using System.ComponentModel.DataAnnotations;

namespace ChessAPI.Models
{
    public class PerformGameboardActionRequest
    {
        [Required(ErrorMessage = "Gameboard is required.")]
        public Gameboard Gameboard { get; set; }

        [Required(ErrorMessage = "Action is required.")]
        public Action Action { get; set; }
    }
}

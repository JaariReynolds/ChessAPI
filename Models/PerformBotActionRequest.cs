using Chess.Classes;
using System.ComponentModel.DataAnnotations;

namespace ChessAPI.Models
{
    public class PerformBotActionRequest
    {
        [Required(ErrorMessage = "Gameboard is required.")]
        public Gameboard Gameboard { get; set; }
    }
}

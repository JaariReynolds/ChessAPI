using System.ComponentModel.DataAnnotations;

namespace ChessAPI.Models
{
    public class ForsythEdwardsNotationRequest
    {
        [Required(ErrorMessage = "Forsyth-Edwards Notation (FEN) string is required.")]
        public string FenString { get; set; }
    }
}

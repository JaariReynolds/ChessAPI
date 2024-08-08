using Chess.Classes;
using ChessAPI.Models;

namespace ChessAPI.Services
{
    public class ChessService
    {
        public List<PieceActionDto> DictionaryToPieceActionDto(Dictionary<Piece, List<Action>> dictionary)
        {
            var dtoList = new List<PieceActionDto>();

            if (dictionary.Count > 0)
                dtoList = dictionary.Select(kvp => new PieceActionDto
                {
                    Piece = kvp.Key,
                    Actions = kvp.Value
                }).ToList();

            return dtoList;
        }
    }
}

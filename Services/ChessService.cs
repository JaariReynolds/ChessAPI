using Chess.Classes;
using ChessAPI.Models;

namespace ChessAPI.Services
{
    public interface IChessService
    {
        List<PieceActionDto> DictionaryToPieceActionDto(Dictionary<Piece, List<Action>> dictionary);
    }

    public class ChessService : IChessService
    {
        /// <summary>
        /// Conversion from Dictionary to List, as Dictionaries without (string) Keys in JSON are not supported 
        /// </summary>
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

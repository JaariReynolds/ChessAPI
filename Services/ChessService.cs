using Chess.Classes;
using ChessAPI.Models;

namespace ChessAPI.Services
{
    public interface IChessService
    {
        List<PieceActionDto> DictionaryToPieceActionListDto(Dictionary<Piece, List<Action>> dictionary);
        GameboardAndActionsDto GetInitialBoard();
        GameboardAndActionsDto PerformAction(Gameboard gameboard, Action requestedAction);
        GameboardAndActionsDto PerformBotAction(Gameboard gameboard);
    }

    public class ChessService : IChessService
    {
        /// <summary>
        /// Conversion from Dictionary to List, as Dictionaries without (string) Keys in JSON are not supported 
        /// </summary>
        public List<PieceActionDto> DictionaryToPieceActionListDto(Dictionary<Piece, List<Action>> dictionary)
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

        private List<PieceActionDto> GetActionsDto(Gameboard gameboard)
        {
            var dictionaryActions = gameboard.CalculateTeamActions(gameboard.CurrentTeamColour);
            return DictionaryToPieceActionListDto(dictionaryActions);
        }

        public GameboardAndActionsDto GetInitialBoard()
        {
            var gameboard = new Gameboard();
            gameboard.InitialiseStandardBoardState();

            var actionsDto = GetActionsDto(gameboard);

            return new GameboardAndActionsDto { Gameboard = gameboard, Actions = actionsDto };
        }

        public GameboardAndActionsDto PerformAction(Gameboard gameboard, Action requestedAction)
        {
            gameboard.ProcessTurn(requestedAction);

            var actionsDto = GetActionsDto(gameboard);

            return new GameboardAndActionsDto { Gameboard = gameboard, Actions = actionsDto };
        }

        public GameboardAndActionsDto PerformBotAction(Gameboard gameboard)
        {
            var chessBot = new ChessBot.ChessBot(ChessBot.BotDifficulty.Easy, gameboard);
            var chessBotAction = chessBot.CalculateBestAction(3);
            gameboard.ProcessTurn(chessBotAction);

            var actionsDto = GetActionsDto(gameboard);

            return new GameboardAndActionsDto { Gameboard = gameboard, Actions = actionsDto };
        }
    }
}

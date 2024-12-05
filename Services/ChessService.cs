using Chess.Classes;
using ChessAPI.Models;
using ChessBotNamespace;

namespace ChessAPI.Services
{
    public interface IChessService
    {
        List<PieceActionDto> ListToPieceActionDto(List<Action> dictionary);
        GameboardAndActionsDto GetInitialBoard();
        GameboardAndActionsDto PerformAction(Gameboard gameboard, Action requestedAction);
        GameboardAndActionsDto PerformBotAction(Gameboard gameboard);
    }

    public class ChessService : IChessService
    {
        /// <summary>
        /// Conversion from Dictionary to List, as Dictionaries without (string) Keys in JSON are not supported 
        /// </summary>
        public List<PieceActionDto> ListToPieceActionDto(List<Action> actions)
        {
            var groupedActions = actions
                .GroupBy(action => action.Piece)
                .Select(group => new PieceActionDto
                {
                    Piece = group.Key,
                    Actions = group.ToList()
                }).ToList();

            return groupedActions;
        }

        private List<PieceActionDto> GetActionsDto(Gameboard gameboard)
        {
            var actions = gameboard.CalculateTeamActions(gameboard.CurrentTeamColour);
            return ListToPieceActionDto(actions);
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
            var chessBot = new ChessBot(gameboard);
            var chessBotAction = chessBot.CalculateBestAction(3);
            gameboard.ProcessTurn(chessBotAction);

            var actionsDto = GetActionsDto(gameboard);

            return new GameboardAndActionsDto { Gameboard = gameboard, Actions = actionsDto };
        }
    }
}

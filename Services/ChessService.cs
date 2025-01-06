using Chess.Classes;
using ChessAPI.Models;
using ChessBotNamespace;
using ChessLogic.Classes;
using System.Diagnostics;

namespace ChessAPI.Services
{
    public interface IChessService
    {
        List<PieceActionDto> ListToPieceActionDto(List<Action> dictionary);
        ApiResponse<GameboardAndActionsDto> GetInitialBoard();
        ApiResponse<GameboardAndActionsDto> PerformAction(Gameboard gameboard, Action requestedAction);
        ApiResponse<GameboardAndActionsDto> PerformBotAction(Gameboard gameboard);
        ApiResponse<GameboardAndActionsDto> ParseFen(string fen);
        ApiResponse<string> GenerateFen(Gameboard gameboard);
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

        public ApiResponse<GameboardAndActionsDto> GetInitialBoard()
        {
            try
            {
                var gameboard = new Gameboard();
                gameboard.InitialiseStandardBoardState();

                var actionsDto = GetActionsDto(gameboard);
                var successObject = new GameboardAndActionsDto { Gameboard = gameboard, Actions = actionsDto };
                return ApiResponse<GameboardAndActionsDto>.CreateSuccessResponse(successObject);
            }
            catch (Exception e)
            {
                return ApiResponse<GameboardAndActionsDto>.CreateErrorResponse(e.Message);
            }
        }

        public ApiResponse<GameboardAndActionsDto> PerformAction(Gameboard gameboard, Action requestedAction)
        {
            try
            {
                gameboard.ProcessTurn(requestedAction);
                var actionsDto = GetActionsDto(gameboard);
                var successObject = new GameboardAndActionsDto { Gameboard = gameboard, Actions = actionsDto };
                return ApiResponse<GameboardAndActionsDto>.CreateSuccessResponse(successObject);
            }
            catch (Exception e)
            {
                return ApiResponse<GameboardAndActionsDto>.CreateErrorResponse(e.Message);
            }
        }

        public ApiResponse<GameboardAndActionsDto> PerformBotAction(Gameboard gameboard)
        {
            try
            {
                var chessBot = new ChessBot(gameboard);

                var stopwatch = Stopwatch.StartNew();
                var chessBotAction = chessBot.CalculateBestAction(3);
                stopwatch.Stop();

                Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds}ms");
                Console.WriteLine("-----------------");

                gameboard.ProcessTurn(chessBotAction);
                var actionsDto = GetActionsDto(gameboard);
                var successObject = new GameboardAndActionsDto { Gameboard = gameboard, Actions = actionsDto };
                return ApiResponse<GameboardAndActionsDto>.CreateSuccessResponse(successObject);
            }
            catch (Exception e)
            {
                return ApiResponse<GameboardAndActionsDto>.CreateErrorResponse(e.Message);
            }
        }

        public ApiResponse<GameboardAndActionsDto> ParseFen(string fen)
        {
            try
            {
                var gameboard = ForsythEdwardsNotation.ParseFen(fen);
                var actionsDto = GetActionsDto(gameboard);
                var successObject = new GameboardAndActionsDto { Gameboard = gameboard, Actions = actionsDto };
                return ApiResponse<GameboardAndActionsDto>.CreateSuccessResponse(successObject);
            }
            catch (Exception e)
            {
                return ApiResponse<GameboardAndActionsDto>.CreateErrorResponse(e.Message);
            }
        }

        public ApiResponse<string> GenerateFen(Gameboard gameboard)
        {
            try
            {
                var generatedFen = ForsythEdwardsNotation.GenerateFen(gameboard);
                return ApiResponse<string>.CreateSuccessResponse(generatedFen);
            }
            catch (Exception e)
            {
                return ApiResponse<string>.CreateErrorResponse(e.Message);
            }
        }
    }
}

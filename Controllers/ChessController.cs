using Chess.Classes;
using ChessAPI.Models;
using ChessAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChessController : ControllerBase
    {
        private readonly ChessService _chessService;
        public ChessController(ChessService chessService)
        {
            _chessService = chessService;
        }

        [HttpGet("initialState")]
        [SwaggerOperation(
            Summary = "Gets the initial gameboard and starting actions for the White team.")]
        [ProducesResponseType(typeof(GameboardAndActionsDto), 200)]
        public ActionResult<GameboardAndActionsDto> GetInitialBoard()
        {
            var gameboard = new Gameboard();
            gameboard.InitialiseStandardBoardState();

            var dictionaryActions = gameboard.CalculateTeamActions(gameboard.CurrentTeamColour);
            var dtoList = _chessService.DictionaryToPieceActionDto(dictionaryActions);

            var returnObject = new GameboardAndActionsDto { Gameboard = gameboard, Actions = dtoList };

            return Ok(returnObject);
        }

        [HttpPost("perform")]
        [SwaggerOperation(
            Summary = "Perform the provided Action on the provided board.",
            Description = "Returns the new gameboard after the action is performed, as well as actions available to the next team.")]
        [ProducesResponseType(typeof(GameboardAndActionsDto), 200)]
        public ActionResult<GameboardAndActionsDto> PerformAction([FromBody] PerformGameboardActionRequest gameboardActionRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Gameboard parameter or Action parameter is either missing properties or is invalid.");

            var gameboard = gameboardActionRequest.Gameboard;
            gameboard.PerformAction(gameboardActionRequest.Action);

            var dictionaryActions = gameboard.CalculateTeamActions(gameboard.CurrentTeamColour);
            var dtoList = _chessService.DictionaryToPieceActionDto(dictionaryActions);

            var returnObject = new GameboardAndActionsDto { Gameboard = gameboard, Actions = dtoList };

            return Ok(returnObject);
        }


        [HttpPost("botAction")]
        [SwaggerOperation(
            Summary = "Performs the bot's action on the provided board",
            Description = "Returns the new gameboard after the action is performed, as well as actions available to the next team.")]
        public ActionResult<GameboardAndActionsDto> PerformBotAction([FromBody] Gameboard gameboard)
        {
            if (!ModelState.IsValid)
                return BadRequest("Gameboard parameter is either missing properties or is invalid.");

            var chessBot = new ChessBot.ChessBot(ChessBot.BotDifficulty.Easy, gameboard);
            var chessBotAction = chessBot.CalculateBestAction();
            gameboard.PerformAction(chessBotAction);

            var dictionaryActions = gameboard.CalculateTeamActions(gameboard.CurrentTeamColour);
            var dtoList = _chessService.DictionaryToPieceActionDto(dictionaryActions);

            var returnObject = new GameboardAndActionsDto { Gameboard = gameboard, Actions = dtoList };

            return Ok(returnObject);
        }
    }
}

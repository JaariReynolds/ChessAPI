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
        private readonly IChessService _chessService;
        public ChessController(IChessService chessService)
        {
            _chessService = chessService;
        }

        [HttpGet("initialState")]
        [SwaggerOperation(
            Summary = "Gets the initial Gameboard and starting Actions for the White team.")]
        [ProducesResponseType(typeof(GameboardAndActionsDto), 200)]
        public ActionResult<GameboardAndActionsDto> GetInitialBoard()
        {
            var result = _chessService.GetInitialBoard();
            return Ok(result);
        }

        [HttpPost("perform")]
        [SwaggerOperation(
            Summary = "Perform the provided Action on the provided board.",
            Description = "Returns the new gameboard after the action is performed, as well as actions available to the next team.")]
        [ProducesResponseType(typeof(GameboardAndActionsDto), 200)]
        public ActionResult<GameboardAndActionsDto> PerformAction([FromBody] PerformGameboardActionRequest gameboardActionRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _chessService.PerformAction(gameboardActionRequest.Gameboard, gameboardActionRequest.Action);
            return Ok(result);
        }

        [HttpPost("botAction")]
        [SwaggerOperation(
            Summary = "Performs the bot's Action on the provided board.",
            Description = "Returns the new gameboard after the action is performed, as well as actions available to the next team.")]
        public ActionResult<GameboardAndActionsDto> PerformBotAction([FromBody] Gameboard gameboard)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _chessService.PerformBotAction(gameboard);
            return Ok(result);
        }

        [HttpPost("importFen")]
        [SwaggerOperation(
            Summary = "Imports the provided FEN.",
            Description = "Converts the FEN string into the equivalent Gameboard object, then returns the Gameboard as well as the available Actions for the current team.")]
        public ActionResult<GameboardAndActionsDto> ImportFen([FromBody] ForsythEdwardsNotationRequest fenRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _chessService.ParseFen(fenRequest.FenString);
            return Ok(result);
        }

        [HttpPost("exportFen")]
        [SwaggerOperation(
            Summary = "Exports the FEN for the provided Gameboard.",
            Description = "Converts the provided Gameboard into the equivalent FEN string.")]
        public ActionResult<string> ExportFen([FromBody] Gameboard gameboard)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _chessService.GenerateFen(gameboard);
            return Ok(result);
        }
    }
}

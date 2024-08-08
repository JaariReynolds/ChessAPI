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
        [SwaggerOperation(Summary = "Get the initial board state for a chess game")]
        [ProducesResponseType(typeof(Gameboard), 200)]
        public IActionResult GetInitialBoard()
        {
            var gameboard = new Gameboard();
            gameboard.InitialiseStandardBoardState();
            return Ok(gameboard);
        }

        [HttpPost("actions")]
        [SwaggerOperation(Summary = "Get legal actions for the current team on the provided board")]
        [ProducesResponseType(typeof(List<PieceActionDto>), 200)]
        public IActionResult GetActionsForCurrentState([FromBody] Gameboard gameboard)
        {
            if (!ModelState.IsValid)
                return BadRequest("Gameboard parameter is either missing properties or is invalid.");

            var dictionaryActions = gameboard.CalculateTeamActions(gameboard.CurrentTeamColour);
            var dtoList = _chessService.DictionaryToPieceActionDto(dictionaryActions);

            return Ok(dtoList);
        }

        [HttpPost("perform")]
        [SwaggerOperation(Summary = "Perform the provided Action on the provided board")]
        [ProducesResponseType(typeof(Gameboard), 200)]
        public IActionResult PerformAction([FromBody] GameboardActionRequest gameboardActionRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Gameboard parameter or Action parameter is either missing properties or is invalid.");

            var gameboard = gameboardActionRequest.Gameboard;
            gameboard.PerformAction(gameboardActionRequest.Action);

            return Ok(gameboard);
        }
    }
}

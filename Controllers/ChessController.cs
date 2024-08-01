using Chess.Classes;
using ChessAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChessController : ControllerBase
    {

        [HttpGet("initialState")]
        [SwaggerOperation(Summary = "Get the initial board state for a chess game")]
        [ProducesResponseType(typeof(Gameboard), 200)]
        public IActionResult GetInitialBoard()
        {
            var gameboard = new Gameboard();
            gameboard.InitialiseTestBoardState();
            return Ok(gameboard);
        }

        [HttpPost("actions")]
        [SwaggerOperation(Summary = "Get legal actions for the current team on the provided board")]
        [ProducesResponseType(typeof(List<Action>), 200)]
        public IActionResult GetActionsForCurrentState([FromBody] Gameboard gameboard)
        {
            if (!ModelState.IsValid)
                return BadRequest("Gameboard parameter is either missing properties or is invalid.");

            return Ok(gameboard.CalculateTeamActions(gameboard.CurrentTeamColour));
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

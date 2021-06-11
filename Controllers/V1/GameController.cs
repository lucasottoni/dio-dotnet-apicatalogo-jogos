using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("/api/V1/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            this._gameService = gameService;
        }

        /// <summary>
        /// Find all games in a paginated search
        /// </summary>
        /// <param name="page">Which page is being searched. Starts at 1</param>
        /// <param name="pageSize">Number of registers in a page. Mininum 1 and maximum of 50</param>
        /// <returns>List of Games</returns>
        /// <response code="200">Success and returns the list of games</response>
        /// <response code="204">No games has being found</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameViewModel>>> ListGames([FromQuery, Range(1, int.MaxValue)] int page = 1,
        [FromQuery, Range(1, 50)] int pageSize = 5)
        {
            var result = await _gameService.ListGames(page, pageSize);
            if (result.Count == 0)
                return NoContent();
            return Ok(result);
        }

        /// <summary>
        /// Find a game by its identifier
        /// </summary>
        /// <param name="gameId">Game Identifier</param>
        /// <returns>The game found</returns>
        /// <response code="200">The game found with the id provided</response>
        /// <response code="404">No game found with the id</response>
        [HttpGet("{gameId:guid}")]
        public async Task<ActionResult<GameViewModel>> GetGame([FromRoute] Guid gameId)
        {
            var result = await _gameService.GetGame(gameId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GameViewModel>> CreateGame([FromBody] GameInputModel game)
        {
            try
            {
                var result = await _gameService.CreateGame(game);
                return Created(result.Id.ToString(), result);
            }
            catch (GameAlreadyExistsException)
            {
                return UnprocessableEntity("This game is already registered with this producer");
            }
        }

        [HttpPut("{gameId:guid}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid gameId, [FromBody] GameInputModel game)
        {
            try
            {
                await _gameService.UpdateGame(gameId, game);
                return Ok();
            }
            catch (GameNotFoundException)
            {
                return NotFound("This game does not exist");
            }
        }

        [HttpPatch("{gameId:guid}/price/{price:double}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid gameId, [FromRoute] double price)
        {
            try
            {
                await _gameService.UpdatePrice(gameId, price);
                return Ok();
            }
            catch (GameNotFoundException)
            {
                return NotFound("This game does not exist");
            }
        }

        [HttpDelete("{gameId:guid}")]
        public async Task<ActionResult> DeleteGame([FromRoute] Guid gameId)
        {
            try
            {
                await _gameService.DeleteGame(gameId);
                return Ok();
            }
            catch (GameNotFoundException)
            {
                return NotFound("This game does not exist");
            }
        }
    }
}
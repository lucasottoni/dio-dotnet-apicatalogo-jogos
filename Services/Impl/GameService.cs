using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Repositories;
using ApiCatalogoJogos.ViewModel;
using ApiCatalogoJogos.Entities;
using ApiCatalogoJogos.Exceptions;

namespace ApiCatalogoJogos.Services.Impl
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            this._gameRepository = gameRepository;
        }

        public async Task<GameViewModel> CreateGame(GameInputModel gameInput)
        {
            var gameEntity = await _gameRepository.FindByNameAndProducer(gameInput.Name, gameInput.Producer);

            if (gameEntity.Count > 0)
                throw new GameAlreadyExistsException(gameInput.Name, gameInput.Producer);

            var gameInsert = new Game
            {
                Id = Guid.NewGuid(),
                Name = gameInput.Name,
                Producer = gameInput.Producer,
                Price = gameInput.Price
            };

            await _gameRepository.Create(gameInsert);

            return Map(gameInsert);
        }

        public async Task DeleteGame(Guid id)
        {
            var game = await _gameRepository.FindById(id);

            if (game == null)
                throw new GameNotFoundException(id);

            await _gameRepository.Delete(id);
        }

        public void Dispose()
        {
            _gameRepository?.Dispose();
        }

        public async Task<GameViewModel> GetGame(Guid id)
        {
            var game = await _gameRepository.FindById(id);

            return Map(game);
        }

        public async Task<List<GameViewModel>> ListGames(int page, int pageSize)
        {
            var games = await _gameRepository.List(page, pageSize);

            return games.Select(g => Map(g)).ToList();
        }

        public async Task UpdateGame(Guid id, GameInputModel gameInput)
        {
            var game = await _gameRepository.FindById(id);

            if (game == null)
                throw new GameNotFoundException(id);

            game.Name = gameInput.Name;
            game.Producer = gameInput.Producer;
            game.Price = gameInput.Price;

            await _gameRepository.Update(game);
        }

        public async Task UpdatePrice(Guid id, double price)
        {
            var game = await _gameRepository.FindById(id);

            if (game == null)
                throw new GameNotFoundException(id);

            game.Price = price;

            await _gameRepository.Update(game);
        }

        private GameViewModel Map(Game g)
        {
            if (g == null)
                return null;
            return new GameViewModel
            {
                Id = g.Id,
                Name = g.Name,
                Producer = g.Producer,
                Price = g.Price
            };
        }
    }
}
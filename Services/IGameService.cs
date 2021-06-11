using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.ViewModel;

namespace ApiCatalogoJogos.Services
{
    public interface IGameService : IDisposable
    {
        Task<List<GameViewModel>> ListGames(int page, int pageSize);
        Task<GameViewModel> GetGame(Guid id);
        Task<GameViewModel> CreateGame(GameInputModel game);
        Task UpdateGame(Guid id, GameInputModel game);
        Task UpdatePrice(Guid id, double price);
        Task DeleteGame(Guid id);
    }
}
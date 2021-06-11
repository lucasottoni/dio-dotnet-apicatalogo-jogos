using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCatalogoJogos.Entities;

namespace ApiCatalogoJogos.Repositories
{
    public interface IGameRepository : IDisposable
    {
        Task<List<Game>> List(int page, int pageSize);
        Task<Game> FindById(Guid id);
        Task<List<Game>> FindByNameAndProducer(string name, string producer);
        Task Create(Game game);
        Task Update(Game game);
        Task Delete(Guid id);
    }
}
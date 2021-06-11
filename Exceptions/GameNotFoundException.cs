using System;

namespace ApiCatalogoJogos.Exceptions
{
    public class GameNotFoundException : Exception
    {

        public GameNotFoundException(Guid id)
        : base(String.Format("Game {0} does not exists", id))
        {

        }
    }
}
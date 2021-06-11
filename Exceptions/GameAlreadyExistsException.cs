using System;

namespace ApiCatalogoJogos.Exceptions
{
    public class GameAlreadyExistsException : Exception
    {
        public GameAlreadyExistsException(string name, string producer) :
        base(String.Format("The game {0} from {1} already exists", name, producer))
        {

        }

    }
}
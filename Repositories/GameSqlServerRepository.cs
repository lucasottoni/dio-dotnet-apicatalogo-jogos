using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ApiCatalogoJogos.Entities;
using Microsoft.Extensions.Configuration;

namespace ApiCatalogoJogos.Repositories
{
    public class GameSqlServerRepository : IGameRepository
    {
        private readonly SqlConnection sqlConnection;

        class Fields
        {
            internal const string ID = "id";
            internal const string NAME = "name";
            internal const string PRODUCER = "producer";
            internal const string PRICE = "price";
        }

        public GameSqlServerRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task Create(Game game)
        {
            var cmd = $@"INSERT games ({Fields.ID}, {Fields.NAME}, {Fields.PRODUCER}, {Fields.PRICE}) 
            values (@id, @name, @producer, @price)";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@id", game.Id);
            sqlCommand.Parameters.AddWithValue("@name", game.Name);
            sqlCommand.Parameters.AddWithValue("@producer", game.Producer);
            sqlCommand.Parameters.AddWithValue("@price", game.Price);

            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Delete(Guid id)
        {
            var cmd = $"DELETE FROM games WHERE Id = @id";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id", id);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }

        public async Task<Game> FindById(Guid id)
        {
            Game game = null;

            var cmd = $"SELECT * FROM games where Id = @id";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id", id);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                game = Map(sqlDataReader);
            }

            await sqlConnection.CloseAsync();

            return game;
        }

        public async Task<List<Game>> FindByNameAndProducer(string name, string producer)
        {
            var games = new List<Game>();

            var cmd = @"SELECT *
                FROM games
                WHERE name = @name and producer = @producer";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@name", name);
            sqlCommand.Parameters.AddWithValue("@producer", producer);

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(Map(sqlDataReader));
            }

            await sqlConnection.CloseAsync();

            return games;
        }

        public async Task<List<Game>> List(int page, int pageSize)
        {
            var games = new List<Game>();

            var cmd = $@"SELECT *
                FROM games
                ORDER BY id
                offset {((page - 1) * pageSize)} rows fetch next {pageSize} rows only";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(Map(sqlDataReader));
            }

            await sqlConnection.CloseAsync();

            return games;
        }

        public async Task Update(Game game)
        {
            var cmd = $@"UPDATE games 
            SET name = @name, 
            producer = @producer, 
            price = @price 
            WHERE id = @id";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@id", game.Id);
            sqlCommand.Parameters.AddWithValue("@name", game.Name);
            sqlCommand.Parameters.AddWithValue("@producer", game.Producer);
            sqlCommand.Parameters.AddWithValue("@price", game.Price);

            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        private Game Map(SqlDataReader sqlDataReader)
        {
            return new Game
            {
                Id = (Guid)sqlDataReader[Fields.ID],
                Name = (string)sqlDataReader[Fields.NAME],
                Producer = (string)sqlDataReader[Fields.PRODUCER],
                Price = Convert.ToDouble(sqlDataReader[Fields.PRICE])
            };
        }
    }
}
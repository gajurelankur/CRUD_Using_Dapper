using Dapper;
using DapperWithCrud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperWithCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public GamesController(IConfiguration config)
        {
           _config = config;
        }

        [HttpGet]

        public async Task<ActionResult<List<Games>>> GetAllGames()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Games> game = await SelectAllGames(connection);
            return Ok(game);

        }


        [HttpGet("{Id}")]

        public async Task<ActionResult<List<Games>>> GetGames(int Id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var game = await connection.QueryFirstAsync<Games>("Select * from Games where GameID = @GameID", new { GameID = Id });
            return Ok(game);

        }

        [HttpPost]

        public async Task<ActionResult<List<Games>>> CreateAllGames(Games game)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into Games (Title,ReleaseYear,Platform,Rating) values (@Title,@ReleaseYear,@Platform,@Rating)",game);
            return Ok(await SelectAllGames(connection));

        }

        [HttpPut]

        public async Task<ActionResult<List<Games>>> UpdateGames(Games game)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Update Games set Title=@title,ReleaseYear=@ReleaseYear,Platform=@Platform,Rating=@Rating where Gameid=@Gameid", game);
            return Ok(await SelectAllGames(connection));

        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<List<Games>>> DeleteGames(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Delete from Games where gameid=@gameid", new {Gameid = id} );
            return Ok(await SelectAllGames(connection));

        }

        private static async Task<IEnumerable<Games>> SelectAllGames(SqlConnection connection)
        {
            return await connection.QueryAsync<Games>("Select * from Games");
        }
    }
}

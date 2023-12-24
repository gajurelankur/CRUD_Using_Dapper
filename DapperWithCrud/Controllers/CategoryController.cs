using Dapper;
using DapperWithCrud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperWithCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CategoryController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Category> types = await SelectAllCategory(connection);
            return Ok(types);

        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<Category>> GetCategory(int Id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var type = await connection.QueryFirstAsync<Category>("Select * from Categories where id= @Id", 
                new { Id = Id });
            return Ok(type);

        }

        [HttpPost]
        public async Task<ActionResult<List<Category>>> CreateCategory(Category type)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into  categories (Title, DisplayOrder) values (@title, @DisplayOrder)",type);
            return Ok(await SelectAllCategory(connection));

        }

        [HttpPut]
        public async Task<ActionResult<List<Category>>> UpdateCategory(Category type)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Update  categories set Title=@Title,DisplayOrder =@DisplayOrder where id=@id", type);
            return Ok(await SelectAllCategory(connection));

        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<List<Category>>> DeleteCategory(int Id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Delete from categories where id = @id", new { Id = Id });
            return Ok(await SelectAllCategory(connection));

        }

        private static async Task<IEnumerable<Category>> SelectAllCategory(SqlConnection connection)
        {
            return await connection.QueryAsync<Category>("Select * from Categories");
        }



    }
}

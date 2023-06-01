using Dapper;
using FreeCourse.Services.Discount.Models;
using FreeCourse.Services.Discount.Services.Interfaces;
using FreeCourse.Shared.Dtos;
using Npgsql;
using System.Data;

namespace FreeCourse.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;  // herhangi bir db ye bağlanmak için kullanılan interface
        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<List<Discounts>>> GetAllDiscountsAsync()
        {                                                   // mapper ile discountDto ile de işlem yapılabilirdi
            var allDiscounts = await _dbConnection.QueryAsync<Discounts>("Select * from discounts");

            return Response<List<Discounts>>.Success(allDiscounts.ToList(),200); ;
        }

        public async Task<Response<Discounts>> GetDiscountsByIdAsync(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Discounts>
                             ("Select * from discounts where id = @Id", new { Id = id })
                           ).SingleOrDefault();

            if (discount == null) return Response<Discounts>.Fail("Discount not found",404);

            return Response<Discounts>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> SaveAsync(Discounts discounts)
        {
            var saveStatus = await _dbConnection.
                         ExecuteAsync("INSERT INTO discounts (userid,rate,code) VALUES (@UserId,@Rate,@Code)", discounts);

            if (saveStatus > 0) return Response<NoContent>.Success(204);

            return Response<NoContent>.Fail("An error occured while adding",500);
        }

        public async Task<Response<NoContent>> UpdateAsync(Discounts discounts)
        {
            var updateStatus = await _dbConnection.
                      //ExecuteAsync("UPDATE discounts SET userid = @UserId,rate = @Rate,code=@Code where id=@Id", discounts);
                      ExecuteAsync("UPDATE discounts SET userid = @UserId,rate = @Rate,code=@Code where id=@Id"
                      , new { Id = discounts.Id, Code = discounts.Code, Rate = discounts.Rate , UserId = discounts.UserId });

            if (updateStatus > 0) return Response<NoContent>.Success(204);

            return Response<NoContent>.Fail("Discount not found", 404);
        }

        public async Task<Response<NoContent>> DeleteAsync(int id)
        {
            var deleteStatus = await _dbConnection.ExecuteAsync("Delete from discounts where id = @Id", new { Id = id });

            return deleteStatus > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Discount not found", 404);
        }

        public async Task<Response<Discounts>> GetByCodeAndUserIdAsync(string code, string userId)
        {
            var discount = await _dbConnection.QueryAsync<Discounts>
                     ("SELECT * FROM discounts where code = @Code and userId = @UserId",
                      new {Code = code , UserId = userId});
            var hasDiscount = discount.FirstOrDefault();
            return hasDiscount == null ? Response<Discounts>.Fail("Discount not found", 404) : Response<Discounts>.Success(hasDiscount, 200);
        }

    }
}

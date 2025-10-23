using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeiraApi.Data;
using PrimeiraApi.Data.Entities;
using PrimeiraApi.DTOs;
using PrimeiraApi.Services; 

namespace PrimeiraApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AlphaVantageService _alphaVantageService; 

        public UsersController(ApplicationDbContext context, AlphaVantageService alphaVantageService)
        {
            _context = context;
            _alphaVantageService = alphaVantageService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User newUser)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Email == newUser.Email);
            if (userExists)
            {
                return BadRequest("Um usuário com este e-mail já existe.");
            }

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var userResponse = new UserResponseDto
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email
            };

            return CreatedAtAction(nameof(CreateUser), new { id = userResponse.Id }, userResponse);
        }

        [HttpPost("{userId}/favorites")]
        public async Task<IActionResult> AddFavoriteStock(int userId, [FromBody] AddFavoriteStockDto favoriteStockDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var ticker = favoriteStockDto.Ticker.ToUpper();
            var alreadyFavorite = await _context.FavoriteStocks
                .AnyAsync(fs => fs.UserId == userId && fs.Ticker == ticker);

            if (alreadyFavorite)
            {
                return BadRequest("Esta ação já está na lista de favoritos.");
            }

            var newFavorite = new FavoriteStock
            {
                Ticker = ticker,
                UserId = userId
            };

            _context.FavoriteStocks.Add(newFavorite);
            await _context.SaveChangesAsync();

            var favoriteResponse = new FavoriteStockResponseDto
            {
                Id = newFavorite.Id,
                Ticker = newFavorite.Ticker,
                UserId = newFavorite.UserId
            };

            return Ok(favoriteResponse);
        }

        [HttpGet("{userId}/favorites")]
        public async Task<IActionResult> GetFavoriteStocksWithQuotes(int userId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteStocks) 
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            if (!user.FavoriteStocks.Any())
            {
                return Ok(new List<FavoriteStockQuoteDto>());
            }

            var quoteTasks = user.FavoriteStocks
                .Select(fav => _alphaVantageService.GetRealTimeQuoteAsync(fav.Ticker))
                .ToList();

            var quotes = await Task.WhenAll(quoteTasks);

            var response = quotes
                .Where(q => q != null)
                .Select(q => new FavoriteStockQuoteDto
                {
                    Ticker = q!.Symbol,
                    Price = q.Price,
                    ChangePercent = q.ChangePercent
                }).ToList();

            return Ok(response);
        }
    }
}
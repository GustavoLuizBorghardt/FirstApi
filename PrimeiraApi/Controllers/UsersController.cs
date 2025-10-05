using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeiraApi.Data;
using PrimeiraApi.Data.Entities;
using PrimeiraApi.DTOs;
using PrimeiraApi.Services; // Adicionar o using para o serviço da AlphaVantage

namespace PrimeiraApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AlphaVantageService _alphaVantageService; // Adicionar o serviço

        // Agora injetamos os dois serviços que o controller precisa
        public UsersController(ApplicationDbContext context, AlphaVantageService alphaVantageService)
        {
            _context = context;
            _alphaVantageService = alphaVantageService;
        }

        // ENDPOINT 1: Criar um novo usuário (sem alterações)
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

        // ENDPOINT 2: Adicionar uma ação favorita (com melhoria anti-duplicados)
        [HttpPost("{userId}/favorites")]
        public async Task<IActionResult> AddFavoriteStock(int userId, [FromBody] AddFavoriteStockDto favoriteStockDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // BÔNUS: Verificação para não adicionar favoritos duplicados
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

        // NOVO ENDPOINT 3: Listar ações favoritas com cotações
        // Rota: GET /api/users/{userId}/favorites
        [HttpGet("{userId}/favorites")]
        public async Task<IActionResult> GetFavoriteStocksWithQuotes(int userId)
        {
            // 1. Encontra o usuário e carrega sua lista de ações favoritas junto
            var user = await _context.Users
                .Include(u => u.FavoriteStocks) // "Include" carrega os dados da tabela relacionada
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            if (!user.FavoriteStocks.Any())
            {
                return Ok(new List<FavoriteStockQuoteDto>()); // Retorna uma lista vazia se não houver favoritos
            }

            // 2. Para cada ação favorita, cria uma "tarefa" para buscar sua cotação.
            //    Isso permite que todas as chamadas para a Alpha Vantage rodem em paralelo,
            //    o que é muito mais rápido!
            var quoteTasks = user.FavoriteStocks
                .Select(fav => _alphaVantageService.GetRealTimeQuoteAsync(fav.Ticker))
                .ToList();

            // 3. Espera todas as tarefas terminarem
            var quotes = await Task.WhenAll(quoteTasks);

            // 4. Monta a lista de resposta final, juntando os dados
            var response = quotes
                .Where(q => q != null) // Ignora se alguma cotação falhou
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
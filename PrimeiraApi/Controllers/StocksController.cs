using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PrimeiraApi.Services;

namespace AlphaVantage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly AlphaVantageService _alphaVantageService;

        public StocksController(AlphaVantageService alphaVantageService)
        {
            _alphaVantageService = alphaVantageService;
        }

        // ENDPOINT 1 Retorna a cotação mais recente de uma ação
        [HttpGet("quote/{symbol}"), EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRealTimeQuote(string symbol)
        {
            var quote = await _alphaVantageService.GetRealTimeQuoteAsync(symbol);
            if (quote == null)
            {
                return NotFound($"Cotação para o símbolo '{symbol}' não encontrada.");
            }
            return Ok(quote);
        }

        // ENDPOINT 2 Ação que mais valorizou
        [HttpGet("top-gainers"), EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTopGainers()
        {
            var movers = await _alphaVantageService.GetTopMoversAsync();
            if (movers == null || !movers.TopGainers.Any())
            {
                return NotFound("Não foi possível obter a lista de ações que mais valorizaram.");
            }
            return Ok(movers.TopGainers);
        }

        // ENDPOINT 3: Ação que menos valorizou
        [HttpGet("top-losers"), EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTopLosers()
        {
            var movers = await _alphaVantageService.GetTopMoversAsync();
            if (movers == null || !movers.TopLosers.Any())
            {
                return NotFound("Não foi possível obter a lista de ações que menos valorizaram.");
            }
            return Ok(movers.TopLosers);
        }

        // ENDPOINT 4 Ação com maior sequência de altas
        [HttpGet("growth-streak/{symbol}"), EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLongestGrowthStreak(string symbol)
        {
            var result = await _alphaVantageService.GetLongestGrowthStreakAsync(symbol);
            return Ok(new { Symbol = result.Symbol, LongestStreakInDays = result.Streak });
        }

        // ENDPOINT 5 Consulta livre com OData
        [HttpGet("history/{symbol}"), EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDailyHistory(string symbol)
        {
            var dailyInfo = await _alphaVantageService.GetDailyInfoAsync(symbol);
            if (dailyInfo?.TimeSeries == null)
            {
                return NotFound($"Não foram encontrados dados diários para o símbolo '{symbol}'.");
            }
            return Ok(dailyInfo.TimeSeries.Values);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PrimeiraApi.Models;
using PrimeiraApi.Services;

namespace PrimeiraApi.Controllers
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

        [HttpGet("quote-by-name/{companyName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetQuoteByCompanyName(string companyName)
        {
            var quote = await _alphaVantageService.TranslateAndGetQuoteAsync(companyName);
            if (quote == null)
            {
                return NotFound($"Não foi possível encontrar a cotação para a empresa '{companyName}'. Verifique o nome ou se a tradução existe.");
            }
            return Ok(quote);
        }

        [HttpGet("quote/{symbol}")]
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

        [HttpGet("growth-streak/{symbol}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLongestGrowthStreak(string symbol)
        {
            var result = await _alphaVantageService.GetLongestGrowthStreakAsync(symbol);
            return Ok(new { Symbol = result.Symbol, LongestStreakInDays = result.Streak });
        }

        [HttpGet("history/{symbol}")]
        [EnableQuery]
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
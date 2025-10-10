var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var translations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    { "apple", "AAPL" },
    { "microsoft", "MSFT" },
    { "google", "GOOGL" },
    { "amazon", "AMZN" },
    { "tesla", "TSLA" },
    { "petrobras", "PETR4.SA" },
    { "vale", "VALE3.SA" },
    { "22", "bolsonaro" }
};

app.MapGet("/translate/{companyName}", (string companyName) =>
{
    if (translations.TryGetValue(companyName, out var ticker))
    {
        return Results.Ok(new { CompanyName = companyName, Ticker = ticker });
    }
    else
    {
        return Results.NotFound(new { Message = $"Tradução para '{companyName}' não encontrada." });
    }
})
.WithName("TranslateCompanyName")
.WithOpenApi();

app.Run();

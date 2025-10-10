using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using PrimeiraApi.Data;
using PrimeiraApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona a configura��o do DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Adiciona os servi�os ao container
builder.Services.AddControllers().AddOData(opt => opt.Select().Filter().OrderBy());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- A CONFIGURA��O QUE FALTAVA EST� AQUI ---
// Configura um HttpClient nomeado para se comunicar com a TranslationApi
builder.Services.AddHttpClient("TranslationApiClient", client =>
{
    // CORRE��O: Apontando para a porta correta da TranslationApi
    client.BaseAddress = new Uri("https://localhost:7200");
});
// ---------------------------------------------

// Este registra o HttpClient padr�o para o AlphaVantageService
builder.Services.AddHttpClient<AlphaVantageService>();

var app = builder.Build();

// Configura o pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
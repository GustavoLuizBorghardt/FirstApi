[API de Consulta de Ações]
Uma API em .NET 8 que consome dados do mercado de ações direto da Alpha Vantage.
Com ela você consegue ver as ações que mais subiram ou caíram no dia, buscar cotações em tempo real e até analisar o histórico de um ativo.

Projeto feito com uma arquitetura limpa (Controllers, Services, Models) pra ficar fácil de manter, evoluir e testar.

[Tecnologias]
.NET 8 + ASP.NET Core → construção da API
Swagger (OpenAPI) → documentação interativa
OData → filtros avançados nos endpoints
Entity Framework + PostgreSQL → planejados para utilização futura

[Como rodar]
Pré-requisitos:
SDK do .NET 8 instalado
Uma chave gratuita da Alpha Vantage (https://www.alphavantage.co/) > https://prnt.sc/CPln9o24eGqC
Git pra clonar o repositório
(Opcional) Postman ou outra ferramenta de teste

[Passo a passo]
Clone o repositório pelo CMD:
git clone https://github.com/GustavoLuizBorghardt/FirstApi.git

Abra o projeto no Visual Studio
Configure sua chave no appsettings.json:
{
  "AlphaVantage": {
    "ApiKey": "SUA_CHAVE_AQUI"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=MyStockDb;Username=postgres;Password=SUA_SENHA_DO_POSTGRES"
  }
}

[Rode o projeto]
dotnet run
Ou pelo Visual Studio (perfil https).

[Endpoints]
Todos os endpoints podem ser testados direto no Swagger:
https://localhost:[PORTA]/swagger

Ou via Postman (URL base: https://localhost:[porta]).

1. Cotação em Tempo Real
Preço mais recente de uma ação.
GET /api/stocks/quote/{symbol}
Exemplo:
https://localhost:[porta]/api/stocks/quote/MSFT

2. Maiores Altas do Dia
GET /api/stocks/top-gainers
Exemplo:
https://localhost:[porta]/api/Stocks/top-gainers

3. Maiores Baixas do Dia
GET /api/stocks/top-losers
Exemplo:
https://localhost:[porta]/api/Stocks/top-losers
Com filter: https://localhost:[porta]/api/Stocks/top-losers?$orderby=Price asc

4. Sequência de Altas
Maior sequência de dias que uma ação fechou em alta.
GET /api/stocks/growth-streak/{symbol}
Exemplo:
https://localhost:[porta]/api/Stocks/growth-streak/MSFT

5. Histórico Diário
Fechamento dos últimos 100 dias
GET /api/stocks/history/{symbol}
Exemplo:
https://localhost:[porta]/api/stocks/history/MSFT

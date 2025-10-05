API de Consulta de Ações
📖 Descrição
Uma API feita em .NET 8 que serve como um hub de informações do mercado de ações. Ela consome dados em tempo real da Alpha Vantage e também permite que usuários cadastrem suas ações favoritas, salvando tudo em um banco de dados PostgreSQL.

O objetivo é ter um serviço completo onde você pode não só consultar dados públicos, como também gerenciar informações personalizadas de usuários.

O projeto foi construído seguindo uma arquitetura limpa e organizada (Controllers, Services, Models, DTOs) para ser fácil de manter e evoluir.

🚀 Tecnologias Utilizadas
.NET 8 e ASP.NET Core → Para a construção da API.

Entity Framework Core → Para fazer a ponte entre o código C# e o banco de dados.

PostgreSQL → Nosso banco de dados para salvar usuários e suas ações favoritas.

Swagger (OpenAPI) → Para a documentação interativa dos endpoints.

OData → Para permitir filtros mais avançados em algumas consultas.

⚙️ Configuração e Instalação
Para rodar este projeto na sua máquina, siga os passos abaixo.

Pré-requisitos
SDK do .NET 8 instalado.

PostgreSQL instalado na sua máquina.

Uma chave de API gratuita da Alpha Vantage (pegue a sua aqui).

Git para clonar o repositório.

Passo a passo
Primeiro, clone o repositório para a sua máquina usando o terminal de sua preferência (CMD, PowerShell, etc.):

git clone [https://github.com/GustavoLuizBorghardt/FirstApi.git](https://github.com/GustavoLuizBorghardt/FirstApi.git)

Navegue até a pasta do projeto e abra a solução (.sln) no Visual Studio.

Configure suas chaves e senhas no arquivo appsettings.json. Ele precisa ter a sua chave da Alpha Vantage e a sua senha do PostgreSQL.

{
  "AlphaVantage": {
    "ApiKey": "SUA_CHAVE_AQUI"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=MyStockDb;Username=postgres;Password=SUA_SENHA_DO_POSTGRES"
  }
}

Muito Importante: Crie o banco de dados. Com o projeto aberto no Visual Studio, abra o Package Manager Console (Tools > NuGet Package Manager > Package Manager Console) e rode o seguinte comando para criar as tabelas:

Update-Database

Agora é só rodar o projeto! Você pode fazer isso apertando F5 no Visual Studio (usando o perfil https) ou pelo terminal com o comando:

dotnet run

Endpoints da API
Você pode testar todos os endpoints usando duas ferramentas principais:

Swagger: É a forma mais fácil. A documentação interativa abrirá automaticamente no seu navegador ao rodar o projeto. Basta encontrar o endpoint, clicar em "Try it out" e preencher os campos.

URL: https://localhost:[PORTA]/swagger

Postman: Uma ferramenta mais completa, ideal para salvar e organizar suas requisições.

URL Base: https://localhost:[PORTA] (você completa o resto do endereço para cada endpoint).

Endpoints de Ações (Dados Públicos)
Cotação em Tempo Real
Busca o preço mais recente de uma ação específica.

Método: GET

URL: /api/stocks/quote/{symbol}

Exemplo (Postman/Navegador): https://localhost:7100/api/stocks/quote/AAPL

Maiores Altas do Dia
Retorna a lista de ações que mais valorizaram.

Método: GET

URL: /api/stocks/top-gainers

Maiores Baixas do Dia
Retorna a lista de ações que mais desvalorizaram.

Método: GET

URL: /api/stocks/top-losers

Histórico Diário
Retorna os dados de fechamento dos últimos 100 dias de uma ação.

Método: GET

URL: /api/stocks/history/{symbol}

Exemplo (Postman/Navegador): https://localhost:7100/api/stocks/history/TSLA

Endpoints de Usuários (Dados Salvos no Banco)
Criar Novo Usuário
Cadastra um novo usuário no sistema.

Método: POST

URL: /api/users

Como testar (Postman):

Defina o método como POST.

Na aba Body, selecione raw e JSON.

Insira o corpo da requisição:

{
  "username": "Seu Nome",
  "email": "seu@email.com"
}

Adicionar Ação Favorita
Adiciona uma ação à lista de favoritos de um usuário.

Método: POST

URL: /api/users/{userId}/favorites

Exemplo de URL (Postman): /api/users/1/favorites

Como testar (Postman):

Defina o método como POST.

Na aba Body, selecione raw e JSON.

Insira o corpo da requisição:

{
  "ticker": "PETR4.SA"
}

Listar Cotações dos Favoritos
Busca a cotação em tempo real de todas as ações favoritas de um usuário.

Método: GET

URL: /api/users/{userId}/favorites

Exemplo de URL (Postman/Navegador): https://localhost:7100/api/users/1/favorites
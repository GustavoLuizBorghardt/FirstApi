API de Consulta de Ações (com Front-End)



📖 Descrição



Esta é uma solução completa em .NET 8 que serve como um hub de informações do mercado de ações. Ela começou como uma API simples (PrimeiraApi) que consome dados em tempo real da Alpha Vantage, mas evoluiu para incluir:



Persistência de Dados: Permite que usuários se cadastrem e salvem suas ações favoritas em um banco de dados PostgreSQL.



Microsserviço de Tradução: Uma segunda API (Minimal API, TranslationApi) que "traduz" nomes de empresas para seus tickers.



Interface Visual: Um site simples (StockApp.WebApp) feito com Razor Pages que consome a API principal, permitindo que um usuário final consulte cotações pelo nome da empresa.



Logging: Um middleware personalizado na API principal que registra informações sobre todas as requisições recebidas.



O projeto foi construído seguindo uma arquitetura limpa (Controllers, Services, Models, DTOs, Middleware) para ser fácil de manter, evoluir e testar.



🚀 Tecnologias Utilizadas



.NET 8 e ASP.NET Core: Para a construção das APIs e do site WebApp.



Razor Pages: Para a camada visual (front-end).



Minimal API: Para o microsserviço de tradução.



Entity Framework Core: Para fazer a ponte entre o código C# e o banco de dados.



PostgreSQL: Nosso banco de dados para salvar usuários e suas ações favoritas.



Swagger (OpenAPI): Para a documentação interativa dos endpoints das APIs.



OData: Para permitir filtros mais avançados em algumas consultas da API principal.



Middleware Personalizado: Para implementar o logging de requisições na API principal.



MSTest \& Moq: Para os testes unitários da lógica de negócio.



⚙️ Configuração e Instalação



Para rodar a solução completa na sua máquina, siga os passos abaixo.



Pré-requisitos



SDK do .NET 8 instalado.



PostgreSQL instalado na sua máquina (incluindo o pgAdmin, se quiser visualizar o banco).



Uma chave de API gratuita da Alpha Vantage (pegue a sua aqui).



Git para clonar o repositório.



Passo a passo



Clone o repositório para a sua máquina usando o terminal de sua preferência (CMD, PowerShell, Git Bash, etc.):



git clone \[https://github.com/GustavoLuizBorghardt/FirstApi.git](https://github.com/GustavoLuizBorghardt/FirstApi.git) # Substitua pela URL correta do seu repo!





Navegue até a pasta que o Git criou e abra o arquivo da solução (.sln) no Visual Studio.



Configure as Conexões: Abra o arquivo appsettings.json dentro do projeto PrimeiraApi. Ele precisa ter a sua chave da Alpha Vantage e a sua senha do PostgreSQL. Preencha os valores corretos:



{

&nbsp; // ... outras seções ...

&nbsp; "AlphaVantage": {

&nbsp;   "ApiKey": "SUA\_CHAVE\_ALPHA\_VANTAGE\_AQUI"

&nbsp; },

&nbsp; "ConnectionStrings": {

&nbsp;   "DefaultConnection": "Host=localhost;Port=5432;Database=MyStockDb;Username=postgres;Password=SUA\_SENHA\_DO\_POSTGRES" // Verifique a porta!

&nbsp; }

}





(Muito Importante) Crie o Banco de Dados: Com o projeto aberto no Visual Studio, abra o Package Manager Console (Tools > NuGet Package Manager > Package Manager Console). Certifique-se de que o "Default project" selecionado seja PrimeiraApi e rode o seguinte comando para criar as tabelas no seu PostgreSQL:



Update-Database





Configure a Inicialização Múltipla: Para que o site funcione, a PrimeiraApi e a TranslationApi precisam estar rodando juntas.



Clique com o botão direito na Solução no Solution Explorer > Properties.



Selecione "Multiple startup projects".



Defina a "Action" como "Start" para PrimeiraApi, TranslationApi e StockApp.WebApp. Os outros (PrimeiraApi.Tests) devem ficar como "None".



Clique Apply e OK.



Verifique as Portas: Confirme se as portas configuradas nos arquivos Program.cs da PrimeiraApi (para chamar a TranslationApi) e da StockApp.WebApp (para chamar a PrimeiraApi) correspondem às portas definidas nos launchSettings.json dos respectivos projetos.



Confie no Certificado HTTPS (se ainda não o fez): Abra um terminal como Administrador e rode:



dotnet dev-certs https --trust





Rode a Solução! Aperte F5 no Visual Studio. As duas APIs iniciarão em consoles e o site StockApp.WebApp abrirá no seu navegador.



🕹️ Como Usar



Aplicação Web (StockApp.WebApp)



A forma mais simples de interagir é através da aplicação web que abre ao rodar a solução. Basta digitar o nome de uma empresa (ex: microsoft, apple, petrobras) e clicar em "Buscar".



APIs (Swagger ou Postman)



Se quiser interagir diretamente com as APIs:



PrimeiraApi: Abra o Swagger dela no endereço https://localhost:\[PORTA\_PrimeiraApi]/swagger.



TranslationApi: Abra o Swagger dela no endereço https://localhost:\[PORTA\_TranslationApi]/swagger.



A documentação abaixo foca na PrimeiraApi, que é a principal.



Endpoints da API Principal (PrimeiraApi)



Você pode testar todos os endpoints usando duas ferramentas principais:



Swagger: É a forma mais fácil. A documentação interativa abrirá automaticamente no seu navegador ao rodar o projeto. Basta encontrar o endpoint, clicar em "Try it out" e preencher os campos.



Postman: Uma ferramenta mais completa, ideal para salvar e organizar suas requisições. Lembre-se de usar a URL Base correta (ex: https://localhost:7100).



Endpoints de Ações (Dados Públicos)



Cotação por Nome de Empresa



Busca a tradução do nome para ticker e depois a cotação.



Método: GET



URL: /api/stocks/quote-by-name/{companyName}



Exemplo (Navegador/Postman): https://localhost:7100/api/stocks/quote-by-name/apple



Cotação por Ticker



Busca o preço mais recente de uma ação específica.



Método: GET



URL: /api/stocks/quote/{symbol}



Exemplo (Navegador/Postman): https://localhost:7100/api/stocks/quote/AAPL



Maiores Altas do Dia



Retorna a lista de ações que mais valorizaram.



Método: GET



URL: /api/stocks/top-gainers



Maiores Baixas do Dia



Retorna a lista de ações que mais desvalorizaram.



Método: GET



URL: /api/stocks/top-losers



Histórico Diário



Retorna os dados de fechamento dos últimos 100 dias de uma ação (permite filtros OData).



Método: GET



URL: /api/stocks/history/{symbol}



Exemplo (Navegador/Postman): https://localhost:7100/api/stocks/history/TSLA



Maior Sequência de Altas



Calcula a maior sequência de dias consecutivos que uma ação fechou em alta.



Método: GET



URL: /api/stocks/growth-streak/{symbol}



Exemplo (Navegador/Postman): https://localhost:7100/api/stocks/growth-streak/MSFT



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

&nbsp; "username": "Seu Nome",

&nbsp; "email": "seu@email.com"

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

&nbsp; "ticker": "PETR4.SA"

}





Listar Cotações dos Favoritos



Busca a cotação em tempo real de todas as ações favoritas de um usuário.



Método: GET



URL: /api/users/{userId}/favorites



Exemplo de URL (Navegador/Postman): https://localhost:7100/api/users/1/favorites


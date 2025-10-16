API de Consulta de Ações (com Front-End Web)



📖 Sobre o Projeto



Este projeto é uma solução completa em .NET 8 para consulta de dados do mercado de ações. O que começou como uma simples API evoluiu para um sistema com múltiplos serviços e uma interface visual para o usuário.



A aplicação consome dados em tempo real da Alpha Vantage e permite que o usuário não só consulte cotações, mas também salve suas ações favoritas.



A arquitetura foi pensada para ser modular e demonstrar um ambiente similar ao de microsserviços, com diferentes projetos cuidando de diferentes responsabilidades.



🏗️ Arquitetura da Solução



A solução é composta por 4 projetos:



PrimeiraApi (Back-end Principal): O coração do sistema. Uma API robusta que busca dados da Alpha Vantage, gerencia usuários e ações favoritas salvas no banco de dados.



TranslationApi (Microsserviço): Uma Minimal API pequena e focada, responsável por "traduzir" nomes de empresas para seus tickers correspondentes.



StockApp.WebApp (Front-end): A camada visual do projeto. Uma aplicação web feita com Razor Pages que permite ao usuário final interagir com o sistema de forma amigável, sem precisar do Swagger ou Postman.



PrimeiraApi.Tests (Testes): Projeto de testes unitários (MSTest) para garantir a qualidade e a corretude da lógica de negócio.



🚀 Tecnologias Utilizadas



.NET 8 e ASP.NET Core para a construção de todos os serviços.



Entity Framework Core como ORM para comunicação com o banco.



PostgreSQL como banco de dados relacional.



Razor Pages para a construção da interface visual (front-end).



Minimal API para a criação do microsserviço de tradução.



MSTest e Moq para os testes unitários.



Swagger (OpenAPI) para a documentação interativa das APIs.



⚙️ Configuração e Instalação (Como Rodar)



Para rodar a solução completa na sua máquina, siga os passos abaixo.



Pré-requisitos:



SDK do .NET 8 instalado.



PostgreSQL instalado e rodando na sua máquina.



Uma chave gratuita da Alpha Vantage (pegue a sua aqui).



Git para clonar o repositório.



Passo a Passo:



Clone o repositório para a sua máquina usando seu terminal de preferência (CMD, PowerShell, etc.):



git clone \[https://github.com/GustavoLuizBorghardt/FirstApi.git](https://github.com/GustavoLuizBorghardt/FirstApi.git)





Abra o arquivo da solução (.sln) no Visual Studio.



Configure o appsettings.json do projeto PrimeiraApi:

Preencha sua chave da Alpha Vantage e os dados do seu banco de dados PostgreSQL.



{

&nbsp; "AlphaVantage": {

&nbsp;   "ApiKey": "SUA\_CHAVE\_AQUI"

&nbsp; },

&nbsp; "ConnectionStrings": {

&nbsp;   "DefaultConnection": "Host=localhost;Database=MyStockDb;Username=postgres;Password=SUA\_SENHA\_DO\_POSTGRES"

&nbsp; }

}





Crie o Banco de Dados (Migrations):

No Visual Studio, abra o Package Manager Console (Tools > NuGet Package Manager > ...).

Certifique-se de que o "Default project" está selecionado para PrimeiraApi e rode o comando:



Update-Database





Configure a Inicialização Múltipla:

Para que o front-end e as APIs rodem juntos, precisamos configurar o Visual Studio:



Clique com o botão direito na Solução (Solution 'PrimeiraApi') e vá em Properties.



Selecione "Multiple startup projects".



Defina a Action como "Start" para os seguintes projetos:



PrimeiraApi



TranslationApi



StockApp.WebApp



Clique em Apply e OK.



Rode o Projeto:

Pressione F5 no Visual Studio. Os 3 projetos vão iniciar, e o seu navegador abrirá a página do StockApp.WebApp.



💻 Como Usar a Aplicação



Você pode interagir com o sistema de duas formas:



Pela Aplicação Web (Recomendado):

Acesse o site que abriu no seu navegador (ex: https://localhost:7231). Use o campo de busca para consultar cotações por nome de empresa.



Pelas APIs (Swagger):

Durante a execução, as páginas do Swagger para as duas APIs também estarão disponíveis em suas respectivas portas (ex: https://localhost:7100/swagger). Você pode usá-las para testar os endpoints diretamente.



🧪 Como Rodar os Testes



Para validar a lógica de negócio, você pode executar os testes unitários:



No Visual Studio, abra o Test Explorer (Test > Test Explorer).



Clique no botão "Run All Tests in View" para executar todos os testes e ver os resultados.


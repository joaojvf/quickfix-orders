# quickfix-orders

## English
### Project Description
Project demand to create two applications, OrderGenerator and OrderAccumulator that communicate using the FIX protocol, through the QuickFixN lib (https://new.quickfixn.org/n/).
The OrderGenerator will send Purchase or Sale Orders to the OrderAccumulator, it will validate whether they are executable, if so it will execute and respond with an ExecutionReport notifying success and if not it will respond with an ExecutionReport notifying failure.

The project was implemented seeking to deliver an easy-to-maintain project and the future implementation of new versions of FIX messages, making use of SOLID, Clean Code, Design and Architecture Patterns.

### Project Characteristics
* FIX messages used are in version 4.4;
* Where there are business rules, unit tests were added using XUnit and Moq;
* Used Hexagonal Architecture pattern;
* Used EFCore in Memory database;
* Used Docker due to ease of execution in different environments.

### Improvement points
* Add AppSettings configuration;
* Configure Logging and refine messages for error analysis;
* Refine mandatory and optional fields, as per best market practices when using the FIX protocol.

### How to execute
* Ensure you have docker and docker-compose installed and configured;
* In the solution root folder, run the docker-compose up command.

### Running the tests
* In the solution root folder, run the following commands:
    * dotnet test tests/QuickFixOrders.Core.Tests/QuickFixOrders.Core.Tests.csproj
    * dotnet test tests/QuickFixOrders.OrderAccumulator.Tests/QuickFixOrders.OrderAccumulator.Tests.csproj
    * dotnet test tests/QuickFixOrders.OrderGenerator.Tests/QuickFixOrders.OrderGenerator.Tests.csproj

## Portuguese
### Descrição do Projeto

Projeto busca criar duas aplicações, OrderGenerator e OrderAccumulator que se comunicam utilizando o protocolo FIX, através da lib QuickFixN(https://new.quickfixn.org/n/).
O OrderGenerator enviará Ordens de Compra ou Venda para o OrderAccumulator validará se são executáveis, se sim irá executar e responder com um ExecutionReport notificando o sucesso e se não irá responder com um ExecutionReport notificando falha.

O projeto foi implementado buscando entregar um projeto de fácil manutenção e a implementação futura de novas versões de mensagens FIX, fazendo uso de SOLID, Clean Code, Padrões de Projeto e Arquitetura.

### Características do Projeto
* Mensagens FIX utilizadas são na versão 4.4;
* Onde há regras de negócio foi adicionado testes de unidade utilizando XUnit e Moq;
* Utilizado padrão de Arquitetura Hexagonal;
* Utilizado EFCore in Memory database;
* Utilizado Docker devido a facilidade de execução em diferentes ambientes.

### Pontos de Melhoria
* Adicionar configuração de AppSettings;
* Configurar Logging e refinar mensagens para análise de erro;
* Refinar campos obrigatórios e opcionais, tão como as melhores práticas de mercado na utilização do protocolo FIX.

### Como executar
* Certifique-se que tenha docker e docker-compose instalado e configurado;
* Na pasta raiz da solution, execute o comando docker-compose up.

### Exutando os testes
* Na pasta raiz da solution, execute os seguintes comandos:
    * dotnet test tests/QuickFixOrders.Core.Tests/QuickFixOrders.Core.Tests.csproj
    * dotnet test tests/QuickFixOrders.OrderAccumulator.Tests/QuickFixOrders.OrderAccumulator.Tests.csproj
    * dotnet test tests/QuickFixOrders.OrderGenerator.Tests/QuickFixOrders.OrderGenerator.Tests.csproj
# Sistema de Cadastro de Pacientes (Demo)
Demo de aplicação web e API CRUD em C# utilizando .NET Core 7.0, MVC, Entity Framework e PostgreSQL.

## Pré-requisitos

1. Banco de dados PostgreSQL
2. Microsoft Visual Studio Community 2022 (64-bit, projeto desenvolvido na versão 17.9.5)
3. Mínimo .NET Core 7.0
4. NuGet Package Manager
5. Postman para testes da API (ou Swagger UI que é gerado pela aplicação)

## Instruções

1. Após abrir o projeto, aguarde até que todas as dependências já tenham sido baixadas, pois a aplicação utiliza o NuGet Package Manager;
2. Alterar no arquivo appsettings.json a connection string "Postgres" para apontar para o banco de dados a ser utilizado;
3. Efetuar o build do projeto;
4. No console do Package Manager (Tools > NuGet Package Manager > Package Manager Console), executar o comando abaixo para criar a primeira migração do banco de dados:
```
Add-Migration InitialCreate
```
5. Executar o comando abaixo para criar a tabela no banco de dados:
```
Update-Database
```
6. Executar a aplicação.

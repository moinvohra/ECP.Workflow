
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/ed6ed32f4d6e4e8591da71b72b636555)](https://www.codacy.com)[![Bitbucket](https://s3.amazonaws.com/assets.coveralls.io/badges/coveralls_60.svg)](https://bitbucket.org) [![JavaScript Style Guide: Good Parts](https://img.shields.io/badge/code%20style-goodparts-brightgreen.svg?style=flat)](https://github.com/dwyl/goodparts "JavaScript The Good Parts")[
![javascript](https://aleen42.github.io/badges/src/javascript.svg)](https://www.javascript.com/)[
![CI](https://camo.githubusercontent.com/3bcf888ec2db200eba1422c065c25fe9b036a9fa/68747470733a2f2f63692e6170707665796f722e636f6d2f6170692f70726f6a656374732f7374617475732f3864736b626e393038653237766576782f6272616e63682f6d61737465723f7376673d74727565)](https://ci.appveyor.com/api/projects/status/8dskbn908e27vevx/branch/master?svg=true)
# ASP.NET Core - ECP.Workflow.API!

The project contains the APIs for creating the workflow of the company.

## Getting started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system


### Prerequisites  

 - Visual Studio (2019)  or Visual Studio Code 
 - .Net Core SDK (3.1)

### Setup

Install the Visual studio or Visual Studio code to run the project
Install .Net Core Runtime https://dotnet.microsoft.com/en-us/download/dotnet/3.1

    
### Run Project in Development enviorment
Clone the repository in your local enviorment.
Rebuild the project so all the dependant packages will be installed.
Install PostgreSql for the database
Create Database name "tsr_local"
Run the Migrations
If you are using visual studio then you can directly run the project.
If you are using Visual studio code then follow the below steps to run the project.

Run below command to clean the project. This command will cleanups the dll from your bin and obj directory.
 
    dotnet clean

 Build the project using below command 
 
    dotnet build 
Run the project using below command

    dotnet run

If you are using your local copy of the database then set your connection string in appsettings.json file of your project.

### Dependancies

#### Nuget Packages

- Microsoft.Extensions.Configuration.Abstractions
- Microsoft.Extensions.DependencyInjection.Abstractions
- Microsoft.Extensions.Diagnostics.HealthChecks
- Microsoft.Extensions.Diagnostics.HealthChecks.Abstractions
- Microsoft.Extensions.FileProviders.Abstractions
- AspNetCore.HealthChecks.NpgSql
- AspNetCore.HealthChecks.OpenIdConnectServer
- AspNetCore.HealthChecks.Rabbitmq
- AspNetCore.HealthChecks.UI.Client
- Dapper
- FluentValidation
- FluentValidation.AspNetCore
- IdentityServer4.AccessTokenValidation
- Microsoft.AspNetCore.Mvc.NewtonsoftJson
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Design
- Microsoft.Extensions.Configuration.Abstractions
- Microsoft.Extensions.DependencyInjection.Abstractions
- Microsoft.Extensions.Diagnostics.HealthChecks
- Microsoft.Extensions.Diagnostics.HealthChecks.Abstractions
- Npgsql
- Npgsql.EntityFrameworkCore.PostgreSQL
- RabbitMQ.Client
- Serilog
- Serilog.AspNetCore
- Serilog.Extensions.Logging
- Serilog.Settings.Configuration
- Serilog.Sinks.File
- Swashbuckle.AspNetCore.Swagger
- 
Note: No need to mention default packages here

### Database
Specify your database server with version and database name
i.e. 
Database Server: PostgreSQL
Database Name: tsr_local

### Project Architecture

 - ECP.Workflow.KendoGridFilter (Contains Filtering for the listing page)
 - ECP.Workflow.Repository(Contains  repositories of all the entities) 
 - ECP.Workflow.Services(Contains business   logic)
 - ECP.Workflow.Model(Contains view model)
 - ECP.Workflow.API
 - ECP.Workflow.EventPublisher (Contains code for the sending the message to MessageBroler)
 - ECP.Workflow.EventReceiver (Contains code for the Receiving the message from MessageBroler)
 - ECP.Messaging.RabbitMQ (Contains Message Broker services) 
 - ECP.Messages / ECP.Messaging.Abstraction (Contains the Message Brokers Libraries)



## Deployment
### Deployement server
If you have multiple server for the stage and production then specify all the details here

### Deployement prerequistes
If you have EC2 instance or any VPS server then install below prerequistes software
1.  Install Docker
2.  Install .Net Core Runtime

### Deployement Steps
1. Create virtual directory under IIS
2. Enable Inbound and Outbound port for PostgreSQL in Firewall
3. Create security group in your production and stage server if you are using EC2 
4. Update the database connection string with Production in appsettings.json
5. Run the Migrations
6. Publish your application from Visual Studio.
 If you are using visual studio code then use below command to publish your project. 

         dotnet publish -o <outputdir>
         
6. Move your publish files to the your server

## Versioning
Specify the version history
|Version                |Descrption	|Other Description|
|----------------|-------------------------------|-----------------------------|
|1.0.0.0|First Version            | Other Description           |

## License

This project is licensed under the Simform Solutions Pvt. Ltd
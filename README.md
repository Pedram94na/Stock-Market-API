# C_Sharp-ASP.NET-API
A web application demo for a stock market where users can sign up, add stocks to their portfolio and comment on them and more. . .

Create a .NET project:

dotnet new webapi -o api

=========================================
Run API application:

dotnet watch run

=========================================
Database Commands:

dotnet ef migrations add {Name}
dotnet ef database update

=========================================
Sample user:
{
  "userName": "Test",
  "email": "Test@gmail.com",
  "password": "Pa$$word!123"
}

{
  "userName": "Test",
  "password": "Pa$$word!123"
}

=========================================
appsettings.json addons:

"ConnectionStrings": {
    "DefaultConnection": "Data Source={PCNAME}\\SQLEXPRESS;Initial Catalog={DATABASENAME};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
  },

  "JWT": {
    "Issuer": "http://localhost:5264",
    "Audience": "http://localhost:5264",
    "SigninKey": "swordfish"
  }

========================================
dotnet Packages:

dotnet add package Microsoft.EntityFrameworkCore

=========================================
NUGET Packages:

Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore.Design
Microsoft.AspNetCore.Mvc.NewtonsoftJson
newtonsoft.Json
Microsoft.Extensions.identity.core
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.AspNetCore.Authentication.JwtBearer

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

*Check version in api.csproj >> <TargetFramework>net8.0</TargetFramework>

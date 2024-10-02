# Stock Market API
A web application demo developed with C# ASP.NET for a stock market where users can sign up, add stocks to their portfolio and comment on them and more. . .

Steps to run:
1. Navigate to the api directory:

```bash
cd .\api
```

2. Database Commands (I used SQL Server Management Studio from Microsoft)

```bash
dotnet ef migrations add {Name}
dotnet ef database update
```

3. Run the program with Swagger

```bash
dotnet watch run
```

=========================================

Sample user:

```json
{
  "userName": "Test",
  "email": "Test@gmail.com",
  "password": "Pa$$word!123"
}

{
  "userName": "Test",
  "password": "Pa$$word!123"
}
```

=========================================

appsettings.json addons:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source={PCNAME}\\SQLEXPRESS;Initial Catalog={DATABASENAME};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
},

"JWT": {
  "Issuer": "http://localhost:5264",
  "Audience": "http://localhost:5264",
  "SigninKey": "swordfish"
}
```

========================================

dotnet Packages:

```bash
dotnet add package Microsoft.EntityFrameworkCore
```

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

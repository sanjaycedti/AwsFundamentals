using Consumers.Api.Database;
using Consumers.Api.Repositories;
using Consumers.Api.Services;
using Dapper;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;

builder.Services.AddControllers();

SqlMapper.AddTypeHandler(new GuidTypeHandler());
SqlMapper.RemoveTypeMap(typeof(Guid));
SqlMapper.RemoveTypeMap(typeof(Guid?));

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
new SqliteConnectionFactory(config.GetValue<string>("Database:ConnectionString")!));

builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();


app.Run();


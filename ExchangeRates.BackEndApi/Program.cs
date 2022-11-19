using ExchangeRates.BackEndApi.Repos;
using ExchangeRates.BackEndApi.Repos.Interfaces;
using ExchangeRates.Extensions.SyncDataServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IRatesRepo, RatesRepo>();
builder.Services.AddHttpClient<IHttpDataService, HttpDataService>()
    .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
        {
           ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

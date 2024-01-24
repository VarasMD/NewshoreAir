using NewshoreAir.Business;
using NewshoreAir.DataAccess;
using NewshoreAir.Gateway;
using NewshoreAir.Interface.Business;
using NewshoreAir.Interface.DataAccess;
using NewshoreAir.Interface.Gateway;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IJourneyBusiness, JourneyBusiness>();
builder.Services.AddScoped<IJourneyDataAccess, JourneyDataAccess>();
builder.Services.AddScoped<IRouteGateway, RouteGateway>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
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

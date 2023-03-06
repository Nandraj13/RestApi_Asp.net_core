using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using RestApi.Interfaces;
using RestApi.Models;
using RestApi.Repository;
using Serilog;
using System.Diagnostics;
using BLun.ETagMiddleware;

var builder = WebApplication.CreateBuilder(args);
Stopwatch stopwatch=new Stopwatch();
// Add services to the container.
var logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().CreateLogger();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();
//builder.Services.AddMvc();
builder.Services.AddETag();
builder.Services.AddHttpLogging(httpLogging =>
{
    httpLogging.LoggingFields = HttpLoggingFields.ResponseBody | HttpLoggingFields.Request;
    httpLogging.RequestBodyLogLimit = 4096;
    httpLogging.ResponseBodyLogLimit = 4096;
});
builder.Services.AddDbContext<MasterContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEmployee, EmployeeRepo>();
builder.Services.AddEntityFrameworkInMemoryDatabase();
var app = builder.Build();

app.Use(async (context, next) =>
{
    stopwatch.Start();
    await next.Invoke();
    stopwatch.Stop();
    logger.Information(stopwatch.ElapsedMilliseconds.ToString());
    stopwatch.Reset();
});
app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.UseETag();
app.Run();

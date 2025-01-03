using DotNetCore_New.Configurations;
using DotNetCore_New.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CollegeDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection"));
});
#region Seriolog related settings
//Log.Logger = new LoggerConfiguration().
//    MinimumLevel.Debug().
//    WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Minute).CreateLogger();

//builder.Logging.AddSerilog();
#endregion
builder.Logging.AddLog4Net();

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

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

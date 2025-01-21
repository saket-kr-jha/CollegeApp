using DotNetCore_New.Configurations;
using DotNetCore_New.Data;
using DotNetCore_New.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

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
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    policy=>
    {
        policy.SetIsOriginAllowed(_ => true)
        .AllowAnyHeader().AllowCredentials().AllowAnyMethod();
    }));

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecret"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers().RequireCors("CorsPolicy");
    endpoint.MapGet("api/testendpoint",
        context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("JWTSecret")));
});

app.MapControllers();

app.Run();

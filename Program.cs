using dndsitgen.Repository;
using dndsitgen.Serveces;
using dndsitgen.Services;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<GroqService>();
builder.Services.AddHttpClient<CreaturesService>();
builder.Services.AddScoped<CreatureCalculatorService>();
builder.Configuration.AddUserSecrets<Program>();

string con_str = builder.Configuration.GetConnectionString("Default");

builder.Services.AddSingleton(con_str);
builder.Services.AddTransient<CreatureRepository>();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<CollectionRepository>();

builder.Services.AddSession();

var key = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddSingleton<JwtService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

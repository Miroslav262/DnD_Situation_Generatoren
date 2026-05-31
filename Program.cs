using dndsitgen.Serveces;
using dndsitgen.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<GroqService>();
builder.Services.AddHttpClient<CreaturesService>();
builder.Services.AddScoped<CreatureCalculatorService>();
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddSession();


var app = builder.Build();

app.UseExceptionHandler("/Home/Error");


//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


using BlogCore.AccesoDatos.Data;
using BlogCore.AccesoDatos.Inicializador;
using BlogCore.AccesoDatos.Repositorio;
using BlogCore.AccesoDatos.Repositorio.IRepositorio;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSql") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//Repositorios
builder.Services.AddScoped<IUnidadTrabajo,UnidadTrabjo>();


//Siembra de Datos
builder.Services.AddScoped<IInicializadorDB,InicializadorDB>();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IEmailSender,EmailSender>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

//Siembra de Datos
SiembraDeDatos();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Cliente}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

//Funcionalidad metodo Siembra de Datos

void SiembraDeDatos()
{
    using (var scope = app.Services.CreateScope())
    {
        var inicializadorDb = scope.ServiceProvider.GetRequiredService<IInicializadorDB>();
        inicializadorDb.Inicializar();
    }
}


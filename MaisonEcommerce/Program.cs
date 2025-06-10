using MaisonEcommerce.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ClienteRepositorio>();
builder.Services.AddScoped<LoginRepositorio>();
builder.Services.AddScoped<ProdutoRepositorio>();
builder.Services.AddScoped<PacoteRepositorio>();
builder.Services.AddScoped<PlanoRepositorio>();
builder.Services.AddScoped<FuncionarioRepositorio>();
builder.Services.AddScoped<ServicoRepositorio>();
builder.Services.AddScoped<AgendamentoRepositorio>();
builder.Services.AddScoped < ServPacoteRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();

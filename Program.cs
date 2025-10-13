using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebAPI.Data;
using WebAPI.Services;
using WebAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ControlAi", Version = "v1" });
});

//Injeção Services
builder.Services.AddScoped<IExtratoService, ExtratoService>();
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<ISaidaFixaService, SaidaFixaService>();
builder.Services.AddScoped<IEntradaFixaService, EntradaFixaService>();
builder.Services.AddScoped<IMesService, MesService>();
builder.Services.AddScoped<ITipoMovimentoService, TipoMovimentoService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapControllers();

app.Run();

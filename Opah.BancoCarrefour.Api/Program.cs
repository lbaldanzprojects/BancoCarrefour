using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Opah.BancoCarrefour.Api.Extentions;
using Opah.BancoCarrefour.Application;
using Opah.BancoCarrefour.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Opah-BancoCarrefour-Api",
        Version = "v1",
    });
});

builder.RegisterDependencies();

builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(ApplicationLayer).Assembly,
        typeof(Program).Assembly
    );
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Opah-BancoCarrefour-Api v1 — Teste");
});

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

var disableHttpsRedirect = app.Configuration.GetValue<bool>("DISABLE_HTTPS_REDIRECT");

if (!disableHttpsRedirect)
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

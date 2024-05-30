using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicketsRavelli.Infra.Data;
using Microsoft.AspNetCore.Diagnostics;
using MySqlConnector;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using TicketsRavelli.Services.Security;
using TicketsRavelli.Infrastructure.Security.Interfaces;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Application.Services.Implementations;
using TicketsRavelli.Infrastructure.EmailServices.Interfaces;
using TicketsRavelli.Infrastructure.EmailServices.Implementations;
using TicketsRavelli.Infrastructure.GerenciaNet.Implementations;
using TicketsRavelli.Infrastructure.GerenciaNet.Interfaces;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Implementations;
using FluentValidation.AspNetCore;
using TicketsRavelli.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();

//builder.Services.AddCors((options) => {
//    options.AddPolicy(name: "AllowSpecificOrigin",
//        builder => {
//            builder.WithOrigins("https://ticketsravelli.com.br");
//        });
//});

var serverVersion = new MariaDbServerVersion(new Version(10, 4, 12));
var connectionString = builder.Configuration.GetConnectionString("MySql");

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySql(connectionString, serverVersion), optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.AddTransient(x =>
  new MySqlConnection(connectionString));

#region Interfaces / Implementações
builder.Services.AddScoped<IAthleteService, AtletaService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ICortesiaService, CortesiaService>();
builder.Services.AddScoped<ISubscriptionService, InscricaoService>();
builder.Services.AddScoped<IRegistrosMedicosService, RegistrosMedicosService>();
builder.Services.AddScoped<IRegulamentoService, RegulamentoService>();
builder.Services.AddScoped<ISubCategoriaService, SubCategoriaService>();
builder.Services.AddScoped<IDescontoService, DescontoService>();
builder.Services.AddScoped<IAfiliadoService, AfiliadoService>();
builder.Services.AddScoped<ICriptografiaService, CriptografiaService>();
builder.Services.AddScoped<ISegurancaService, SegurancaService>();
builder.Services.AddScoped<IRelatoriosService, RelatoriosService>();
builder.Services.AddScoped<IUtilidadesService, UtilidadesService>();
builder.Services.AddScoped<IImagemService, ImagemService>();

builder.Services.AddScoped<IBoletoService, BoletoService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();
builder.Services.AddScoped<ILogSystem, LogSistema>();
builder.Services.AddScoped<IPixService, PixService>();
//builder.Services.AddScoped<IWebhookService, WebhookService>();

// Repositories
builder.Services.AddScoped<IAtletaRepository, AtletaRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IEventRepository, EventRepsitory>();
#endregion

builder.Services.AddControllers()
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<NewAthleteInputModelValidator>());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tickets Ravelli API", Version = "v1" });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme {
            Description = @"Cabeçalho de autorização JWT usando o esquema Bearer. \r\n\r\n 
                      Digite 'Bearer' [espaço] e, em seguida, seu token na entrada de texto abaixo.
                      Exemplo: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddAuthorization(options => {
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
      .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
      .RequireAuthenticatedUser()
      .Build();
    options.AddPolicy("EmployeePolicy", p =>
        p.RequireAuthenticatedUser().RequireClaim("employee"));
    options.AddPolicy("CpfPolicy", p =>
        p.RequireAuthenticatedUser().RequireClaim("cpf"));
    options.AddPolicy("Integracao", p =>
        p.RequireAuthenticatedUser().RequireClaim("integracao"));
});
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters() {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"]))
    };
});

var app = builder.Build();

app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

//app.UseCors("AllowSpecificOrigin");

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tickets Ravelli V1");
    });
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.Map("/error", (HttpContext http) => {
    var error = http.Features?.Get<IExceptionHandlerFeature>()?.Error;

    if (error != null) {
        switch (error) {
            case MySqlException:
                return Results.Problem(title: "Databse out", statusCode: 500);
            case FormatException:
                return Results.Problem(title: "Error to convert data to other type. Confirm all information sent", statusCode: 500);
        }
    }

    return Results.Problem(title: "An error ocurred", statusCode: 500);
});

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});

app.Run();


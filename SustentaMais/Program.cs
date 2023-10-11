using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SustentaMais.Data;
using SustentaMais.Model;
using SustentaMais.Security.Implements;
using SustentaMais.Security;
using SustentaMais.Service;
using SustentaMais.Service.Implements;
using SustentaMais.Validator;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using blogpessoal.Configuration;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace SustentaMais
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                            .AddNewtonsoftJson(options =>
                            {
                                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                            });

            // Conexão com o Banco de dados
            if (builder.Configuration["Environment:Start"] == "PROD")
            {
                // Conexão com o PostgresSQL - Nuvem

                builder.Configuration
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("secrets.json");
                
                var connectionString = builder.Configuration
               .GetConnectionString("ProdConnection");

                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(connectionString)
                );
            }
            else 
            {
                // Conexão com o SQL Server - Localhost
                var connectionString = builder.Configuration
                .GetConnectionString("DefaultConnection");

                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString)
                );
            }


            //Validação das entidades 
            builder.Services.AddTransient<IValidator<Categoria>, CategoriaValidator>();
            builder.Services.AddTransient<IValidator<Produto>, ProdutoValidator>();
            builder.Services.AddTransient<IValidator<User>, UserValidator>();

            //Registrar as classses e interfaces service
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddTransient<IAuthService, AuthService>();

            // Adicionar a Validação do Token JWT
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Settings.Secret);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                //Personalizar a Págna inicial do Swagger
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Projeto Sustenta+",
                    Description = "Projeto Sustenta+ - ASP.NET Core 7 - Entity Framework",
                    Contact = new OpenApiContact
                    {
                        Name = "Sustenta+",
                        Email = "sustentamaispi@gmail.com",
                        Url = new Uri("https://github.com/Projeto-Integrador-Sustenta-Mais"),
                    }
                });

                //Adicionar a Segurança no Swagger
                options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Digite um Token JWT válido!",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                //Adicionar a configuração visual da Segurança no Swagger
                options.OperationFilter<AuthResponsesOperationFilter>();

            });

            // Adicionar o Fluent Validation no Swagger
            builder.Services.AddFluentValidationRulesToSwagger();

            // Configuração do CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            // Add services to the container.
            //configuração para não criar um loop infinito no JASON quando fizermos requisição
            builder.Services.AddControllers()
                    .AddNewtonsoftJson(
                        Options =>
                        {
                            Options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        }
                    );

            var app = builder.Build();

            // Criar o Banco de dados e as tabelas Automaticamente
            using (var scope = app.Services.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            app.UseDeveloperExceptionPage();

            // Configure the HTTP request pipeline.

            app.UseSwagger();

            app.UseSwaggerUI();

            // Swagger Como Página Inicial - Nuvem

            if (app.Environment.IsProduction())
            {
                app.UseSwaggerUI( options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sustenta+ - v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

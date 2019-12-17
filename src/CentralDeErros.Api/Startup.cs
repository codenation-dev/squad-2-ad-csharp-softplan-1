using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CentralDeErros.Application.Mapping;
using CentralDeErros.CrossCutting.IoC;
using CentralDeErros.Data.Context;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using CentralDeErros.CrossCutting.Helpers;
using System.Text;
using System.Collections.Generic;
using CentralDeErros.Api.Graph;
using CentralDeErros.Api.Graph.Types.User;
using Microsoft.AspNetCore.Localization;
using GraphQL;
using GraphQL.Types;
using GraphiQl;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Diagnostics.CodeAnalysis;
using CentralDeErros.Api.Graph.Types.Log;

namespace CentralDeErros.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Contexto>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            MongoDbContext.ConnectionString = Configuration.GetConnectionString("mongoAtlas");

            RegisterIoC.Register(services);

            services.AddAutoMapper(typeof(AutoMappingDomainToViewModel));
            services.AddAutoMapper(typeof(AutoMappingViewModelToDomain));

            ConfigurationSwegger(services);

            ConfigurationAuthJWT(services);

            ConfigurationGraphQl(services);

            services.AddControllers()
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.AddCors();
        }

        private void ConfigurationGraphQl(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();
            services.AddScoped<CentralDeErrosQuery>();
            services.AddScoped<CentralDeErrosMutation>();
            services.AddScoped<UserType>();
            services.AddScoped<UserInputType>();
            services.AddScoped<UserFilterType>();
            services.AddScoped<LogType>();
            services.AddScoped<LogInputType>();
            services.AddScoped<LogFilterType>();

            var sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new CentralDeErrosSchema(new FuncDependencyResolver(type => sp.GetService(type))));

            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            services.AddGraphQL(_ =>
            {
                _.EnableMetrics = true;
                _.ExposeExceptions = true;
            })
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddUserContextBuilder(httpContext => httpContext.User);

            services.AddScoped<CentralDeErrosSchema>();
                        
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        private void ConfigurationSwegger(IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Central de Erros - API",
                    Description = "API Back-End - Squad 2",
                    Contact = new OpenApiContact
                    {
                        Name = "squad 2",
                        Email = "squad2_codenation@codenation.com.br",
                    }
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header usando Bearer scheme. Exemplo: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                config.IncludeXmlComments(xmlPath);
            });
        }

        private void ConfigurationAuthJWT(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("TokenConfigurations");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKeyJWT);
            var audience = Encoding.ASCII.GetBytes(appSettings.Audience);
            var issuer = Encoding.ASCII.GetBytes(appSettings.Issuer);

            // Aciona a extensão que irá configurar o uso de
            // autenticação e autorização via tokens
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.Audience = BitConverter.ToString(audience);
                x.ClaimsIssuer = BitConverter.ToString(issuer);
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });
        }

        [ExcludeFromCodeCoverage]
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            ConfigureCulture(app);
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Back-End - Squad 2");
                c.RoutePrefix = string.Empty;
            });


            app.UseGraphQL<ISchema>();
            app.UseGraphiQl("/graphql");
            app.UseGraphQL<CentralDeErrosSchema>(path: "/graphql");
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                GraphQLEndPoint = "/graphql",
                Path = "/playground",
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureCulture(IApplicationBuilder app)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("pt-BR")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
    }
}

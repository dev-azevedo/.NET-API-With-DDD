
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EscNet.DependencyInjection.IoC.Cryptography;
using Manager.API.Token;
using Manager.API.ViewModels;
using Manager.Domain.Entities;
using Manager.Infra.Context;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Manager.API
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

            services.AddControllers();

            #region Jwt
            var secreteKey = Configuration["Jwt:Key"];
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secreteKey));

            services.AddAuthentication(x => 
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => 
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeys = new List<SecurityKey>{ symmetricSecurityKey },
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion

            #region AutoMapper
            var autoMapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserDTO>().ReverseMap();
                cfg.CreateMap<CreateUserViewModel, UserDTO>().ReverseMap();
                cfg.CreateMap<UpdateUserViewModel, UserDTO>().ReverseMap();
            });

            services.AddSingleton(autoMapperConfig.CreateMapper());
            #endregion

            #region DI

            services.AddSingleton(d => Configuration);
            services.AddDbContext<ManagerContext>(option => option.UseSqlServer(Configuration["ConnectionStrings:USER_MANAGER"]), ServiceLifetime.Transient);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            
            #endregion
            
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Manager.API", 
                    Version = "v1",
                    Description = "API contruída para estudo da arquitetura DDD.",
                    Contact = new OpenApiContact
                    {
                        Name = "Jhonatan Azevedo",
                        Email = "dev.azevedo@outlook.com",
                        Url = new Uri("https://github.com/dev-azevedo")
                    }

                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
                {
                    In = ParameterLocation.Header,
                    Description = "Por favor utilize Bearer <TOKEN>",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            #endregion

            #region Cryptography
            
            services.AddRijndaelCryptography(Configuration["Cryptography:Key"]);

            #endregion
        }
 
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manager.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using System.Text;
using DicomApp.DAL.DB;
using DicomApp.Helpers.Services.GenrateAvatar;
using DicomApp.Helpers.Services.GetCounter;
using DicomApp.Helpers.Services.PayPal;
using DicomApp.Helpers.Services.Telegram;
using ECommerce.Core.Services.MailServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rotativa.AspNetCore;

namespace DicomApp.Portal
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login";
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = "GT",
                        ValidIssuer = "GT",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("GTSportsSecurityKey")
                        )
                    };
                });

            services.AddCors(o =>
                o.AddPolicy(
                    "MyPolicy",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    }
                )
            );
            services.AddMvc().AddSessionStateTempDataProvider();
            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 20971520000;
            });
            services.AddSession();
            services.AddHttpClient<AvatarService>();
            services.AddDbContext<ShippingDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DicomAppDBEntities"))
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TreeChat.API v0", Version = "v0" });
                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description =
                            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                    }
                );
                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
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
                            new string[] { }
                        }
                    }
                );
            });
            //****************** Email Settings ******************************
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            //Services
            services.AddScoped<IApiCountryService, ApiCountryService>();
            services.AddScoped<IPayPalService, PayPalService>();
            services.AddScoped<IMailServices, MailServices>();
            services.AddScoped<ITelegramService, TelegramService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession();
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Home/Error");
            }
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    ContentTypeProvider = provider,
                    ServeUnknownFileTypes = true
                }
            );
            app.UseCors("MyPolicy");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example API v1");
            });
            RotativaConfiguration.Setup(env);
        }
    }
}

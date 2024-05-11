using LegendCR.DAL.DB;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rotativa.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;


}).AddCookie(options =>
{
    options.LoginPath = "/User/Login/";

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = "GT",
        ValidIssuer = "GT",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GTSportsSecurityKey"))
    };
});

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.Configure<FormOptions>(x =>
{
    x.MultipartBodyLengthLimit = 20971520000;
});
builder.Services.AddSession();
// Add ASPNETCoreDemoDBContext services.
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Legend_CRDBEntities")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    //app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
}
var provider = new FileExtensionContentTypeProvider();

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider,
    ServeUnknownFileTypes = true

});
app.UseRouting();
app.UseCors("MyPolicy");
//app.UseMvc(routes =>
//{
//    routes.MapRoute(
//        name: "default",
//        template: "{controller=Home}/{action=Index}/{id?}");
//});
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example API v1");
//});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

RotativaConfiguration.Setup(builder.Environment.WebRootPath);
app.Run();

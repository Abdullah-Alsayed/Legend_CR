using LegendCR.Core;
using LegendCR.DAL.DB;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCors(o =>
    o.AddPolicy(
        "MyPolicy",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    )
);
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.Configure<FormOptions>(x =>
{
    x.MultipartBodyLengthLimit = 20971520000;
});

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Legend_CRDBEntities"))
);

builder
    .Services.AddIdentity<User, Role>(Option =>
    {
        Option.Password.RequireNonAlphanumeric = false;
        Option.Password.RequireDigit = false;
        Option.Password.RequiredUniqueChars = 0;
        Option.Password.RequireLowercase = false;
        Option.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}
var provider = new FileExtensionContentTypeProvider();
using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    await DataSeeder.SeedData(context, roleManager, userManager);
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles(
    new StaticFileOptions { ContentTypeProvider = provider, ServeUnknownFileTypes = true }
);
app.UseRouting();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

RotativaConfiguration.Setup(builder.Environment.WebRootPath);
app.Run();

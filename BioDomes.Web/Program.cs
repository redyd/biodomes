using System.Threading.RateLimiting;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Infrastructures;
using BioDomes.Infrastructures.Models;
using BioDomes.Infrastructures.Repositories;
using BioDomes.Web.Constraints;
using BioDomes.Web.Mapper;
using BioDomes.Web.Middlewares;
using BioDomes.Web.Middlewares.extensions;
using BioDomes.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("kebab", typeof(KebabRouteConstraint));
});

builder.Services.AddScoped<ReverseProxyLinksMiddleware>();
builder.Services.AddWebOptimizer(pipeline => { pipeline.AddCssBundle("/css/bundle.css", "css/**/*.css"); });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IImagePathResolver, ImagePathResolver>();

// repositories
builder.Services.AddDbContext<BiodomeContext>(options
    => options.UseSqlServer(
        builder.Configuration.GetConnectionString("BiodomeContext")));

builder.Services.AddDefaultIdentity<UserEntity>(options => { options.SignIn.RequireConfirmedAccount = false; })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BiodomeContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
});

builder.Services.AddScoped<IBiomeManagementRepository, BiomeManagementRepository>();
builder.Services.AddScoped<IBiomeStatRepository, BiomeStatRepository>();
builder.Services.AddScoped<ISpeciesRepository, SpeciesRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IBiomeRepository, BiomeRepository>();
builder.Services.AddScoped<ISpecieAttributesRepository, SpecieAttributesRepository>();
builder.Services.AddScoped<IFoodChainRepository, FoodChainRepository>();

builder.Services.AddScoped<IBiomeAnalyzer, BiomeAnalyzer>();

builder.Services.AddScoped<IBiomeViewMapper, BiomeViewMapper>();
builder.Services.AddScoped<IEquipmentViewMapper, EquipmentViewMapper>();
builder.Services.AddScoped<ISpecieViewMapper, SpecieViewMapper>();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 60,
                Window = TimeSpan.FromMinutes(1)
            }
        )
    );

    options.RejectionStatusCode = 429;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var adminRole = "Admin";

    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    var admin = await userManager.FindByEmailAsync("admin@biodomes.com");
    if (admin != null)
    {
        await userManager.AddPasswordAsync(admin, "Password1234!");

        if (!await userManager.IsInRoleAsync(admin, adminRole))
        {
            await userManager.AddToRoleAsync(admin, adminRole);
        }
    }

    var client1 = await userManager.FindByEmailAsync("john@biodomes.com");

    if (client1 != null)
    {
        await userManager.AddPasswordAsync(client1, "Password1234!");
    }
    
    var client2 = await userManager.FindByEmailAsync("bob@biodomes.com");

    if (client2 != null)
    {
        await userManager.AddPasswordAsync(client2, "Password1234!");
    }
}

app.UseForwardedHeaders(new ForwardedHeadersOptions());

app.UseReverseProxyMiddleware();

app.UseWebOptimizer();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot/img/uploads")),
    RequestPath = "/img/uploads"
});

app.UseRouting();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
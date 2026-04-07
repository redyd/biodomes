using BioDomes.Domains.Repository;
using BioDomes.Infrastructures;
using BioDomes.Infrastructures.Repositories;
using BioDomes.Web.Middlewares;
using BioDomes.Web.Middlewares.extensions;
using BioDomes.Web.Transformers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<RouteOptions>(options => { options.ConstraintMap.Add("kebab", typeof(SlugTransformer)); });


builder.Services.AddScoped<ReverseProxyLinksMiddleware>();
builder.Services.AddWebOptimizer(pipeline => { pipeline.AddCssBundle("/css/bundle.css", "css/**/*.css"); });

// repositories
builder.Services.AddDbContext<BiodomeContext>(options
    => options.UseSqlServer(
        builder.Configuration.GetConnectionString("BiodomeContext")));

builder.Services.AddScoped<ISpeciesRepository, SpeciesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions());

app.UseReverseProxyMiddleware();

app.UseWebOptimizer();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot/img/uploads")),
    RequestPath = "/wwwroot/img/uploads"
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
using BioDomes.Domains.Extensions;
using BioDomes.Infrastructures.repository;
using BioDomes.Web.Middlewares;
using BioDomes.Web.Middlewares.extensions;
using BioDomes.Web.Transformers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<RouteOptions>(options =>
{ 
    options.ConstraintMap.Add("kebab", typeof(SlugTransformer));
});

builder.Services.AddScoped<ReverseProxyLinksMiddleware>();
builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddCssBundle("/css/bundle.css", "css/**/*.css");
});
builder.Services.AddSingleton<IRepository, SqlRepository>();

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

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
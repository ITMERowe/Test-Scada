using InfluxDBWebApp.Services; // Make sure this namespace matches your project structure


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Register InfluxDBService with DI
builder.Services.AddSingleton(new InfluxDBService(
    "https://us-east-1-1.aws.cloud2.influxdata.com",
    "wLxNMU_7tmp82gRtqKRh8IUa5PbcYOF4PQtWioLy7wKMoBayQjzLxSEa3RqU-EeIG1wABKpJ2qkdhXtxhDFnWg==" // Replace with actual token
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

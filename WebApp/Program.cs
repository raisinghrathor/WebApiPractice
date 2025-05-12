using WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("ShirtsApi", client => {
    client.BaseAddress=new Uri("https://localhost:7283/api/");
    client.DefaultRequestHeaders.Add("Accept","application/json");
});

// Add services to the container.
builder.Services.AddHttpClient("AuthorityApi", client => {
    client.BaseAddress = new Uri("https://localhost:7283/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IWebApiExecuter,WebApiExecuter>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make the session cookie essential
});
builder.Services.AddHttpContextAccessor();

// Configure the HTTP request pipeline.
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

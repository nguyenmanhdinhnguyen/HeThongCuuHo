
using Microsoft.EntityFrameworkCore;
using HeThongCuuHo.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DBHeThongCuuHoContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DBHeThongCuuHo")));


builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

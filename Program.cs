using Microsoft.EntityFrameworkCore;
using WebDoDungNhaBep.Models;

namespace WebDoDungNhaBep
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ===================== KẾT NỐI CSDL =====================
            var connectionString = builder.Configuration.GetConnectionString("Employee");
            builder.Services.AddDbContext<ShopDoDungNhaBep02Context>(options =>
                options.UseSqlServer(connectionString));

            // ===================== CẤU HÌNH SESSION =====================
            builder.Services.AddDistributedMemoryCache(); // Bộ nhớ tạm để lưu session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian sống của session
                options.Cookie.HttpOnly = true; // Bảo mật cookie session
                options.Cookie.IsEssential = true; // Bắt buộc có cookie này
            });

            // ===================== CẤU HÌNH MVC =====================
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ===================== XỬ LÝ NGOẠI LỆ =====================
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // ===================== MIDDLEWARE PIPELINE =====================
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // ⚠️ PHẢI ĐẶT TRƯỚC Authorization
            app.UseSession(); // Kích hoạt Session để dùng trong controller

            app.UseAuthorization();

            // ===================== ĐỊNH TUYẾN (ROUTING) =====================
            // Route cho khu vực Admin
            app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // Route mặc định cho site
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // ===================== CHẠY ỨNG DỤNG =====================
            app.Run();
        }
    }
}

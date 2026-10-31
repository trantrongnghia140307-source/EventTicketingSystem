var builder = WebApplication.CreateBuilder(args);

// Đăng ký dịch vụ Controllers cho Web API
builder.Services.AddControllers();

var app = builder.Build();

// Cấu hình đọc file index.html và tài nguyên tĩnh trong wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

// Áp dụng định tuyến Route cho các Controller API
app.MapControllers();

// Khởi chạy ứng dụng Web
app.Run();
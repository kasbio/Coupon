using System.Text.Json;
using System.Text.Unicode;
using Coupon.Redis;
using Coupon.Template.ApplicationCore;
using Coupon.Template.ApplicationCore.Mapper;
using Coupon.Template.DbContexts;
using Coupon.Template.Grpc;
using Coupon.Template.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
// Add services to the container.




var services = builder.Services;
services.AddTemplateService();

ConfigureGrpc();//Grpc配置
ConfigureData();

var app = builder.Build();
var env = app.Environment;
// Configure the HTTP request pipeline.
app.MapGrpcService<CouponTemplateService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();


void ConfigureData()
{
    var conn = config.GetConnectionString("DB");
    services.AddDbContext<TemplateDbContext>(o => o.UseMySql(conn, ServerVersion.Parse(conn),
        opt => opt.MigrationsAssembly("Coupon.Template.Model")));
 
    services.AddRedis(config);
    services.AddAutoMapper(typeof(CouponTemplateMapper).Assembly);

}

void ConfigureGrpc()
{
    services.AddGrpc();
    services.AddGrpcReflection();
}
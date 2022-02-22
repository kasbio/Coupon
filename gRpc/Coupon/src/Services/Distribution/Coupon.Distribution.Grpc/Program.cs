

using System.Security.Principal;
using Coupon.Common;
using Coupon.Distribution.ApplicationCore.Extension;
using Coupon.Distribution.ApplicationCore.Mapper;
using Coupon.Distribution.ApplicationCore.Services;
using Coupon.Distribution.ApplicationCore.Services.Implement;
using Coupon.Distribution.DbContexts;
using Coupon.Distribution.Grpc.Services;
using Coupon.Redis;
using Coupon.Settlement.Grpc;
using Coupon.Template.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var config = builder.Configuration;
var services = builder.Services;

AddGrpc();
ConfigureData();
services.AddScoped<IUserService, UserService>();



services.AddAutoMapper(typeof(UserController).Assembly);
var app = builder.Build();
if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}
// Configure the HTTP request pipeline.
app.MapGrpcService<UserController>();
app.MapGet("/", 
    () => @"Communication with gRPC endpoints must be made through a gRPC client. 
            To learn how to create a client, 
            visit: https://go.microsoft.com/fwlink/?linkid=2086909");


app.Run();


void AddGrpc()
{
    builder.Services.AddGrpc();
    services.AddDistributionService(config);
    services.AddGrpcReflection();
}

void ConfigureData()
{
    var conn = config.GetConnectionString("DB");
    services.AddDbContext<CouponDbContext>(o => o.UseMySql(conn, ServerVersion.Parse(conn),
        opt => opt.MigrationsAssembly("Coupon.Distribution.Model")));
 
    services.AddRedis(config);
    services.AddAutoMapper(typeof(CouponMapper).Assembly);
  

}
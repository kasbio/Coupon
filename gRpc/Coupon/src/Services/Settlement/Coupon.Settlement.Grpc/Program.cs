

using Coupon.Settlement;
using Coupon.Settlement.ApplicationCore;
using Coupon.Settlement.Grpc.Controllers;
using Coupon.Template.Grpc;


var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var config = builder.Configuration;
var services = builder.Services;
services.AddGrpcReflection();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
services.AddGrpc();
services.AddScoped<RuleService>();
services.AddSettlementService(config);
var app = builder.Build();
if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}
// Configure the HTTP request pipeline.
app.MapGrpcService<SettlementController>();
app.MapGet("/", () => @"Communication with gRPC endpoints must be made through a gRPC client. 
                                    To learn how to create a client, 
                                    visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

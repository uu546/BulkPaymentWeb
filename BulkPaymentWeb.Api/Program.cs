using BulkPaymentWeb.Application;
using BulkPaymentWeb.Infrastructure;
using BulkPaymentWeb.Infrastructure.Data.Hubs;
using Hangfire;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplicationInit()
    .AddInfrastructureInit(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiCorsPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.AddSignalR();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(opt =>
    {
        opt.Title = "Документация API";
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    });
}

app.UseCors("ApiCorsPolicy");

app.MapControllers();

app.UseHangfireDashboard();
app.MapHub<PaymentHub>("/hubs/payments");

app.Run();

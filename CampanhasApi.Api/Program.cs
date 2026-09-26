using CampanhasApi.Api.Extensions;
using CampanhasApi.Api.Middlewares;
using CampanhasApi.Infrastructure.Data;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDatabase(builder.Configuration)
                .AddApplicationServices(builder.Configuration)
                .AddApiDocumentation()
                .AddJWTConfig(builder.Configuration)
                .AddApiCors()
                .AddMessaging(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CampanhasDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>()
    .UseMiddleware<ExceptionMiddleware>()
    .UseMiddleware<RequestLoggingMiddleware>()
    .UseCors("DefaultCors")
    .UseHttpsRedirection()
    .UseHttpMetrics()
    .UseAuthentication()
    .UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();
app.MapMetrics();

app.Run();

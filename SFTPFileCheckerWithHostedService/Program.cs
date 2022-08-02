using Microsoft.EntityFrameworkCore;
using SFTPFileCheckerWithHostedService;
using SFTPFileCheckerWithHostedService.Data;
using SFTPFileCheckerWithHostedService.Services;

var builder = WebApplication.CreateBuilder(args);
static IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureWebHostDefaults(webBuilder =>
         {
             webBuilder.UseUrls("http://0.0.0.0:3000", "https://0.0.0.0:3000");
         });
// Add services to the container.
//builder.Services.AddHostedService<ScheduledHostedService>();
builder.Services.AddDbContext<AppsDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("PostgreConnection")));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddHostedService<ConsumeServiceHostedService>();
builder.Services.AddScoped<IProcessingService, ProcessingService>();
builder.Services.AddScoped<ISFTPService, SFTPService>();
builder.Services.AddScoped<IFileHistoryService, FileHistoryService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<AppsDbContext>();
    dataContext.Database.Migrate();
    dataContext.Database.EnsureCreated();
}
var devCorsPolicy = "devCorsPolicy";
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(policy =>
{
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});
if (app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
    app.Run("http://0.0.0.0:3000");
}

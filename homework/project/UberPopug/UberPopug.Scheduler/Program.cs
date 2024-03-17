using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

GlobalConfiguration.Configuration.UseSqlServerStorage(builder.Configuration["Hangfire:ConnectionString"]);
app.UseHangfireDashboard();
app.UseHangfireServer();

RecurringJob.AddOrUpdate(19872.ToString(),() => Console.WriteLine("Background job"), Cron.Daily(0, 0));

app.Run();
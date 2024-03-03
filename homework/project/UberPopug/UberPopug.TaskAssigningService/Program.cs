using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddTransient<IExampleTransientService, ExampleTransientService>();

Console.WriteLine("Hello, World!");

Console.ReadKey();
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddTransient<IExampleTransientService, ExampleTransientService>();

Console.WriteLine("App is running");

Console.ReadKey();
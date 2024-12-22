using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieQueue.Services;
using StackExchange.Redis;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.ConfigureFunctionsWebApplication();

        // Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
        // builder.Services
        //     .AddApplicationInsightsTelemetryWorkerService()
        //     .ConfigureFunctionsApplicationInsights();
        var muxer = ConnectionMultiplexer.Connect(
         new ConfigurationOptions
         {
             EndPoints = { { "redis-14728.c326.us-east-1-3.ec2.redns.redis-cloud.com", 14728 } },
             User = "default",
             Password = "2cncW8bIijfJh68IgKu6Pm29jGvlnGck"
         });
        //redis - cli - u redis://default:2cncW8bIijfJh68IgKu6Pm29jGvlnGck@redis-14728.c326.us-east-1-3.ec2.redns.redis-cloud.com:14728
        builder.Services.AddSingleton<IConnectionMultiplexer>(muxer);
        builder.Services.AddTransient<RedisHelper>();


        var azureStorageConnectionString = builder.Configuration.GetValue<string>("AzureWebJobsStorage");
        var queueName = builder.Configuration.GetValue<string>("QueueName");


        builder.Services.AddSingleton<IQueueService>(sp =>
        {
            return new QueueService(azureStorageConnectionString, queueName);
        });


        builder.Build().Run();
    }
}
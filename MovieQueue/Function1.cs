using System;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MovieQueue.Models;
using MovieQueue.Services;

namespace MovieQueue
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly RedisHelper redisHelper;
        private static readonly HttpClient client = new HttpClient();

        public Function1(ILogger<Function1> logger, RedisHelper redisHelper)
        {
            _logger = logger;
            this.redisHelper = redisHelper;
        }

        //private readonly IQueueService queueService;
        //public Function1(ILogger<Function1> logger, IQueueService queueService, RedisHelper redisHelper)
        //{
        //    _logger = logger;
        //    this.queueService = queueService;
        //    this.redisHelper = redisHelper;
        //}

        [Function(nameof(Function1))]
        public async Task Run([QueueTrigger("mynewqueue", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            string apiKey = Environment.GetEnvironmentVariable("OMDB_API_KEY");
            HttpResponseMessage response = await client.GetAsync($"http://www.omdbapi.com/?apikey={apiKey}&t={message.MessageText}");
            //response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var movieInfo = JsonSerializer.Deserialize<MovieInfo>(jsonResponse);
                if (movieInfo.Title != null)
                {

                    redisHelper.SaveMovieToRedis("movies", movieInfo.Title, movieInfo.Poster);
                }

            }


            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

        }
    }
}

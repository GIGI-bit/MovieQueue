using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieQueue.Services
{
    public class RedisHelper
    {
        private readonly IDatabase _database;

        public RedisHelper(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }
        
        public void SaveMovieToRedis(string listKey, string movieName, string posterUrl)
        {
            var movieData = new
            {
                Name = movieName,
                PosterUrl = posterUrl
            };
            string serializedMovie = JsonConvert.SerializeObject(movieData);
            _database.ListLeftPush(listKey, serializedMovie);
        }
    }
}

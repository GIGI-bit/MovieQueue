//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//namespace MovieQueue.Services
//{
//    public class MovieService
//    {
//        private readonly RedisHelper _redisHelper;
//        private const string RedisListKey = "movies"; // The list key in Redis

//        public MovieService()
//        {
//            _redisHelper = new RedisHelper();
//        }

//        public void SaveMovie(string movieName, string posterUrl)
//        {
//            _redisHelper.SaveMovieToRedis(RedisListKey, movieName, posterUrl);
//        }

//    }
//}

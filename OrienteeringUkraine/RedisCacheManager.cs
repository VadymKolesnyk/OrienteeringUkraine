using Microsoft.AspNetCore.Mvc.Rendering;
using OrienteeringUkraine.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrienteeringUkraine
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly IConnectionMultiplexer redis;
        private readonly TimeSpan lifeTime = TimeSpan.FromHours(1);

        public RedisCacheManager(IConnectionMultiplexer redis)
        {
            this.redis = redis;
            redis.GetDatabase().KeyDelete(new RedisKey[] { "clubs", "regions" });
        }

        public void SetRegions(IEnumerable<Region> list)
        {
            redis.GetDatabase().StringSet("regions", JsonSerializer.Serialize(list), lifeTime);
        }

        public IEnumerable<Region> GetRegions()
        {
            string regions = redis.GetDatabase().StringGet("regions");
            if (regions == null)
                return null;
            return JsonSerializer.Deserialize<IEnumerable<Region>>(regions);
        }

        public void SetClubs(IEnumerable<Club> list)
        {
            redis.GetDatabase().StringSet("clubs", JsonSerializer.Serialize(list), lifeTime);
        }

        public IEnumerable<Club> GetClubs()
        {
            string clubs = redis.GetDatabase().StringGet("clubs");
            if (clubs == null)
                return null;
            return JsonSerializer.Deserialize<IEnumerable<Club>>(clubs);
        }
    }
}

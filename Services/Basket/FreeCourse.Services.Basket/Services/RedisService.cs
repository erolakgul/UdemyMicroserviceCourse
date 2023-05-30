using StackExchange.Redis;

namespace FreeCourse.Services.Basket.Services
{
    public class RedisService
    {
        private ConnectionMultiplexer _connectionMultiplexer;

        private readonly string? _host;
        private readonly int _port;

        public RedisService(string host,int port)
        {
            _port = port;
            _host = host;
        }

        public void Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");
        //redis in default 16 tane db si var, biz 5.ciyi bu amaç için kullanmak istediğimizi belirtiyoruz
        public IDatabase GetDatabase(int db = 5) => _connectionMultiplexer.GetDatabase(db);

    }
}

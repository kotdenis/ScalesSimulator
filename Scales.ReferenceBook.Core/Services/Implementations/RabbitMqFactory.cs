using RabbitMQ.Client;

namespace Scales.ReferenceBook.Core.Services.Implementations
{
    public class RabbitMqFactory : IDisposable
    {
        private static IModel? _channel;
        private static IConnection? _connection;
        public static IModel CreateChannel(string hostName, int port)
        {
            var factory = new ConnectionFactory() { HostName = hostName, Port = port };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            return _channel;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                _channel!.Close();
                _connection!.Close();

            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

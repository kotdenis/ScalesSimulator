namespace Scales.Journal.Core.Options
{
    public class RabbitMqOptions
    {
        public const string RabbitMq = "RabbitMq";
        public string HostName { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}

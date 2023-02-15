namespace Scales.Journal.Core.Infrastructure
{
    public class TransportFactory
    {
        public static TransportForWeghing CreateTransport()
        {
            var random = new Random();
            return new TransportForWeghing
            {
                NumberOfAxles = random.Next(2, 5),
                Brand = DefaultTransportData.TransportBrands[random.Next(0, DefaultTransportData.TransportBrands.Count - 1)],
                Cargo = DefaultTransportData.TransportCargoes[random.Next(0, DefaultTransportData.TransportCargoes.Count - 1)],
                CarPlate = GenerateCarPlate(),
                Weight = random.Next(10000, 50000)
            };
        }

        private static string GenerateCarPlate()
        {
            string carPlate = "";
            var random = new Random();
            for (int i = 0; i < 6; i++)
            {
                var letter = DefaultTransportData.CarPlatesLetters[random.Next(0, DefaultTransportData.CarPlatesLetters.Count - 1)];
                var number = random.Next(0, 9);
                if (i < 1 || i > 3)
                    carPlate += letter;
                else if (i >= 1 && i < 4)
                    carPlate += number.ToString();
            }
            return carPlate;
        }
    }

    public class TransportForWeghing
    {
        public string Brand { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string CarPlate { get; set; } = string.Empty;
        public int NumberOfAxles { get; set; }
        public float Weight { get; set; }
        public int WeighingPeriod { get; set; } = 100;
        public List<float> WeighingList { get; set; } = new List<float>();
        public List<float> AxlesList { get; set; } = new List<float>();
    }
}

namespace Scales.Journal.Core.Infrastructure
{
    public class DefaultTransportData
    {
        private static List<string> _transportCargoes;
        private static List<string> _transportBrands;
        private static List<char> _carPlatesLetters;
        static DefaultTransportData()
        {
            _transportCargoes = GetTransportCargoes().ToList();
            _transportBrands = GetTransportBrands().ToList();
            _carPlatesLetters = GetCarPlatesLetters().ToList();
        }

        public static List<string> TransportCargoes => _transportCargoes;
        public static List<string> TransportBrands => _transportBrands;
        public static List<char> CarPlatesLetters => _carPlatesLetters;

        private static IEnumerable<string> GetTransportCargoes()
        {
            return new[]
            {
                "Electronics", "Appliances", "Cosmetic", "Fertilizers", "Furniture", "Wood boards", "Logs", "Wheat", "Crushed stone", "Coal", "Seed"
            };
        }

        private static IEnumerable<string> GetTransportBrands()
        {
            return new[]
            {
                "КамАЗ-65207", "Hyundai Mighty", "ISUZU ELF", "МАЗ-6310", "ГАЗ «Садко»", "HOWO A7", "JAC N-56", "МАЗ-5440", "MAN TGS",
                "Scania «G-Series»", "КрАЗ М16.1Х", "Volvo серии «FH»", "ISUZU GIGA 6х4", "КрАЗ-6230C40", "MAN TGS", "КамАЗ-689011"
            };
        }

        private static IEnumerable<char> GetCarPlatesLetters()
        {
            return new[] { 'A', 'B', 'C', 'K', 'M', 'H', 'O', 'P', 'T', 'X' };
        }
    }
}

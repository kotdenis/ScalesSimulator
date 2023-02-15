using Scales.Journal.Core.Constants;

namespace Scales.Journal.Core.Infrastructure
{
    public class WeighingSimulator : IWeighingSimulator
    {
        public WeighingSimulator()
        {

        }

        public TransportForWeghing GenerateTransportDataForWeighing()
        {
            var random = new Random();
            var transport = TransportFactory.CreateTransport();
            transport.WeighingPeriod = AppConstants.WEIGHING_PERIOD;
            var weightOnAxle = Math.Round(transport.Weight / transport.NumberOfAxles);
            var delimiter = weightOnAxle;
            var splittedWeight = (float)Math.Round(transport.Weight / AppConstants.WEIGHING_PERIOD);
            var level = splittedWeight;
            var splitter = 1;
            transport.WeighingList.Add(0);
            for (int i = 0; i < transport.Weight; i += (int)splittedWeight)
            {
                if (splitter == 1)
                {
                    transport.WeighingList.Add(i);
                }
                else
                {
                    var amplitude = random.Next(0, 200);
                    transport.WeighingList.Add((float)(level + amplitude));
                }

                if (i > delimiter - splittedWeight * 2)
                {
                    level = (float)delimiter;
                    delimiter = weightOnAxle + delimiter;
                    splitter++;
                }
            }
            for (int i = 0; i < transport.NumberOfAxles; i++)
            {
                if (i % 2 == 0)
                    transport.AxlesList.Add((float)(weightOnAxle - 100));
                else
                    transport.AxlesList.Add((float)(weightOnAxle + 100));
            }
            return transport;
        }
    }
}

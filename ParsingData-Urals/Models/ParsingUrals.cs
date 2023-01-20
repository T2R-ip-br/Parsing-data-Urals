using System.Security.Cryptography;

namespace ParsingData_Urals.Models
{
    public class ParsingUrals
    {
        private DateTime beginPriceMonitoringPeriod;
        private DateTime endPriceMonitoringPeriod;
        private double averageOilPrice;

        public ParsingUrals(DateTime beginPriceMonitoringPeriod, DateTime endPriceMonitoringPeriod, double averageOilPrice)
        {
            BeginPriceMonitoringPeriod = beginPriceMonitoringPeriod;
            EndPriceMonitoringPeriod = endPriceMonitoringPeriod;
            AverageOilPrice = averageOilPrice;
        }

        public DateTime BeginPriceMonitoringPeriod
        {
            get { return beginPriceMonitoringPeriod; }
            set { beginPriceMonitoringPeriod = value; }
        }

        public DateTime EndPriceMonitoringPeriod
        {
            get { return endPriceMonitoringPeriod; }
            set { endPriceMonitoringPeriod = value; }
        }

        public double AverageOilPrice
        {
            get { return averageOilPrice; }
            set { averageOilPrice = value; }
        }
    }
}

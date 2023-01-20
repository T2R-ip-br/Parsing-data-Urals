using ParsingData_Urals.Models;
using FileHelpers;
using System.Net;
using System;

namespace ParsingData_Urals.Service
{
    public class ParsingUralsService
    {
        private static List<ParsingUrals> listParsingUrals = new List<ParsingUrals>();
        private static DateTime dateDataUpdate;

        public ParsingUralsService()
        {
            // TODO: Реализовать получение файла с сайта, парсинг данных из файла и сохранение их в список listParsingUrals
            dateDataUpdate = DateTime.Now;

            // Скачиваем файл (3 попытки чтобы скачать файл)
            int count = 3;
            while (DownloadDataFile())
            {
                if (count > 0) --count;
                else
                {
                    Console.WriteLine("ОШИБКА: Сервису не удалось получить доступ к файлу");
                    break;
                }
            }

            // Парсинг данных из файла
            ParsingDataFromFile();
        }

        // Получение списка всех записей о ценах
        public List<ParsingUrals> GetListParsingUrals()
        {
            /* Если список пуст или данные устарели на сутки то загружаем данные или обновляем их */
            if (listParsingUrals.Count == 0 || (dateDataUpdate.Date < DateTime.Now.Date))
            {
                listParsingUrals.Clear();

                // Скачиваем файл (3 попытки чтобы скачать файл)
                int count = 3;
                while (DownloadDataFile())
                {
                    if (count > 0) --count;
                    else
                    {
                        Console.WriteLine("ОШИБКА: Сервису не удалось получить доступ к файлу");
                        break;
                    }
                }

                // Парсинг данных из файла
                ParsingDataFromFile();

                // Указываем дату обновления данных
                dateDataUpdate = DateTime.Now;
            }

            return listParsingUrals;
        }

        // Загрузка файла с сайта
        private bool DownloadDataFile()
        {
            try
            {
                // Получаем ссылку на файл
                String link = GetLink();

                // Выполняем попытку скачивания файла
                /* ЗАкомментировано так как нет доступа к сайту
                using (var client = new WebClient())
                {
                    client.DownloadFile(link, @"resource\data.csv");
                    return false;
                }
                */

                // (Удалить когда код выше будет работать)
                return false;
            }
            catch
            {
                Console.WriteLine("ОШИБКА: Не удалось скачать файл с данными");
                return true;
            }
        }

        private string GetLink()
        {
            string link = "";

            /* (Описываю логику работы парсера так как нет доступа к сайту)
             * Получаем страницу https://data.gov.ru/opendata/7710349494-urals 
             * Ищем нужный тег со сылкой на файл
             * Вытаскиваем ссылку из тега и сохраняем её в переменную link
             */

            return link;
        }

        // Парсинг данных из файла
        public void ParsingDataFromFile()
        {
            var fileHelperEngine = new FileHelperEngine<TempRecord>();
            var records = fileHelperEngine.ReadFile(@"D:\C#\ParsingData-Urals\ParsingData-Urals\bin\Debug\net6.0\resource\data.csv");

            foreach (var record in records)
            {
                try
                {
                    record.averageOilPrice = record.averageOilPrice.Replace("\"", "");
                    record.beginPriceMonitoringPeriod = ConvertingMonthToNumber(record.beginPriceMonitoringPeriod.Replace("\"", ""));
                    record.endPriceMonitoringPeriod   = ConvertingMonthToNumber(record.endPriceMonitoringPeriod.Replace("\"", ""));
                    listParsingUrals.Add( 
                        new ParsingUrals(
                            DateTime.ParseExact(record.beginPriceMonitoringPeriod, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            DateTime.ParseExact(record.endPriceMonitoringPeriod,   "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            double.Parse(record.averageOilPrice)));
                } 
                catch
                {
                    Console.WriteLine("ОШИБКА: Некорректные данные в файле: [" + record.beginPriceMonitoringPeriod + "], [" + record.endPriceMonitoringPeriod + "], [" + record.averageOilPrice + "].");
                    continue;
                }
            }
        }

        // преобразование строкового формата месяца в число
        private static string ConvertingMonthToNumber(string date)
        {
            string month = date.Substring(date.IndexOf("."), 5);

            if (month == ".янв.")
                date = date.Replace(".янв.", "-01-20"); // -20 добавляем заодно год, для полного формата dd-MM-yyyy

            else if (month == ".фев.")
                date = date.Replace(".фев.", "-02-20");

            else if (month == ".мар.")
                date = date.Replace(".мар.", "-03-20");

            else if (month == ".апр.")
                date = date.Replace(".апр.", "-04-20");

            else if (month == ".май.")
                date = date.Replace(".май.", "-05-20");

            else if (month == ".июн.")
                date = date.Replace(".июн.", "-06-20");

            else if (month == ".июл.")
                date = date.Replace(".июл.", "-07-20");

            else if (month == ".авг.")
                date = date.Replace(".авг.", "-08-20");

            else if (month == ".сен.")
                date = date.Replace(".сен.", "-09-20");

            else if (month == ".окт.")
                date = date.Replace(".окт.", "-10-20");

            else if (month == ".ноя.")
                date = date.Replace(".ноя.", "-11-20");

            else if (month == ".дек.")
                date = date.Replace(".дек.", "-12-20");

            return date;
        }

        // Получение цены по конкретной дате
        internal static double GetPriceByDate(DateTime date)
        {
            double price = -1;

            foreach (ParsingUrals e in listParsingUrals) {
                
                if (e.BeginPriceMonitoringPeriod.Date <= date.Date && date.Date <= e.EndPriceMonitoringPeriod.Date)
                {
                    price = e.AverageOilPrice; 
                    break;
                }
            }

            return price;
        }

        // Получение средней цены за период
        internal static object GetAveragePriceByPeriodDate(DateTime beginDate, DateTime endDate)
        {
            double price = 0;
            int count = 0;
            double temp;

            // TODO: Реализовать поиск средней цены за промежуток времени
            while (beginDate.Date != endDate.Date.AddDays(1))
            {
                temp = GetPriceByDate(beginDate);
                if (temp == -1) return -1;

                price += temp;
                ++count;
                beginDate = beginDate.Date.AddDays(1);
            }

            return price / count;
        }

        // Получение минимальной и максимальной цены за период
        internal static MinMaxPrice GetMinMaxPriceInJson(DateTime beginDate, DateTime endDate)
        {
            double minPrice = double.MaxValue;
            double maxPrice = double.MinValue;
            bool inPeriod = false;

            if (beginDate.Date > endDate.Date)
            {
                DateTime temp = beginDate;
                beginDate = endDate;
                endDate = temp;
            }

            // TODO: реализовать нахождение минимальной и максимальной цены за период
            foreach (ParsingUrals e in listParsingUrals)
            {
                if (!inPeriod && e.BeginPriceMonitoringPeriod.Date <= beginDate.Date && beginDate.Date <= e.EndPriceMonitoringPeriod.Date)
                    inPeriod = true;

                if (inPeriod && e.EndPriceMonitoringPeriod.Date >= endDate.Date)
                    break;

                if (inPeriod)
                {
                    if (minPrice > e.AverageOilPrice)
                        minPrice = e.AverageOilPrice;

                    if (maxPrice < e.AverageOilPrice)
                        maxPrice = e.AverageOilPrice;
                }

            }

            if (!inPeriod)
            {
                minPrice = -1;
                maxPrice = -1;
            }

            return new MinMaxPrice(minPrice, maxPrice);
        }
    }

    // Класс хранения минимальной и максимальной цены
    class MinMaxPrice
    {
        private double minPrice;
        private double maxPrice;

        public MinMaxPrice(double minPrice, double maxPrice)
        {
            this.minPrice = minPrice;
            this.maxPrice = maxPrice;
        }

        public double MinPrice
        {
            get { return minPrice; }
        }

        public double MaxPrice
        {
            get { return maxPrice; }
        }
    }

    [DelimitedRecord(",\"")]
    public class TempRecord
    {
        public string beginPriceMonitoringPeriod { get; set; }
        public string endPriceMonitoringPeriod { get; set; }
        public string averageOilPrice { get; set; }
    }
}

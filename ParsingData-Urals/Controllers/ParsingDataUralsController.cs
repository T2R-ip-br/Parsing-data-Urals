using Microsoft.AspNetCore.Mvc;
using ParsingData_Urals.Models;
using ParsingData_Urals.Service;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ParsingData_Urals.Controllers
{
    [Route("api/parsing-urals")]
    [ApiController]
    public class ParsingDataUralsController : ControllerBase
    {
        private readonly ParsingUralsService parsingUralsService = new ParsingUralsService();

        // GET: api/parsing-urals/records
        // Получить список всех записей с ценами на нефть
        [HttpGet("records")]
        public string GetAllRecords()
        {
            return JsonSerializer.Serialize(parsingUralsService.GetListParsingUrals());
        }

        // GET: api/parsing-urals/price?date="{date}"  /// {date} format dd-MM-yyyy 
        // Получение цены на нефть на указанную дату
        [HttpGet("price")]
        public string GetPrice([FromQuery(Name = "date")] string sDate)
        {
            try
            {
                DateTime date = DateTime.ParseExact(sDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                return ParsingUralsService.GetPriceByDate(date).ToString();

            } catch {
                return "Date format: dd-MM-yyyy";
            }
        }

        // GET: api/parsing-urals/average-price?begin-date="{date}"&end-date="{date}"  /// {date} format dd-MM-yyyy 
        // Получение средней цены за период
        [HttpGet("average-price")]
        public string GetAveragePrice([FromQuery(Name = "begin-date")] string sBeginDate, [FromQuery(Name = "end-date")] string sEndDate)
        {
            try
            {
                DateTime beginDate = DateTime.ParseExact(sBeginDate.Replace("\"", ""), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(sEndDate.Replace("\"", ""), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                return ParsingUralsService.GetAveragePriceByPeriodDate(beginDate, endDate).ToString();
            }
            catch
            {
                return "Date format: dd-MM-yyyy";
            }
        }

        // GET: api/parsing-urals/min-max-price?begin-date=Э{date}"&end-date="{date}"  /// {date} format dd-MM-yyyy 
        // Получение максимальной и минимальной цены за период
        [HttpGet("min-max-price")]
        public string GetMinMaxPrice([FromQuery(Name = "begin-date")] string sBeginDate, [FromQuery(Name = "end-date")] string sEndDate)
        {
            try
            {
                DateTime beginDate = DateTime.ParseExact(sBeginDate.Replace("\"", ""), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(sEndDate.Replace("\"", ""), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                return JsonSerializer.Serialize(ParsingUralsService.GetMinMaxPriceInJson(beginDate, endDate));

            }
            catch
            {
                return "Date format: dd-MM-yyyy";
            }
        }
    }
}

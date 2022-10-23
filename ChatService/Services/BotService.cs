using ChatService.Model;
using System.Globalization;
using System.Net;

namespace ChatService.Services
{
    public class BotService
    {
        private readonly HttpClient _client;

        public BotService()
        {
            _client = new HttpClient();
        }

        public async Task<Stock> GetSotck(string symbol)
        {

            using (HttpResponseMessage response = _client.GetAsync($"https://stooq.com/q/l/?s={symbol}&f=sd2t2ohlcv&h&e=csv,").Result)
            using (HttpContent content = response.Content)
            {
                var stockResponse = content.ReadAsStringAsync().Result;
                if (response.StatusCode != HttpStatusCode.OK)
                    throw null;

                var data = stockResponse.Substring(stockResponse.IndexOf(Environment.NewLine, StringComparison.Ordinal) + 2);
                var processedArray = data.Split(',');
                var culture = new CultureInfo("en-US");
                return new Stock()
                {
                    Symbol = processedArray[0],
                    Date = !processedArray[1].Contains("N/D") ? Convert.ToDateTime(processedArray[1]) : default,
                    Time = !processedArray[2].Contains("N/D") ? Convert.ToDateTime(processedArray[2]) : default,
                    Open = !processedArray[3].Contains("N/D") ? Convert.ToDouble(processedArray[3], culture) : default,
                    High = !processedArray[4].Contains("N/D") ? Convert.ToDouble(processedArray[4], culture) : default,
                    Low = !processedArray[5].Contains("N/D") ? Convert.ToDouble(processedArray[5], culture) : default,
                    Close = !processedArray[6].Contains("N/D") ? Convert.ToDouble(processedArray[6], culture) : default,
                    Volume = !processedArray[7].Contains("N/D") ? Convert.ToDouble(processedArray[7]) : default,
                };
            }

            return null;

        }

    }
}

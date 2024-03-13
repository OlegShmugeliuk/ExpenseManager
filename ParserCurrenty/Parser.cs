using AngleSharp.Dom;
using AngleSharp.Text;
using HtmlAgilityPack;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ParserCurrenty
{
    public class Parser
    {



        private async Task<string> GetHtmlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync(url).ConfigureAwait(false);
            }
        }

        public async Task<Dictionary<string, double>> ParseForUAH()
        {
            string url = "https://minfin.com.ua/ua/currency/";
            string html = await GetHtmlAsync(url);
            int i = 0;
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            List<double> values = new List<double>();

            var val = document.DocumentNode.SelectNodes("//div[@class='sc-1x32wa2-9 bKmKjX']").Take(9);
            if (val != null)
            {

                foreach (var _value in val)
                {
                    string innerText = _value.InnerText.Trim();
                    Match match = Regex.Match(innerText, @"(\d+,\d+)");
                    if (match.Success)
                    {
                        string parsedValue = match.Groups[1].Value;

                        if (double.TryParse(parsedValue.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
                        {
                            if (i == 0 || i == 3 || i == 6)
                            {
                                values.Add(res);

                            }
                        }
                    }
                    i++;

                }
            }


            int j = 0;
            var key = document.DocumentNode.SelectNodes("//a[@class='sc-1x32wa2-7 ciClTw']").Take(3);
            Dictionary<string, double> result = new Dictionary<string, double>();
            foreach (var _key in key)
            {

                result[_key.InnerText.Trim()] = values[j];
                j++;
            }

            return result;
        }


        public async Task<Dictionary<string, double>> ParseForEUR()
        {
            string url = "https://www.iban.com/exchange-rates";
            string html = await GetHtmlAsync(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            var strongNode = document.DocumentNode.SelectNodes("//td/strong");
            Dictionary<string, double> res = new Dictionary<string, double>();
            int i = 0, c = 0;
            foreach (var item in strongNode)
            {
                if (i == 0)
                {
                    string innerText = item.InnerText.Trim();
                    Match match = Regex.Match(innerText, @"(\d+.\d+)");
                    if (match.Success)
                    {
                        string parsedValue = match.Groups[1].Value;

                        if (double.TryParse(parsedValue.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                        {
                            res.Add("USD", value);
                        }

                    }
                }
                else if (i == 7)
                {
                    string innerText = item.InnerText.Trim();
                    Match match = Regex.Match(innerText, @"(\d+.\d+)");
                    if (match.Success)
                    {
                        string parsedValue = match.Groups[1].Value;

                        if (double.TryParse(parsedValue.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                        {
                            res.Add("PLN", value);
                        }
                    }

                }
                i++;

            }
            return res;
        }


        public async Task<Dictionary<string, double>> ParseForUSD()
        {
            // Replace the path with the correct path to your JSON file
            string jsonFilePath = "E:\\Project ASP.NET\\ExpenseManager\\ParserCurrenty\\ExchangeRate.json";

            try
            {
                using (FileStream fs = File.OpenRead(jsonFilePath))
                {
                    JsonDocument doc = await JsonDocument.ParseAsync(fs);
                    var exchangeUsd = doc.RootElement.GetProperty("Exchange USD");

                    Dictionary<string, double> exchangeRates = new Dictionary<string, double>
                {
                    { "UAH", exchangeUsd.GetProperty("UAH").GetDouble() },
                    { "EUR", exchangeUsd.GetProperty("EUR").GetDouble() },
                    { "PLN", exchangeUsd.GetProperty("PLN").GetDouble() }
                };

                    foreach (var exchangeRate in exchangeRates)
                    {
                        await Console.Out.WriteLineAsync($"{exchangeRate.Key}: {exchangeRate.Value}");
                    }

                    return exchangeRates;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<Dictionary<string, double>> ParseForPLN()
        {
            string jsonFilePath = @"E:\Project ASP.NET\ExpenseManager\ParserCurrenty\ExchangeRate.json";
            using (FileStream fs = File.OpenRead(jsonFilePath))
            {
                JsonDocument doc = await JsonDocument.ParseAsync(fs);
                var exchangePLN = doc.RootElement.GetProperty("Exchange PLN");
                Dictionary<string, double> result = new Dictionary<string, double>{
                    {"UAH", exchangePLN.GetProperty("UAH").GetDouble() },
                    {"EUR", exchangePLN.GetProperty("EUR").GetDouble() },
                    {"USD", exchangePLN.GetProperty("USD").GetDouble() }
                };
                foreach (var exchangeRate in result)
                {
                    await Console.Out.WriteLineAsync($"{exchangeRate.Key}: {exchangeRate.Value}");
                }

                return result;
            }


        }
    }
}

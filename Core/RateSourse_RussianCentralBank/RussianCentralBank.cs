using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Core.RateSourse_RussianCentralBank
{
    public class CurrencyRate_CB
    {
        public string ID { get; set; }
        public string NumCode { get; set; }
        public string CharCode { get; set; }
        public double Nominal { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Previous { get; set; }
    }

    public class Rate_FromCB
    {
        public string Date { get; set; }
        public string PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public string Timestamp { get; set; }
        public Valutes Valute { get; set; }
    }

    public class Valutes
    {
        public CurrencyRate_CB AUD { get; set; }
        public CurrencyRate_CB AZN { get; set; }
        public CurrencyRate_CB GBP { get; set; }
        public CurrencyRate_CB AMD { get; set; }
        public CurrencyRate_CB BYN { get; set; }
        public CurrencyRate_CB BGN { get; set; }
        public CurrencyRate_CB BRL { get; set; }
        public CurrencyRate_CB HUF { get; set; }
        public CurrencyRate_CB HKD { get; set; }
        public CurrencyRate_CB DKK { get; set; }
        public CurrencyRate_CB USD { get; set; }
        public CurrencyRate_CB EUR { get; set; }
        public CurrencyRate_CB INR { get; set; }
        public CurrencyRate_CB KZT { get; set; }
        public CurrencyRate_CB CAD { get; set; }
        public CurrencyRate_CB KGS { get; set; }
        public CurrencyRate_CB CNY { get; set; }
        public CurrencyRate_CB MDL { get; set; }
        public CurrencyRate_CB NOK { get; set; }
        public CurrencyRate_CB PLN { get; set; }
        public CurrencyRate_CB RON { get; set; }
        public CurrencyRate_CB XDR { get; set; }
        public CurrencyRate_CB SGD { get; set; }
        public CurrencyRate_CB TJS { get; set; }
        public CurrencyRate_CB TRY { get; set; }
        public CurrencyRate_CB TMT { get; set; }
        public CurrencyRate_CB UZS { get; set; }
        public CurrencyRate_CB UAH { get; set; }
        public CurrencyRate_CB CZK { get; set; }
        public CurrencyRate_CB SEK { get; set; }
        public CurrencyRate_CB CHF { get; set; }
        public CurrencyRate_CB ZAR { get; set; }
        public CurrencyRate_CB KRW { get; set; }
        public CurrencyRate_CB JPY { get; set; }
    }

    public class RussianCentralBank : IRateSource
    {
        public string UpdateDate { get { return RateUpdateDate; } }

        public string SourceName { get { return "ЦБ РФ"; } }

        public string SourceAddress { get { return SourceAddress_CB; } }

        private const string SourceAddress_CB = "https://www.cbr-xml-daily.ru/daily_json.js";

        private string RateUpdateDate = "";

        private Rate_FromCB Data_CB;

        private bool IsInit = false;
        
        public void Init()
        {
            try
            {
                Task<Rate_FromCB> GetRate = ReadData(SourceAddress_CB);

                Data_CB = GetRate.Result;

                string[] Date = Data_CB.Date.Split('T');

                RateUpdateDate = Date[0];

                IsInit = true;
            }
            
            catch(Exception error)
            {
                throw new Exception("Ошибка инициализации у " + SourceName + ".\n\n" + error.Message);
            }
        }

        private async Task<Rate_FromCB> ReadData(string SourceAddress)
        {
            HttpClient client = new HttpClient();

            List<Task> Tasks = new List<Task>();

            Task<HttpResponseMessage> HttpRequest = client.GetAsync(SourceAddress);
            Tasks.Add(HttpRequest);

            HttpResponseMessage Response = HttpRequest.Result;
            Response.EnsureSuccessStatusCode();

            Task<string> DecodedResponse = Response.Content.ReadAsStringAsync();
            Tasks.Add(DecodedResponse);

            await Task.WhenAll(Tasks);

            return JsonSerializer.Deserialize<Rate_FromCB>(DecodedResponse.Result);
        }

        public Rate GetRate(TypeOfCurrency type)
        {
            try
            {
                if (IsInit == false)
                {
                    throw new Exception("Источник " + SourceName + " не инициализирован.");
                }

                Rate CurrencyRate = new Rate();

                CurrencyRate.Currency = type;

                switch (type)
                {
                    case TypeOfCurrency.Ruble:
                        CurrencyRate.Value = 1;
                        break;

                    case TypeOfCurrency.Dollar:
                        CurrencyRate.Value = Data_CB.Valute.USD.Value;
                        break;

                    case TypeOfCurrency.Euro:
                        CurrencyRate.Value = Data_CB.Valute.EUR.Value;
                        break;

                    default:
                        throw new Exception("Конвертирование валюты не обработано.");
                }

                return CurrencyRate;
            }

            catch (Exception error)
            {
                throw new Exception("Ошибка чтения курса у " + SourceName + ".\n\n" + error.Message);
            }
        }        
    }
}

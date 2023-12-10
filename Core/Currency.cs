using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public static class Currency
    {
        public static TypeOfCurrency GetType(string Type)
        {
            switch (Type)
            {
                case "Рубль":
                    return TypeOfCurrency.Ruble;

                case "Ruble":
                    return TypeOfCurrency.Ruble;

                case "Доллар":
                    return TypeOfCurrency.Dollar;

                case "Dollar":
                    return TypeOfCurrency.Dollar;

                case "Евро":
                    return TypeOfCurrency.Euro;

                case "Euro":
                    return TypeOfCurrency.Euro;

                default:
                    return TypeOfCurrency.NotDefined;
            }
        }

        public static string GetName(TypeOfCurrency Type)
        {
            switch (Type)
            {
                case TypeOfCurrency.Ruble:
                    return "Рубль";

                case TypeOfCurrency.Dollar:
                    return "Доллар";

                case TypeOfCurrency.Euro:
                    return "Евро";

                default:
                    throw new Exception("Неизвестный тип валюты.");
            }
        }

        public static string GetShortName(TypeOfCurrency Type)
        {
            switch (Type)
            {
                case TypeOfCurrency.Ruble:
                    return "руб.";

                case TypeOfCurrency.Dollar:
                    return "$";

                case TypeOfCurrency.Euro:
                    return "E";

                default:
                    throw new Exception("Неизвестный тип валюты.");
            }
        }
    }
}

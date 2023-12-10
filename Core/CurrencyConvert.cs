using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public static class CurrencyConvert
    {
        public static double ToRuble(double Value, TypeOfCurrency Currency, IRateSource Source)
        {
            return Math.Round(ConvertToRuble(Value, Currency, Source), 2);
        }

        public static double ToDollar(double Value, TypeOfCurrency Currency, IRateSource Source)
        {
            double IntermediateCurrency = ConvertToRuble(Value, Currency, Source);

            return Math.Round(IntermediateCurrency / Source.GetRate(TypeOfCurrency.Dollar).Value, 2);
        }

        public static double ToEuro(double Value, TypeOfCurrency Currency, IRateSource Source)
        {
            double IntermediateCurrency = ConvertToRuble(Value, Currency, Source);

            return Math.Round(IntermediateCurrency / Source.GetRate(TypeOfCurrency.Euro).Value, 2);
        }

        private static double ConvertToRuble(double Value, TypeOfCurrency Currency, IRateSource Source)
        {
            if (Currency == TypeOfCurrency.Ruble)
            {
                return Value;
            }

            else
            {
                return Value * Source.GetRate(Currency).Value;
            }
        }
    }
}

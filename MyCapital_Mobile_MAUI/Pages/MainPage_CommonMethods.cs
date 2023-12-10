using Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCapital_Mobile_MAUI
{
    public partial class MainPage : ContentPage
    {
        private void SaveData()
        {
            List<CapitalValue> DataForSave = new List<CapitalValue>();

            foreach (PartOfCapital element in Parts)
            {
                DataForSave.Add(element.ValueForSave);
            }

            SaveFile.Save(DataForSave, SaveFileName);
        }

        private double AmountOfMoney_ConvertIn(TypeOfCurrency Type)
        {
            switch (Type)
            {
                case TypeOfCurrency.Ruble:
                    return Math.Round(AmountOfMoney_rub, 2);

                case TypeOfCurrency.Dollar:
                    return CurrencyConvert.ToDollar(AmountOfMoney_rub, TypeOfCurrency.Ruble, Data_CB);

                case TypeOfCurrency.Euro:
                    return CurrencyConvert.ToEuro(AmountOfMoney_rub, TypeOfCurrency.Ruble, Data_CB);

                default:
                    throw new Exception("Неизвестная итоговая валюта.");
            }
        }

        private static string MakeSpaceInNumber(string EnteredText)
        {
            string[] SplitText = EnteredText.Split('.');

            char[] FieldNumber_Original = SplitText[0].ToCharArray();

            List<char> FieldNumber_Modified = new List<char>();

            Array.Reverse(FieldNumber_Original);

            for (int i = 0; i < FieldNumber_Original.Length; i++)
            {
                FieldNumber_Modified.Add(FieldNumber_Original[i]);

                if ((i != FieldNumber_Original.Length - 1) && (i + 1) % 3 == 0)
                {
                    FieldNumber_Modified.Add(' ');
                }
            }

            char[] OutArray = FieldNumber_Modified.ToArray();

            Array.Reverse(OutArray);

            string FieldNumber = new string(OutArray);

            if (SplitText.Length > 1)
            {
                FieldNumber += "." + SplitText[1];
            }

            return FieldNumber;
        }      

        private void CalculateResult()
        {
            try
            {
                if (Parts.Count != 0)
                {
                    AmountOfMoney_rub = 0;

                    foreach (PartOfCapital element in Parts)
                    {
                        if (element.SelectedCurrency != TypeOfCurrency.NotDefined && element.Visibility == true)
                        {
                            AmountOfMoney_rub += element.Value * Data_CB.GetRate(element.SelectedCurrency).Value;
                        }
                    }

                    Button_AmountOfMoney.Text = MakeSpaceInNumber(AmountOfMoney_ConvertIn(ResultCurrency).ToString(CultureInfo.InvariantCulture)) +
                        " " + Currency.GetShortName(ResultCurrency);
                }

                else
                {
                    Button_AmountOfMoney.Text = "0 " + Currency.GetShortName(ResultCurrency);
                }
            }

            catch (Exception error)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка",
                    "Ошибка подсчета результата:\n\n" + error.Message, "ОK");
            }
        }
    }
}

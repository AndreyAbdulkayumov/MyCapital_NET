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
        private PartOfCapital GetPart(string ID)
        {
            PartOfCapital SelectedPart = Parts.Single(Part => Part.ID == Convert.ToInt32(ID));

            if (SelectedPart == null)
            {
                throw new Exception("Не удалось найти часть капитала с ID = " + ID);
            }

            return SelectedPart;
        }

        /*
         * 
         * Entry_FieldName
         * 
         */

        private async void Entry_FieldName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (sender as Entry != null)
                {
                    PartOfCapital SelectedPart = GetPart((sender as Element).ClassId);

                    SelectedPart.Name = e.NewTextValue;

                    SaveData();
                }                
            }

            catch (Exception error)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", error.Message, "ОK");
            }
        }

        /*
         * 
         * CheckBox_ChangeVisibility
         * 
         */

        private async void CheckBox_ChangeVisibility_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            try
            {
                if (sender as CheckBox != null)
                {
                    PartOfCapital SelectedPart = GetPart((sender as Element).ClassId);

                    SelectedPart.Visibility = e.Value;

                    CalculateResult();

                    SaveData();
                }
            }

            catch (Exception error)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", error.Message, "ОK");
            }
        }


        /*
         * 
         * Entry_AmountOfMoney
         * 
         */

        private async void Entry_AmountOfMoney_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (sender as Entry != null)
                {
                    // По факту MaxLength 10, эта особенность появляется из за добавления пробелов к числам
                    if ((sender as Entry).Text.Length >= (sender as Entry).MaxLength)
                    {
                        (sender as Entry).Text = e.OldTextValue;
                        return;
                    }

                    //  Во избежания захода в этот обработчик два раза подряд во время добавления пробелов в строку (Entry.Text)

                    if (e.OldTextValue == e.NewTextValue.Replace(" ", ""))
                    {
                        return;
                    }

                    string EnteredText = (sender as Entry).Text.Replace(" ", "");

                    //  Обрезание дробной части числа в строке до сотых

                    string[] SplitText = EnteredText.Split(".");

                    if (SplitText.Length > 1)
                    {
                        if (SplitText[1].Length > 2)
                        {
                            SplitText[1] = SplitText[1].Substring(0, 2);

                            EnteredText = SplitText[0] + "." + SplitText[1];
                        }
                    }

                    //  Попытка преобразовать строку в число

                    double EnteredValue;

                    if ((sender as Entry).Text == "")
                    {
                        EnteredValue = 0;
                    }

                    else
                    {
                        if (Double.TryParse(EnteredText, NumberStyles.Float, CultureInfo.InvariantCulture, out EnteredValue) == false)
                        {
                            EnteredText = EnteredText.Remove(EnteredText.Length - 1);
                            (sender as Entry).Text = EnteredText;
                            await Application.Current.MainPage.DisplayAlert("Предупреждение",
                                "Можно вводить только целые и дробные числа.", "ОK");
                        }
                    }

                    PartOfCapital SelectedPart = GetPart((sender as Element).ClassId);

                    SelectedPart.Value = EnteredValue;

                    (sender as Entry).Text = MakeSpaceInNumber(EnteredText);

                    if (SelectedPart.SelectedCurrency == TypeOfCurrency.NotDefined)
                    {
                        return;
                    }

                    CalculateResult();

                    SaveData();
                }
            }

            catch (Exception error)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", error.Message, "ОK");
            }
        }


        /*
         * 
         * Button_TypeOfCurrency
         * 
         */

        private async void Button_TypeOfCurrency_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender as Button != null)
                {
                    string SelectedCurrency = await DisplayActionSheet("Выберите валюту:", null, null,
                        "Рубль", "Доллар", "Евро");

                    if (SelectedCurrency == null)
                    {
                        return;
                    }

                    (sender as Button).Text = SelectedCurrency;

                    PartOfCapital SelectedPart = GetPart((sender as Element).ClassId);

                    SelectedPart.SelectedCurrency = Currency.GetType(SelectedCurrency);

                    CalculateResult();

                    SaveData();
                }
            }

            catch (Exception error)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", error.Message, "ОK");
            }
        }


        /*
         * 
         * Button_DeleteField
         * 
         */

        private async void Button_DeleteField_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender as Button != null)
                {
                    PartOfCapital SelectedPart = GetPart((sender as Element).ClassId);

                    Parts.Remove(SelectedPart);

                    VerticalStackLayout_Content.Remove(SelectedPart.UI as IView);

                    CalculateResult();

                    SaveData();
                }
            }

            catch (Exception error)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", error.Message, "ОK");
            }
        }
    }
}

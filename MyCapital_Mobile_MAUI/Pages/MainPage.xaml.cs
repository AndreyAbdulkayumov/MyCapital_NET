using System.Globalization;
using Core;
using Core.RateSourse_RussianCentralBank;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyCapital_Mobile_MAUI;

public partial class MainPage : ContentPage
{
    private const string SaveFileName = "Capital.json";

    private readonly List<PartOfCapital> Parts = new List<PartOfCapital>();

    private readonly IRateSource Data_CB = new RussianCentralBank();

    private double AmountOfMoney_rub = 0;
    private TypeOfCurrency ResultCurrency = TypeOfCurrency.Ruble;


    public MainPage()
	{
        InitializeComponent();

        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            ViewErrorConnectionPage();
        }
    }

    private async void ViewErrorConnectionPage()
    {
        // Перейти на страницу без анимации перехода
        await Shell.Current.GoToAsync(nameof(ErrorConnectionPage), false);
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        try
        {
            Data_CB.Init();

            DateTime Date = DateTime.Parse(Data_CB.UpdateDate);

            Label_Date.Text = "Курс валют на " + Date.ToString("d MMMM yyyy");

            Label_Rate_Dollar.Text += Data_CB.GetRate(TypeOfCurrency.Dollar).Value + " руб.";
            Label_Rate_Euro.Text += Data_CB.GetRate(TypeOfCurrency.Euro).Value + " руб.";

            List<CapitalValue>? PartsFromSaveFile = SaveFile.GetAllParts(SaveFileName);

            // Если файл пуст
            if (PartsFromSaveFile == null)
            {
                return;
            }

            TypeOfCurrency CurrentCurrency;

            foreach(CapitalValue element in PartsFromSaveFile)
            {
                CurrentCurrency = Currency.GetType(element.Currency);

                IView Field = FieldBuilder.Build(
                    Entry_FieldName_TextChanged,
                    CheckBox_ChangeVisibility_CheckedChanged,
                    Entry_AmountOfMoney_TextChanged,
                    Button_TypeOfCurrency_Clicked,
                    Button_DeleteField_Clicked,
                    element.Name,
                    element.Visibility,
                    element.Value != 0 ? MakeSpaceInNumber(element.Value.ToString()) : null,
                    CurrentCurrency != TypeOfCurrency.NotDefined ? Currency.GetName(CurrentCurrency) : null
                    );

                VerticalStackLayout_Content.Add(Field);

                PartOfCapital Part = new PartOfCapital(
                    FieldBuilder.LastID,
                    element.Name,
                    element.Visibility,
                    Currency.GetType(element.Currency),
                    element.Value,
                    Field
                    );

                Parts.Add(Part);
            }

            CalculateResult();
        }

        catch (Exception error)
        {
            await DisplayAlert("Ошибка", "Ошибка запуска приложения:\n\n" + error.Message, "ОK");
        }
    }    

    private async void Button_AmountOfMoney_Clicked(object sender, EventArgs e)
    {
        try
        {
            string SelectedCurrency = await DisplayActionSheet("Выберите валюту:", null, null,
                    "Рубль", "Доллар", "Евро");

            if (SelectedCurrency == null)
            {
                return;
            }

            ResultCurrency = Currency.GetType(SelectedCurrency);

            Button_AmountOfMoney.Text = MakeSpaceInNumber(AmountOfMoney_ConvertIn(ResultCurrency).ToString(CultureInfo.InvariantCulture)) +
                " " + Currency.GetShortName(ResultCurrency);
        }

        catch (Exception error)
        {
            await DisplayAlert("Ошибка", error.Message, "ОK");
        }
    }
    
    private async void Button_CreateNewField_Clicked(object sender, EventArgs e)
    {
        try
        {
            IView Field = FieldBuilder.Build
                (
                Entry_FieldName_TextChanged,
                CheckBox_ChangeVisibility_CheckedChanged,
                Entry_AmountOfMoney_TextChanged,
                Button_TypeOfCurrency_Clicked,
                Button_DeleteField_Clicked,
                null,
                true,
                null,
                null
                );

            VerticalStackLayout_Content.Add(Field);

            Parts.Add(new PartOfCapital(FieldBuilder.LastID) 
            { 
                UI = Field, 
                Visibility = true,
                SelectedCurrency = TypeOfCurrency.NotDefined, 
                Value = 0 
            });

            SaveData();
        }

        catch (Exception error)
        {
            await DisplayAlert("Ошибка", error.Message, "ОK");
        }
    }
}
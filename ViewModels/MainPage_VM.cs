using Core;
using DynamicData;
using ReactiveUI;
using Services.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive;

namespace ViewModels;

public class MainPage_VM : ReactiveObject
{
    private const string SaveFileName = "Capital.json";

    private string _rateOfCurrencyDate = "Курс валют на xx.xx.xxxx";

    public string RateOfCurrencyDate
    {
        get => _rateOfCurrencyDate;
        set => this.RaiseAndSetIfChanged(ref _rateOfCurrencyDate, value);
    }

    private string _rateDollar = string.Empty;

    public string RateDollar
    {
        get => _rateDollar;
        set => this.RaiseAndSetIfChanged(ref _rateDollar, value);
    }

    private string _rateEuro = string.Empty;

    public string RateEuro
    {
        get => _rateEuro;
        set => this.RaiseAndSetIfChanged(ref _rateEuro, value);
    }

    private string _amountOfMoney = string.Empty;

    public string AmountOfMoney
    {
        get => _amountOfMoney;
        set => this.RaiseAndSetIfChanged(ref _amountOfMoney, value);
    }

    public ReactiveCommand<Unit, Unit> Command_ChangeСurrency { get; set; }
    public ReactiveCommand<Unit, Unit> Command_CreateNewField { get; set; }

    private ObservableCollection<Field_VM> _fields = new ObservableCollection<Field_VM>();

    public ObservableCollection<Field_VM> Fields
    {
        get => _fields;
        set => this.RaiseAndSetIfChanged(ref _fields, value);
    }

    private readonly INotifications _notifications;
    private readonly IRateSource _rateSource;

    private double _amountOfMoney_rub = 0;
    private TypeOfCurrency _resultCurrency = TypeOfCurrency.Ruble;

    public MainPage_VM(INotifications notifications, IRateSource rateSource)
    {
        _notifications = notifications ?? throw new ArgumentNullException(nameof(notifications));
        _rateSource = rateSource ?? throw new ArgumentNullException(nameof(rateSource));

        Command_ChangeСurrency = ReactiveCommand.CreateFromTask(ChangeCurrency);
        Command_ChangeСurrency.ThrownExceptions.Subscribe(async error => await _notifications.ShowAlert("Ошибка", $"Ошибка изменения типа валюты.\n\n{error.Message}", "ОK"));

        Command_CreateNewField = ReactiveCommand.Create(() => Fields.Add(new Field_VM(null, RemoveField, _notifications)));
        Command_CreateNewField.ThrownExceptions.Subscribe(async error => await _notifications.ShowAlert("Ошибка", $"Ошибка создания нового поля.\n\n{error.Message}", "ОK"));
    }

    private void RemoveField(Guid fieldId)
    {
        var removedField = Fields.FirstOrDefault(f => f.Id == fieldId);

        if (removedField == null)
            return;

        Fields.Remove(removedField);
    }

    public async Task Init()
    {
        try
        {
            _rateSource.Init();

            var date = DateTime.Parse(_rateSource.UpdateDate);

            RateOfCurrencyDate = "Курс валют на " + date.ToString("d MMMM yyyy");

            RateDollar = $"1$ = {_rateSource.GetRate(TypeOfCurrency.Dollar).Value} руб.";
            RateEuro = $"1E = {_rateSource.GetRate(TypeOfCurrency.Euro).Value} руб.";

            var partsFromSaveFile = SaveFile.GetAllParts(SaveFileName);

            if (partsFromSaveFile == null)
                return;

            foreach (var part in partsFromSaveFile)
            {
                Fields.Add(new Field_VM(part, RemoveField, _notifications));
            }
        }

        catch (Exception error)
        {
            await _notifications.ShowAlert("Ошибка", $"Ошибка запуска приложения:\n\n{error.Message}", "Ок");
        }        
    }

    private async Task ChangeCurrency()
    {
        var SelectedCurrency = await _notifications.ShowSheet("Выберите валюту:", null, null, "Рубль", "Доллар", "Евро");

        if (SelectedCurrency == null)
            return;

        _resultCurrency = Currency.GetType(SelectedCurrency);

        AmountOfMoney = $"{MakeSpaceInNumber(AmountOfMoney_ConvertIn(_resultCurrency).ToString(CultureInfo.InvariantCulture))} {Currency.GetShortName(_resultCurrency)}";
    }

    //private void CalculateResult()
    //{
    //    try
    //    {
    //        AmountOfMoney_rub = 0;

    //        foreach (PartOfCapital element in Fields)
    //        {
    //            if (element.SelectedCurrency != TypeOfCurrency.NotDefined && element.Visibility == true)
    //            {
    //                AmountOfMoney_rub += element.Value * Data_CB.GetRate(element.SelectedCurrency).Value;
    //            }
    //        }

    //        Button_AmountOfMoney.Text = MakeSpaceInNumber(AmountOfMoney_ConvertIn(ResultCurrency).ToString(CultureInfo.InvariantCulture)) +
    //            " " + Currency.GetShortName(ResultCurrency);
        
    //    }

    //    catch (Exception error)
    //    {
    //        throw new Exception("Ошибка подсчета результата:\n\n" + error.Message);
    //    }
    //}

    private double AmountOfMoney_ConvertIn(TypeOfCurrency Type)
    {
        switch (Type)
        {
            case TypeOfCurrency.Ruble:
                return Math.Round(_amountOfMoney_rub, 2);

            case TypeOfCurrency.Dollar:
                return CurrencyConvert.ToDollar(_amountOfMoney_rub, TypeOfCurrency.Ruble, _rateSource);

            case TypeOfCurrency.Euro:
                return CurrencyConvert.ToEuro(_amountOfMoney_rub, TypeOfCurrency.Ruble, _rateSource);

            default:
                throw new Exception("Неизвестная итоговая валюта.");
        }
    }

    private string MakeSpaceInNumber(string EnteredText)
    {
        var SplitText = EnteredText.Split('.');

        var FieldNumber_Original = SplitText[0].ToCharArray();

        var FieldNumber_Modified = new List<char>();

        Array.Reverse(FieldNumber_Original);

        for (int i = 0; i < FieldNumber_Original.Length; i++)
        {
            FieldNumber_Modified.Add(FieldNumber_Original[i]);

            if ((i != FieldNumber_Original.Length - 1) && (i + 1) % 3 == 0)
            {
                FieldNumber_Modified.Add(' ');
            }
        }

        var OutArray = FieldNumber_Modified.ToArray();

        Array.Reverse(OutArray);

        var FieldNumber = new string(OutArray);

        if (SplitText.Length > 1)
        {
            FieldNumber += "." + SplitText[1];
        }

        return FieldNumber;
    }
}

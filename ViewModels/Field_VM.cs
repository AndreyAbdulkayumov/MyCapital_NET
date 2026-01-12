using Core;
using ReactiveUI;
using Services.Interfaces;
using System.Reactive;

namespace ViewModels;

public class Field_VM : ReactiveObject
{
    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            this.RaiseAndSetIfChanged(ref _isSelected, value);
            _value.Visibility = value;
        }
    }

    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    private string _amountOfMoney = string.Empty;

    public string AmountOfMoney
    {
        get => _amountOfMoney;
        set => this.RaiseAndSetIfChanged(ref _amountOfMoney, value);
    }

    private string _selectedCurrencyName = "Рубль";

    public string SelectedCurrencyName
    {
        get => _selectedCurrencyName;
        set => this.RaiseAndSetIfChanged(ref _selectedCurrencyName, value);
    }
 
    public ReactiveCommand<Unit, Unit> Command_RemoveField { get; set; }
    public ReactiveCommand<Unit, Unit> Command_SelectTypeOfCurrency { get; set; }

    public readonly Guid Id;

    private readonly CapitalValue _value;

    private readonly INotifications _notifications;

    public Field_VM(CapitalValue? initValue, Action<Guid> removeFieldHandler, INotifications notifications)
    {
        _notifications = notifications ?? throw new ArgumentNullException(nameof(notifications));

        Id = Guid.NewGuid();

        if (initValue != null)
        {
            _value = initValue;

            IsSelected = initValue.Visibility;
            Title = initValue.Name;

            var currencyType = Currency.GetType(initValue.Currency);
            SelectedCurrencyName = currencyType == TypeOfCurrency.NotDefined ? "Рубль" : Currency.GetName(currencyType);

            AmountOfMoney = initValue.Value.ToString();
        }

        else
        {
            _value = new CapitalValue(string.Empty, true, "Рубль", 0);
        }
        
        Command_RemoveField = ReactiveCommand.Create(() => removeFieldHandler(Id));
        Command_RemoveField.ThrownExceptions.Subscribe(async error => await _notifications.ShowAlert("Ошибка", error.Message, "ОК"));

        Command_SelectTypeOfCurrency = ReactiveCommand.CreateFromTask(SelectTypeOfCurrency);
        Command_SelectTypeOfCurrency.ThrownExceptions.Subscribe(async error => await _notifications.ShowAlert("Ошибка", error.Message, "ОК"));
    }

    private async Task SelectTypeOfCurrency()
    {
        var SelectedCurrency = await _notifications.ShowSheet("Выберите валюту:", null, null, "Рубль", "Доллар", "Евро");

        if (SelectedCurrency == null)
            return;

        SelectedCurrencyName = SelectedCurrency;

        //PartOfCapital SelectedPart = GetPart(Control.ClassId);

        //SelectedPart.SelectedCurrency = Currency.GetType(SelectedCurrency);

        //CalculateResult();

        //SaveData();
    }
}

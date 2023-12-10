using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class CapitalValue
    {
        public string Name { get; set; }
        public bool Visibility { get; set; }
        public string Currency { get; set; }
        public double Value { get; set; }

        public CapitalValue(string Name, bool Visibility, string Currency, double Value)
        {
            this.Name = Name;
            this.Visibility = Visibility;
            this.Currency = Currency;
            this.Value = Value;
        }
    }

    public class PartOfCapital
    {
        public readonly int ID;
        public string Name { get; set; }
        public bool Visibility { get; set; }
        public TypeOfCurrency SelectedCurrency { get; set; }
        public double Value { get; set; }
        public object UI { get; set; }
        public CapitalValue ValueForSave
        {
            get
            {
                return new CapitalValue(Name, Visibility, SelectedCurrency.ToString(), Value);
            }
        }

        public PartOfCapital(int ID)
        {
            this.ID = ID;
        }

        public PartOfCapital(int ID, string Name, bool Visibility, TypeOfCurrency SelectedCurrency, double Value, object UI)
        {
            this.ID = ID;
            this.Name = Name;
            this.Visibility = Visibility;
            this.SelectedCurrency = SelectedCurrency;
            this.Value = Value;
            this.UI = UI;
        }
    }
}
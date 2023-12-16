using Core;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCapital_Mobile_MAUI
{
    public static class FieldBuilder
    {
        public static int LastID { get; private set; } = -1;

        public static IView Build(
            EventHandler<TextChangedEventArgs> Entry_FieldName_TextChanged_Handler,
            EventHandler<CheckedChangedEventArgs> CheckBox_CheckedChanged_Handler,
            EventHandler<TextChangedEventArgs> Entry_AmountOfMoney_TextChanged_Handler,
            EventHandler Button_TypeOfCurrency_Click_Handler,
            EventHandler Button_DeleteField_Click_Handler,
            string? FieldName,
            bool Visibility,
            string? AmountOfMoney,
            string? SeletedCurrency)
        {
            LastID++;

            string FieldID = Convert.ToString(LastID);

            CheckBox CheckBox_ChangeVisibility = new CheckBox()
            {
                ClassId = FieldID,

                IsChecked = true,

                Color = Color.FromArgb("FFECECEC"),

                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End
            };

            CheckBox_ChangeVisibility.IsChecked = Visibility;
            CheckBox_ChangeVisibility.CheckedChanged += CheckBox_CheckedChanged_Handler;


            Entry Entry_FieldName = new Entry()
            {
                ClassId = FieldID,

                Placeholder = "Введите имя",

                Text = FieldName,

                FontAutoScalingEnabled = false,
                TextColor = Colors.White,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            Entry_FieldName.TextChanged += Entry_FieldName_TextChanged_Handler;
            
            Entry Entry_AmountOfMoney = new Entry()
            {
                ClassId = FieldID,

                Keyboard = Keyboard.Numeric,

                Placeholder = "...",

                Text = AmountOfMoney != null ? AmountOfMoney : String.Empty,

                WidthRequest = 120,

                BackgroundColor = Color.FromArgb("FF404040"),

                FontAutoScalingEnabled = false,
                TextColor = Color.FromArgb("FFD0D0D0"),
                FontSize = 20,
                MaxLength = 11,  // По факту 10, эта особенность появляется из за добавления пробелов к числам
                HorizontalTextAlignment = TextAlignment.Center,

                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End
            };

            Entry_AmountOfMoney.TextChanged += Entry_AmountOfMoney_TextChanged_Handler;

            Button Button_TypeOfCurrency = new Button()
            {
                ClassId = FieldID,

                Text = SeletedCurrency != null ? SeletedCurrency : String.Empty,
                FontAutoScalingEnabled = false,
                FontSize = 20,
                TextColor = Color.FromArgb("FF569CD6"),

                Background = new SolidColorBrush(Color.FromArgb("FF404040")),

                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };

            Button_TypeOfCurrency.Clicked += Button_TypeOfCurrency_Click_Handler;

            Button Button_DeleteField = new Button()
            {
                ClassId = FieldID,

                WidthRequest = 40,

                Text = "X",
                FontAutoScalingEnabled = false,
                FontSize = 20,
                TextColor = Colors.IndianRed,

                Background = new SolidColorBrush(Color.FromArgb("FF404040")),

                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };

            Button_DeleteField.Clicked += Button_DeleteField_Click_Handler;

            Grid Field = new Grid();

            Field.HeightRequest = 110;

            Field.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            Field.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));


            Grid UpRow = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center,
            };

            UpRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(60, GridUnitType.Absolute)));
            UpRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(260, GridUnitType.Absolute)));
            UpRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(60, GridUnitType.Absolute)));

            UpRow.Children.Add(CheckBox_ChangeVisibility);
            UpRow.SetColumn(CheckBox_ChangeVisibility, 0);

            UpRow.Children.Add(Entry_FieldName);
            UpRow.SetColumn(Entry_FieldName, 1);

            UpRow.Children.Add(Button_DeleteField);
            UpRow.SetColumn(Button_DeleteField, 2);


            Grid DownRow = new Grid();

            DownRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            DownRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(20, GridUnitType.Absolute)));
            DownRow.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(0.9, GridUnitType.Star)));

            DownRow.Children.Add(Entry_AmountOfMoney);
            DownRow.SetColumn(Entry_AmountOfMoney, 0);

            DownRow.Children.Add(Button_TypeOfCurrency);
            DownRow.SetColumn(Button_TypeOfCurrency, 2);


            Field.Children.Add(UpRow);
            Field.SetRow(UpRow, 0);

            Field.Children.Add(DownRow);
            Field.SetRow(DownRow, 1);


            return Field;
        }
    }
}

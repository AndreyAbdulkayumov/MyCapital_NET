namespace MyCapital_Mobile_MAUI
{
    public partial class AppShell : Shell
    {
        private const int TIME_DELAY_MS = 200;
        private DateTime PressTime = new DateTime();

        public AppShell()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            // Выход из приложения по двойному нажатию на кнопку Назад (Android)
            if (PressTime.AddMilliseconds(TIME_DELAY_MS) > DateTime.Now)
            {
                Application.Current?.Quit();
            }

            PressTime = DateTime.Now;

            // true - отмена текущего действия
            // false - продолжение выполнения текущего действия
            return true;
        }
    }
}

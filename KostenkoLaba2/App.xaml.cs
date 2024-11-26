using Microsoft.Maui.Controls;
using System;

namespace KostenkoLaba2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Views.SelectFile());
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);
            if (window != null)
            {
                ConfigureWindow(window);
            }

#if WINDOWS
            ConfigureWindowsSpecificBehavior(window);
#endif

            return window;
        }

        private void ConfigureWindow(Window window)
        {
            window.Title = "XML Analyzer";
            window.Width = 1100;
            window.Height = 500;
        }

#if WINDOWS
        private void ConfigureWindowsSpecificBehavior(Window window)
        {
            window.Created += (s, e) =>
            {
                var handle = WinRT.Interop.WindowNative.GetWindowHandle(window.Handler.PlatformView);
                var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

                appWindow.Closing += async (s, e) =>
                {
                    e.Cancel = true;
                    bool shouldClose = await ConfirmExit();
                    if (shouldClose)
                    {
                        App.Current.Quit();
                    }
                };
            };
        }

        private static Task<bool> ConfirmExit()
        {
            return App.Current.MainPage.DisplayAlert(
                "Підтвердження",
                "Ви впевнені, що хочете завершити?",
                "Так",
                "Ні");
        }
#endif
    }
}

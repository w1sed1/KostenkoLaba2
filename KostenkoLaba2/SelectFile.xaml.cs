
using Microsoft.Maui.Controls;
using System;
using System.IO;

namespace KostenkoLaba2.Views
{
    public partial class SelectFile : ContentPage
    {
        public string SelectedFilePath { get; private set; }

        public SelectFile()
        {
            InitializeComponent();
        }

        private async void OnAddFileClicked(object sender, EventArgs e)
        {
           
            var result = await FilePicker.PickAsync();

            if (result != null)
            {
                
                if (Path.GetExtension(result.FullPath).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    SelectedFilePath = result.FullPath; 
                    
                    await Navigation.PushAsync(new MainPage(SelectedFilePath));
                }
                else
                {
                    
                    await DisplayAlert("Помилка", "Будь ласка, оберіть файл у форматі XML.", "OK");
                }
            }
        }


        
    }
}

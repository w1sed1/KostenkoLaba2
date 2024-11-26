using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using KostenkoLaba2.Parsers;

namespace KostenkoLaba2.Views
{
    public partial class Results : ContentPage
    {
        private readonly IList<Person> _searchResults;

        public Results(IList<Person> searchResults)
        {
            InitializeComponent();
            _searchResults = searchResults ?? throw new ArgumentNullException(nameof(searchResults));
            ResultsCollectionView.ItemsSource = _searchResults;
        }

        private async void OnTransformToHtmlClicked(object sender, EventArgs e)
        {
            try
            {
                // Save search results to a temporary XML file
                string tempXmlPath = Path.Combine(Path.GetTempPath(), "searchResults.xml");
                SaveResultsToXml(tempXmlPath, _searchResults);

                // Load the XSL template
                var xslt = new XslCompiledTransform();
                using var xslStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("KostenkoLaba2.Resources.template.xsl");
                if (xslStream == null)
                {
                    await ShowAlert("Помилка", "Шаблон template.xsl не знайдено як ресурс.");
                    return;
                }

                using var xslReader = XmlReader.Create(xslStream);
                xslt.Load(xslReader);

                // Transform the XML to HTML
                string outputHtmlPath = GetHtmlOutputPath();
                using var writer = new StreamWriter(outputHtmlPath);
                xslt.Transform(tempXmlPath, null, writer);

                await ShowAlert("Успіх", $"HTML файл збережено в папці: {outputHtmlPath}");
            }
            catch (Exception ex)
            {
                await ShowAlert("Помилка", $"Не вдалося виконати трансформацію: {ex.Message}");
            }
        }

        private static string GetHtmlOutputPath()
        {
            string projectPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent?.Parent?.FullName
                ?? throw new InvalidOperationException("Не вдалося визначити шлях проекту.");
            return Path.Combine(projectPath, "searchResults.html");
        }

        private static void SaveResultsToXml(string filePath, IEnumerable<Person> searchResults)
        {
            var document = new XDocument(new XElement("Results"));

            foreach (var person in searchResults)
            {
                document.Root?.Add(new XElement("Person",
                    new XAttribute("FullName", person.FullName ?? "N/A"),
                    new XAttribute("Faculty", person.Faculty ?? "N/A"),
                    new XAttribute("Course", person.Course ?? "N/A"),
                    new XElement("Room", person.Room ?? "N/A")
                ));
            }

            document.Save(filePath);
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private static Task ShowAlert(string title, string message)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}

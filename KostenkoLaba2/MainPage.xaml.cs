using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using KostenkoLaba2.Parsers;

namespace KostenkoLaba2.Views
{
    public partial class MainPage : ContentPage
    {
        public string FilePath { get; }
        public IXmlParser CurrentParser { get; private set; }

        public MainPage(string filePath)
        {
            InitializeComponent();
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

            ParserPicker.SelectedIndexChanged += OnParserPickerChanged;
            LoadCoursePickerData();
        }

        private void LoadCoursePickerData()
        {
            CoursePicker.Items.Clear();

            try
            {
                var courses = ExtractCoursesFromXml(FilePath);
                foreach (var course in courses)
                {
                    CoursePicker.Items.Add(course);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час завантаження даних: {ex.Message}");
            }
        }

        private static HashSet<string> ExtractCoursesFromXml(string filePath)
        {
            var courses = new HashSet<string>();
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };

            using var stream = File.OpenRead(filePath);
            using var reader = XmlReader.Create(stream, settings);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Person")
                {
                    if (reader.GetAttribute("Course") is string course)
                    {
                        courses.Add(course);
                    }
                }
            }

            return courses;
        }

        private void OnParserPickerChanged(object sender, EventArgs e)
        {
            if (ParserPicker.SelectedItem == null) return;

            CurrentParser = ParserPicker.SelectedItem.ToString() switch
            {
                "DOM" => new DomXmlParser(),
                "LINQ to XML" => new LinqXmlParser(),
                "SAX" => new SaxXmlParser(),
                _ => null
            };

            if (CurrentParser == null) return;

            LoadXmlWithParser();
        }

        private void LoadXmlWithParser()
        {
            try
            {
                var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
                using var stream = File.OpenRead(FilePath);

                if (CurrentParser.Load(stream, settings))
                {
                    Console.WriteLine("Файл завантажено успішно.");
                }
                else
                {
                    Console.WriteLine("Помилка завантаження файлу.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка завантаження XML: {ex.Message}");
            }
        }

        private async void OnFindClicked(object sender, EventArgs e)
        {
            if (CurrentParser == null)
            {
                await DisplayAlert("Помилка", "Парсер не обрано.", "OK");
                return;
            }

            var filters = CreateFiltersFromInputs();
            IList<Person> results = CurrentParser.Find(filters);

            if (results.Count > 0)
            {
                await Navigation.PushAsync(new Results(results));
            }
            else
            {
                await DisplayAlert("Результати пошуку", "Результатів не знайдено.", "OK");
            }
        }

        private Filters CreateFiltersFromInputs()
        {
            var filters = new Filters();

            if (NameCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(NameEntry.Text))
            {
                filters.AttributeName = "FullName";
                filters.AttributeValue = NameEntry.Text;
            }

            if (FacultyCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(FacultyEntry.Text))
            {
                filters.NodeName = "Faculty";
                filters.NodeValue = FacultyEntry.Text;
            }

            if (CourseCheckBox.IsChecked == true && CoursePicker.SelectedItem != null)
            {
                filters.NodeName = "Course";
                filters.NodeValue = CoursePicker.SelectedItem.ToString();
            }

            if (RoomCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(RoomEntry.Text))
            {
                filters.NodeName = "Room";
                filters.NodeValue = RoomEntry.Text;
            }

            return filters;
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            NameEntry.Text = string.Empty;
            FacultyEntry.Text = string.Empty;
            RoomEntry.Text = string.Empty;
            CoursePicker.SelectedIndex = -1;
        }
    }
}

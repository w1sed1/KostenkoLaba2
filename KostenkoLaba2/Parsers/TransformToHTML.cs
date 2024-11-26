using System;
using System.IO;
using System.Xml.Xsl;

namespace KostenkoLaba2.Parsers
{
    /// <summary>
    /// Клас для трансформації XML у HTML за допомогою XSLT.
    /// </summary>
    public static class XmlTransformer
    {
        /// <summary>
        /// Трансформує XML-файл у HTML-файл, використовуючи XSLT-шаблон.
        /// </summary>
        /// <param name="xmlPath">Шлях до XML-файлу.</param>
        /// <param name="xslPath">Шлях до XSLT-шаблону.</param>
        /// <param name="outputHtmlPath">Шлях до вихідного HTML-файлу.</param>
        /// <returns>Повертає true, якщо трансформація виконана успішно, інакше false.</returns>
        public static bool TransformXmlToHtml(string xmlPath, string xslPath, string outputHtmlPath)
        {
            if (string.IsNullOrWhiteSpace(xmlPath) || string.IsNullOrWhiteSpace(xslPath) || string.IsNullOrWhiteSpace(outputHtmlPath))
            {
                Console.WriteLine("Помилка: Один або кілька шляхів є порожніми.");
                return false;
            }

            if (!File.Exists(xmlPath))
            {
                Console.WriteLine($"Помилка: XML-файл не знайдено за шляхом {xmlPath}.");
                return false;
            }

            if (!File.Exists(xslPath))
            {
                Console.WriteLine($"Помилка: XSL-файл не знайдено за шляхом {xslPath}.");
                return false;
            }

            try
            {
                var xslt = new XslCompiledTransform();
                xslt.Load(xslPath);

                using var writer = new StreamWriter(outputHtmlPath);
                xslt.Transform(xmlPath, null, writer);

                Console.WriteLine($"Трансформація виконана успішно. HTML збережено за шляхом {outputHtmlPath}.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час трансформації: {ex.Message}");
                return false;
            }
        }
    }
}

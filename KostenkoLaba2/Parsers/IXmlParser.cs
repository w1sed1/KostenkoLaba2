    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    namespace KostenkoLaba2.Parsers
    {
        
        // Інтерфейс для парсерів XML, який задає методи для завантаження та пошуку даних
        
        public interface IXmlParser
        {
            bool Load(Stream inputStream, XmlReaderSettings settings);
            IList<Person> Find(Filters filters);
        }
    }



﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace KostenkoLaba2.Parsers
{
    public class DomXmlParser : IXmlParser
    {
        private readonly List<Person> _people;

        public DomXmlParser()
        {
            _people = new List<Person>();
        }

        public bool Load(Stream inputStream, XmlReaderSettings settings)
        {
            _people.Clear();

            var document = new XmlDocument();
            try
            {
                using var reader = XmlReader.Create(inputStream, settings);
                document.Load(reader);

                if (document.DocumentElement == null)
                    return false;

                foreach (XmlNode personNode in document.DocumentElement.SelectNodes("Person"))
                {
                    var person = new Person
                    {
                        FullName = personNode.Attributes["FullName"]?.Value ?? "",
                        Room = personNode.SelectSingleNode("Room")?.InnerText ?? ""
                    };

                  
                    foreach (XmlAttribute attribute in personNode.Attributes)
                    {
                        person.Attributes[attribute.Name] = attribute.Value;
                    }

                    
                    if (personNode.SelectSingleNode("Faculty") != null)
                        person.Attributes["Faculty"] = personNode.SelectSingleNode("Faculty").InnerText;

                    if (personNode.SelectSingleNode("Course") != null)
                        person.Attributes["Course"] = personNode.SelectSingleNode("Course").InnerText;

                    
                   

                    _people.Add(person);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading XML: {ex.Message}");
                return false;
            }
        }

        public IList<Person> Find(Filters filters)
        {
            return _people.FindAll(person => filters.ValidatePerson(person));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Configuration;
using chat_server.Interfaces;

namespace chat_server.Entities
{
    class BaseStorage : IStore
    {
        protected static String projectDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                         + $@"\{ConfigurationManager.AppSettings["project_name"]}\";

        public static bool Create<T>()
        {
            return true;
        }

        public static List<T> All<T>()
        {
            return new List<T>();
        }

        internal static T Delete<T>()
        {
            throw new NotImplementedException();
        }

        public T Save<T>(T objectToSave, string key, bool update = false)
        {
            var entities = new List<T>();
            var type = typeof(T);
            var typeName = typeof(T).Name;
            var document = LoadTypeData(typeName);
            var xElement = SerializeToXElement<T>(objectToSave);

            var prop = type.GetProperty(key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            string keyValue = (string)prop.GetValue(objectToSave);

            // Find whether entity exists by id - if it does, update it. Otherwise, add it as a new node
            var rootNode = document.Element(typeName + "s");
            var entityNodes = rootNode.Elements(typeName);
            var thisNode = entityNodes.FirstOrDefault(node => (string)node.Attribute("ID") == keyValue);

            if (update || thisNode != null)
            {
                thisNode.ReplaceWith(xElement);
            }
            else
            {
                rootNode.Add(xElement);
            }

            document.Save(projectDocPath + typeName.ToLower() + "s.xml");

            return objectToSave;
        }

        public List<T> Load<T>(string key = null, string value = null, bool all = false)
        {
            var entities = new List<T>();
            var type = typeof(T);
            var typeName = typeof(T).Name;
            var document = LoadTypeData(typeName);

            var xmlNodes = from xmlNode in document.Root.Elements(typeName)
                           select xmlNode;

            if (all)
            {
                foreach (var xmlNode in xmlNodes)
                {
                    var obj = DeserializeFromXElement<T>(xmlNode);
                    entities.Add(obj); // as entities is a list of T, casting now allows not to cast in future
                }
            } else
            {
                var thisNode = xmlNodes.FirstOrDefault(node => (string)node.Attribute(key) == value);

                if (thisNode == null) return entities;

                var obj = DeserializeFromXElement<T>(thisNode);
                entities.Add(obj);
            }

            return entities;
        }

        private static XDocument LoadTypeData(string typeName)
        {
            XDocument document;
            String typeFile = typeName.ToLower() + "s.xml";

            try
            {
                document = XDocument.Load(projectDocPath + typeFile);
            } catch (Exception e)
            {
                document = new XDocument(new XElement(typeName + "s"));
                document.Save(projectDocPath + typeFile);
            }

            return document;
        }

        public T Destroy<T>(T objectToDestroy, string key)
        {
            var type = typeof(T);
            var typeName = typeof(T).Name;
            var document = LoadTypeData(typeName);

            var prop = type.GetProperty(key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            string keyValue = (string)prop.GetValue(objectToDestroy);

            var xmlNodes = from xmlNode in document.Root.Elements(typeName)
                           select xmlNode;

            var rootNode = document.Element(typeName + "s");
            var entityNodes = rootNode.Elements(typeName);
            var thisNode = entityNodes.FirstOrDefault(node => (string)node.Attribute(key) == keyValue);

            thisNode.Remove();
            document.Save(projectDocPath + typeName);

            return objectToDestroy;
        }

        private static object SerializeToXElement<T>(T objectToSave)
        {
            var doc = new XDocument();

            using (XmlWriter writer = doc.CreateWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, objectToSave);
            }

            return doc.Root;
        }

        public static T DeserializeFromXElement<T>(XElement element)
        {
            using (XmlReader reader = element.CreateReader())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T)); // using typeof vs getype -> way to determine type during compile time
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}

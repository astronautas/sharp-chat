using System.Xml.Serialization;

namespace chat_server.Entities
{
    class Message : Entity
    {
        [XmlAttribute]
        public string Body { get; set;}

        public Message(string body)
        {
            Body = body;
        }
    }
}

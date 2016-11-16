using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace chat_server.Entities
{
    public class Entity
    {
        [XmlAttribute]
        public string ID { get; set; }

        public Entity()
        {
            ID = Guid.NewGuid().ToString("N");
        }
    }
}

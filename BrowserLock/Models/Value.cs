using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

namespace BrowserLock.Models
{
    [Serializable]
    public class Value : IData
    {
        private class Props
        {
            public static string Name = "Name";
            public static string Type = "Type";
            public static string Data = "Data";
        }

        public static Value Deserialize(JObject obj)
        {
            string name = (string) obj[Props.Name];
            string type = (string)obj[Props.Type];
            string data = (string)obj[Props.Data];
            var value = new Value()
            {
                Name = name,
                ValueType = type,
                Data = data
            };
            
            return value;
        }

        public JObject ToJSON()
        {
            JObject obj = new JObject();

            obj.Add(AppState.DataClassType, ClassType);
            obj.Add(Props.Name, Name);
            obj.Add(Props.Data, Data);
            obj.Add(Props.Type, ValueType);

            return obj;
        }


        public const string classType = "Value";

        public string ClassType
        {
            get
            {
                return classType;
            }
        }

        public Value() { }

        public Value(string name, RegistryValueKind kind, object value)
        {
            Name = name;
            ValueType = kind.ToString();
            Data = GetValue(kind, value);
        }

        private string GetValue(RegistryValueKind kind, object value)
        {
            switch (kind)
            {
                case RegistryValueKind.String:
                    return value.ToString();
                default:
                    throw new NotImplementedException();
            }
        }

        public string Name { get; set; } //Key
        public string Data {get; set;}
        public string ValueType {get; set;}

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 3
                + Data.GetHashCode() * 7
                + ValueType.GetHashCode() * 11;
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        
    }
}

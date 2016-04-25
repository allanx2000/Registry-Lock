using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BrowserLock.Models
{
    [Serializable]
    public class Value : IData
    {
        public Value(string name, RegistryValueKind kind, object value)
        {
            Name = name;
            Type = kind.ToString();
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
        public string Type {get; set;}

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 3
                + Data.GetHashCode() * 7
                + Type.GetHashCode() * 11;
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}

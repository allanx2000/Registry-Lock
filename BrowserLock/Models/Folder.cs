using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserLock.Models
{
    [Serializable]
    public class Folder : IData
    {
        private class Props
        {
            public const string Name = "Name";
            public const string Children = "Children";
        }

        public JObject ToJSON()
        {
            JObject obj = new JObject();
            obj.Add(AppState.DataClassType, ClassType);
            obj.Add(Props.Name, Name);

            JArray children = new JArray();
            foreach (var c in Children)
            {
                children.Add(c.ToJSON());
            }

            obj.Add(Props.Children, children);

            return obj;
        }

        public static Folder Deserialize(JToken obj)
        {
            string name = (string) obj[Props.Name];
            List<IData> data = AppState.DeserializeIData(obj[Props.Children]);

            return new Folder(name, data); 
        }

        public const string FolderType = "Folder";

        public string Name { get; set; }
        public List<IData> Children { get; set; }

        public string ClassType
        {
            get
            {
                return FolderType;
            }
        }

        public Folder() { }

        public Folder(string name, List<IData> children)
        {
            this.Name = name;
            this.Children = children;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Folder;

            if (other == null
                || other.Name != Name
                || (Children == null && other.Children != null)
                || (Children != null && other.Children == null))
                return false;

            if (Children == null && other.Children == null)
                return true;

            var c1 = other.Children.OrderBy(x => x.Name).ToList();
            var c2 = Children.OrderBy(x => x.Name).ToList();

            if (c1.Count != c2.Count)
                return false;

            for (int i = 0; i< c1.Count; i++)
            {
                if (c1[i] != c2[i])
                    return false;
            }

            return true;
        }
    }
}

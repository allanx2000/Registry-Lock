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
        public string Name { get; set; }
        public List<IData> Children { get; set; }

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

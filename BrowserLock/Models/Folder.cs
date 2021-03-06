﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserLock.Models
{
    public class Folder : IData
    {
        #region Serialization
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
        #endregion

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

        #region Equality
        /*
        
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var other = obj as Folder;

            if (other == null)
                return false;
            else return AppState.ChildrenEqual(this.Children, other.Children);

          
        }
        */
        #endregion
    }
}

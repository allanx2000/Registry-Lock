﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BrowserLock.Models
{
    public class RuleInfo
    {
        #region Serialization
        private static class Props
        {
            public const string ID = "ID";
            public const string CheckerID = "CheckerID";
            public const string Name = "Name";
            public const string Path = "Path";
            public const string Extension = "Extension";
            public const string Data = "Data";
        }

        public static RuleInfo Deserialize(JObject obj)
        {
            int id = (int) obj[Props.ID];
            string checkerId = (string)obj[Props.CheckerID];
            string name = (string)obj[Props.Name];
            string path = (string)obj[Props.Path];
            string extension = (string)obj[Props.Extension];

            List<IData> data = AppState.DeserializeIData(obj[Props.Data]);
            /*
            List<IData> data = new List<IData>();

            foreach (var c in obj[Props.Data].Children<JObject>())
            {
                bool isFolder = (string) c[AppState.DataClassType] == Folder.FolderType;

                IData child = isFolder ? Folder.Deserialize(c) : Value.Deserialize(c);
                data.Add(child);
            }
            */

            RuleInfo rule = new RuleInfo(checkerId, name, data);
            rule.Path = path;
            rule.Extension = extension;
            rule.ID = id;

            return rule;
        }

        public JObject ToJSON()
        {
            JObject obj = new JObject();
            obj.Add(Props.ID, ID);
            obj.Add(Props.CheckerID, CheckerId);
            obj.Add(Props.Name, Name);
            obj.Add(Props.Path, Path);
            obj.Add(Props.Extension, Extension);

            JArray data = new JArray();
            foreach (var d in Data)
            {
                data.Add(d.ToJSON());
            }

            obj.Add(Props.Data, data);

            return obj;
        }
        #endregion

        public string CheckerId { get; set; }

        //First is Folder with full path
        public List<IData> Data { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public int ID { get; set; }


        public RuleInfo()
        { }

        public RuleInfo(string checkerId, string name, List<IData> data)
        {
            this.CheckerId = checkerId;
            this.Data = data;
            this.Name = name;
        }

        public RuleInfo(string checkerId, string name, Folder folder) 
            : this(checkerId, name, new List<IData>() { folder })
        {
        }

        public bool IsEqual(RuleInfo other)
        {
            var hc1 = this.GetHashCode2();
            var hc2 = other.GetHashCode2();
            return this.CheckerId != other.CheckerId
            && hc1 == hc2;

        }

        private int GetHashCode2()
        {
            int ctr = 0;

            foreach (var c in Data)
            {
                ctr += c.GetHashCode();
            }

            return ctr;
        }

        public override int GetHashCode()
        {
            int ctr = CheckerId.GetHashCode();

            foreach (IData data in Data)
            {
                ctr += data.GetHashCode();
            }

            return ctr;
        }

        public override bool Equals(object obj)
        {
            RuleInfo other = obj as RuleInfo;
            if (other == null || this.CheckerId != other.CheckerId)
                return false;
            else
                return AppState.ChildrenEqual(this.Data, other.Data);
        }
    }
}
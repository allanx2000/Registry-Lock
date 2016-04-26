using BrowserLock.Checkers;
using BrowserLock.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BrowserLock
{
    public class AppState
    {
        public const string DataClassType = "ClassType";

        public static bool ChildrenEqual(List<IData> list1, List<IData> list2)
        {
            if ((list1 == null && list2!= null)
                || (list1 != null && list2 == null))
                return false;
            else if (list1 == null && list2 == null)
                return true;

            var sorted1 = list1.OrderBy(x => x.Name).ToList();
            var sorted2 = list2.OrderBy(x => x.Name).ToList();

            if (sorted1.Count != sorted2.Count)
                return false;

            for (int i = 0; i < sorted1.Count; i++)
            {
                Folder folder1 = sorted1[i] as Folder;
                Folder folder2 = sorted2[i] as Folder;

                if (folder1 != null && folder2 != null)
                {
                    if (folder1.Name != folder2.Name)
                        return false;
                    else return ChildrenEqual(folder1.Children, folder2.Children);
                }
                else if (folder1 == null && folder2 != null)
                    return false;
                else if (folder1 != null && folder2 == null)
                    return false;
                //IsValue
                else if (sorted1[i].GetHashCode() != sorted2[i].GetHashCode())
                    return false;
            }

            return true;

        }

        public static List<IData> DeserializeIData(JToken array)
        {
            List<IData> data = new List<IData>();

            foreach (var c in array.Children<JObject>())
            {
                bool isFolder = (string)c["ClassType"] == Folder.FolderType;

                IData child = isFolder ? (IData) Folder.Deserialize(c) : (IData)Value.Deserialize(c);
                data.Add(child);
            }

            return data;
        }

        private static List<IChecker> DefaultCheckers = new List<IChecker>()
        {
            new BrowserChecker(),
            new PathChecker()
        };

        private static readonly Dictionary<string, IChecker> Checkers = new Dictionary<string, IChecker>();
        public IChecker GetChecker(string key)
        {
            return Checkers[key];
        }

        public static readonly AppState Instance = new AppState();

        private const string RulesFile = "rules.json";

        private AppState()
        {
            Rules = LoadRules(RulesFile);

            foreach (var c in DefaultCheckers)
            {
                Checkers.Add(c.ID, c);
            }
        }

        public void SaveRules()
        {
            ExportRules(RulesFile);
        }

        public void ExportRules(string path)
        {
            JArray rules = new JArray();

            foreach (var rule in this.Rules)
            {
                rules.Add(rule.ToJSON());
            }

            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(rules.ToString());
            sw.Close();
        }

        //TODO: Edit
        private List<RuleInfo> LoadRules(string path)
        {
            if (!File.Exists(path))
                return new List<RuleInfo>();

            using (StreamReader sr = new StreamReader(path))
            {
                string json = sr.ReadToEnd();

                sr.Close();

                JArray array = JArray.Parse(json);

                List<RuleInfo> rules = new List<RuleInfo>();

                foreach (var rule in array.Children<JObject>())
                {
                    rules.Add(RuleInfo.Deserialize(rule));
                }

                return rules;
            }
        }

        public List<RuleInfo> Rules
        {
            get; private set;
        }

        public List<IChecker> CheckersList
        {
            get { return DefaultCheckers; }
        }

        internal void Test()
        {
            var checker = Checkers.Values.First();
            var values = checker.GetValues("http", null);
        }

        private int lastId = -1;
        public int GetNextId()
        {
            if (lastId == -1)
            {
                if (Rules.Count == 0)
                    lastId = 1;
                else 
                    lastId = Rules.Max(x => x.ID);
            }

            return lastId++;
        }

        public void AddNewRule(RuleInfo rule)
        {
            var match = FindExisting(rule.Name);

            if (match != null)
                throw new Exception("A rule by the same name already exists: " + match.Name);

            rule.ID = GetNextId();

            Rules.Add(rule);

            SaveRules();
        }

        public RuleInfo FindExisting(RuleInfo rule)
        {
            return FindExisting(rule.ID, rule.Name);
        }

        public RuleInfo FindExisting(string name)
        {
            return (from i in Rules where i.Name == name select i).FirstOrDefault();
        }

        public RuleInfo FindExisting(int id, string name)
        {
            return (from i in Rules where i.Name == name && i.ID != id select i).FirstOrDefault();
        }

        internal void UpdateRule(RuleInfo rule)
        {
            throw new NotImplementedException();
        }

        //TODO: Setup Timer


    }
}

using BrowserLock.Checkers;
using BrowserLock.Models;
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
        private static List<IChecker> DefaultCheckers = new List<IChecker>()
        {
            new BrowserChecker()
        };

        private static readonly Dictionary<string, IChecker> Checkers = new Dictionary<string, IChecker>();

        public static readonly AppState Instance = new AppState();

        private const string RulesFile = "rules.xml";

        private AppState()
        {
            Rules = LoadRules(RulesFile);

            foreach (var c in DefaultCheckers)
            {
                Checkers.Add(c.ID, c);
            }
        }

        public void SaveRules(List<RuleInfo> rules)
        {
            ExportRules(rules, RulesFile);
        }

        public void ExportRules(List<RuleInfo> rules, string path)
        {
            XmlSerializer xser = new XmlSerializer(typeof(List<RuleInfo>));
            StreamWriter sw = new StreamWriter(path);
            xser.Serialize(sw, rules);
            sw.Close();
        }

        private List<RuleInfo> LoadRules(string path)
        {
            if (!File.Exists(path))
                return new List<RuleInfo>();

            using (StreamReader sr = new StreamReader(path))
            {
                XmlSerializer xser = new XmlSerializer(typeof(List<RuleInfo>));
                List<RuleInfo> rules = (List<RuleInfo>)xser.Deserialize(sr);
                sr.Close();

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
            var values = checker.GetValues("http");
        }
        public void AddNewRule(RuleInfo rule)
        {
            var match = FindExisting(rule);
            if (match != null)
                throw new Exception("A rule by the same name already exists: " + match.Name);

            Rules.Add(rule);
        }

        private RuleInfo FindExisting(RuleInfo rule)
        {
            return (from i in Rules where i.Name == rule.Name select i).FirstOrDefault();
        }

        //TODO: Setup Timer


    }
}

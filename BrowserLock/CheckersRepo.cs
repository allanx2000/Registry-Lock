using BrowserLock.Checkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserLock
{
    public class CheckersRepo
    {
        private static List<IChecker> DefaultCheckers = new List<IChecker>()
        {
            new BrowserChecker()
        };

        private static readonly Dictionary<string, IChecker> Checkers = new Dictionary<string, IChecker>();

        public static readonly CheckersRepo Instance = new CheckersRepo();
        
        private CheckersRepo()
        {
            foreach (var c in DefaultCheckers)
            {
                Checkers.Add(c.ID, c);
            }
        }

        internal void Test()
        {
            var checker = Checkers.Values.First();
            var values = checker.GetValues("http");
        }

        //TODO: Setup Timer


    }
}

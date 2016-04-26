using BrowserLock.Checkers;
using BrowserLock.Models;
using BrowserLock.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrowserLock
{
    public static class ChangeChecker
    {
        private static Timer timer;
        private static ICollection<RuleViewModel> rules;
        private static Action<List<RuleViewModel>> onUpdated;

        public static void SetRulesSource(ICollection<RuleViewModel> rules, Action<List<RuleViewModel>> onUpdated)
        {
            ChangeChecker.rules = rules;
            ChangeChecker.onUpdated = onUpdated;
        }

        public static bool RulesSet
        {
            get { return rules != null; }
        }

        private const int Timeout = 10 * 1000;

        public static void StartTimer()
        {
            CheckRulesSet();

            if (timer == null)
            {
                timer = new Timer((state) => CheckForChanges(), null, Timeout, Timeout);
            }

        }
        private static void CheckRulesSet()
        {
            if (!RulesSet)
                throw new Exception("Rules not set.");
        }

        public static void InvokeOnUI(Action action)
        {
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    action.Invoke();
                });
            }
            else
                action.Invoke();

        }

        public static void CheckForChanges(bool silent = true)
        {
            try
            {
                CheckRulesSet();

                List<RuleViewModel> changed = new List<RuleViewModel>();

                foreach (var rule in rules)
                {
                    var original = rule.Original;
                    var checker = AppState.Instance.GetChecker(original.CheckerId);
                    RuleInfo updated = checker.GetValues(original.Path, original.Extension);

                    if (!updated.Equals(original))
                    {
                        InvokeOnUI(() => rule.SetChanged(updated));
                    }

                    if (rule.Current != null)
                        changed.Add(rule);
                }
                
                InvokeOnUI(() => onUpdated.Invoke(changed));

                //return changed;
            }
            catch (Exception e)
            {
                if (!silent)
                    throw;
                /*                
                else
                    return new List<RuleViewModel>();
                */
            }
        }
    }
}

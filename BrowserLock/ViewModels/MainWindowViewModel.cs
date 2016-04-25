using Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Prop = BrowserLock.Properties.Settings;

namespace BrowserLock.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private ObservableCollection<RuleViewModel> rules = new ObservableCollection<RuleViewModel>();
        private ObservableCollection<ChangesViewModel> changes = new ObservableCollection<ChangesViewModel>();

        private CollectionViewSource rulesView, changesView;

        public MainWindowViewModel()
        {
            foreach (var r in AppState.Instance.Rules)
            {
                rules.Add(new RuleViewModel(r));
            }

            var sortName = new SortDescription("Name", ListSortDirection.Ascending);

            rulesView = new CollectionViewSource();
            rulesView.Source = rules;
            rulesView.SortDescriptions.Add(sortName);

            changesView = new CollectionViewSource();
            changesView.Source = changes;
            changesView.SortDescriptions.Add(sortName);

        }

        #region Rules

        public ICollectionView Rules
        {
            get { return rulesView.View; }
        }

        private RuleViewModel selectedRule;
        public RuleViewModel SelectedRule
        {
            get { return selectedRule; }
            set
            {
                selectedRule = value;
                RaisePropertyChanged();
                RaisePropertyChanged("RuleSelected");
            }
        }

        public bool RuleSelected
        {
            get { return SelectedRule != null; }
        }

        public ICommand NewRuleCommand
        {
            get
            {
                return new CommandHelper(NewRule);
            }
        }

        private void NewRule()
        {
            var window = new EditLockWindow();
            window.ShowDialog();

            if (window.Cancelled)
                return;

            var rule = window.GetRule();
            AppState.Instance.AddNewRule(rule);
            rules.Add(new RuleViewModel(rule));
        }

        #endregion

        #region Changes

        public ICollectionView Changes
        {
            get { return changesView.View; }
        }
        
        #endregion
    }
}

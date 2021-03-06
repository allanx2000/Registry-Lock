﻿using Innouvous.Utils;
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

        //Not sure if need different
        private ObservableCollection<RuleViewModel> changes = new ObservableCollection<RuleViewModel>();

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

            ChangeChecker.SetRulesSource(rules, UpdateChanges);
        }

        private void UpdateChanges(List<RuleViewModel> changed)
        {
            changes.Clear();

            foreach (var i in changed)
                changes.Add(i);
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

            switch (window.Result)
            {
                case WindowResult.Cancelled:
                    return;
                case WindowResult.Saved:
                    var rule = window.GetRule();
                    //AppState.Instance.AddNewRule(rule);
                    rules.Add(new RuleViewModel(rule));
                    break;
            }
            
        }

        #endregion

        #region Changes

        public ICollectionView Changes
        {
            get { return changesView.View; }
        }

        private RuleViewModel selectedChange;
        public RuleViewModel SelectedChange
        {
            get { return selectedChange; }
            set
            {
                selectedChange = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ChangeSelected");
            }
        }

        public bool ChangeSelected
        {
            get { return SelectedChange != null; }
        }
        #endregion

        #region Commands
        public ICommand CheckChangesCommand
        {
            get
            {
                return new CommandHelper(CheckChanges);
            }
        }

        private void CheckChanges()
        {
            try
            {
                ChangeChecker.CheckForChanges(false);
            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e);
            }
        }

        #endregion
    }
}

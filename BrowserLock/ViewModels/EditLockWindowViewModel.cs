using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BrowserLock.Models;
using BrowserLock.Checkers;
using System.Collections.ObjectModel;
using Innouvous.Utils.MVVM;
using System.Windows.Input;
using Innouvous.Utils;

namespace BrowserLock.ViewModels
{
    //TODO: Add IsEnabled for combos and paths
    class EditLockWindowViewModel : ViewModel
    {
        private RuleViewModel existing;
        private Window window;

        //Final created rule
        private RuleInfo rule;
        public RuleInfo Rule
        {
            get
            {
                return rule;
            }
        }

        public EditLockWindowViewModel(Window window, RuleViewModel existing = null)
        {
            this.existing = existing;
            this.window = window;

            Result = WindowResult.Cancelled;
        }

        public string Title
        {
            get
            {
                return (existing == null ? "Add" : "Edit") + " Rule";
            }
        }

        #region Checkers
        private ObservableCollection<IChecker> checkers;
        public ObservableCollection<IChecker> Checkers
        {
            get
            {
                if (checkers == null)
                {
                    checkers = new ObservableCollection<IChecker>();

                    foreach (var c in AppState.Instance.CheckersList.OrderBy(x => x.FriendlyName))
                        checkers.Add(c);
                }

                return checkers;
            }
        }

        private IChecker selectedChecker;
        public IChecker SelectedChecker
        {
            get { return selectedChecker; }
            set
            {
                selectedChecker = value;
                RaisePropertyChanged();

                SetSelectedChecker(value);
            }
        }

        private void SetSelectedChecker(IChecker checker)
        {
            Path = null;
            SelectedExt = null;

            RaisePropertyChanged("PathVisible");
            RaisePropertyChanged("ExtVisible");
        }

        #endregion

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private static Visibility GetVisibility(bool isVisible)
        {
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        #region Extensions
        public List<string> Extensions
        {
            get
            {
                List<string> exts = SelectedChecker != null ? SelectedChecker.SupportedExtensions : null;
                return exts;
            }
        }

        private string selectedExt;
        public string SelectedExt
        {
            get { return selectedExt; }
            set
            {
                selectedExt = value;
                RaisePropertyChanged();
            }
        }

        public Visibility ExtVisible
        {
            get
            {
                bool visible = SelectedChecker != null && SelectedChecker.HasExtensions;
                return GetVisibility(visible);
            }
        }
        #endregion

        #region Path

        public Visibility PathVisible
        {
            get
            {
                bool vis = SelectedChecker != null && SelectedChecker.HasPath;
                return GetVisibility(vis);
            }
        }

        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public Visibility DeleteVisibility
        {
            get
            {
                var vis = GetVisibility(existing != null);
                return vis;
            }
        }

        public WindowResult Result { get; private set; }

        #region Commands
        public ICommand CancelCommand
        {
            get
            {
                return new CommandHelper(() =>
                {
                    Result = WindowResult.Cancelled;
                    rule = null;
                    window.Close();
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new CommandHelper(Save);
            }
        }

        public void Save()
        {
            try
            {
                if (String.IsNullOrEmpty(Name))
                    throw new Exception("Name is empty.");

                if (existing != null)
                {
                    rule = existing.Original;

                    if (AppState.Instance.FindExisting(rule.ID, Name) != null)
                        throw new Exception("Name already exists.");

                    rule.Name = Name;
                    AppState.Instance.UpdateRule(rule);
                }
                else
                {
                    if (AppState.Instance.FindExisting(Name) != null)
                        throw new Exception("Name already exists.");
                    else if (SelectedChecker == null)
                        throw new Exception("No Checker selected.");
                    else if (SelectedChecker.HasExtensions && string.IsNullOrEmpty(SelectedExt))
                        throw new Exception("No Extension selected.");
                    else if (SelectedChecker.HasPath && !RegistryUtil.IsValid(Path))
                        throw new Exception("Path is invalid.");

                    rule = SelectedChecker.GetValues(Path, SelectedExt);
                    rule.Name = Name;

                    AppState.Instance.AddNewRule(rule);
                }

                Result = WindowResult.Saved;
                window.Close();
            }
            catch (Exception e)
            {
                MessageBoxFactory.ShowError(e);
            }
        }


        #endregion
    }
}

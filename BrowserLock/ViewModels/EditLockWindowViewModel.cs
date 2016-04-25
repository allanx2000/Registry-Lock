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

namespace BrowserLock.ViewModels
{
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

            Cancelled = true;
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
            if (checker == null)
            {
                //Set to null and hide all
            }
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
                List<string> exts = SelectedChecker != null? SelectedChecker.SupportedExtensions : null;
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

        public Visibility PathVisibility
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
            get {
                var vis = GetVisibility(existing != null);
                return vis;
            }
        }

        public bool Cancelled { get; private set; }


        #region Commands
        #endregion
    }
}

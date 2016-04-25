using Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prop = BrowserLock.Properties.Settings;

namespace BrowserLock.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private ObservableCollection<ExtensionInfoViewModel> current;
        private ObservableCollection<ExtensionInfoViewModel> saved;
        private ObservableCollection<ChangesViewModel> changes;

        public MainWindowViewModel()
        {
            
            //Load from saved and registry
        }

        //Current
        private string cHTTP, cHTTPS;

        //Saved
        private string sHTTP, sHTTPS;

        public string CurrentHTTP
        {
            get { return cHTTP; }
            set
            {
                cHTTP = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CanRestore");
            }
        }
        
        public string CurrentHTTPS
        {
            get { return cHTTPS; }
            set
            {
                cHTTPS = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CanRestore");
            }
        }

        public string SavedHTTP
        {
            get { return sHTTP; }
            set
            {
                sHTTP = value;
                RaisePropertyChanged();
            }
        }

        public string SavedHTTPS
        {
            get { return sHTTPS; }
            set
            {
                sHTTPS = value;
                RaisePropertyChanged();
            }
        }

        private bool isLocked;
        public bool IsLocked
        {
            get { return IsLocked; }
            set
            {
                isLocked = value;
                RaisePropertyChanged();
                RaisePropertyChanged("LockText");

                SetLocked(value);
            }
        }

        public string LockText
        {
            get
            {
                return isLocked ? "Unlock" : "Lock";
            }
        }

        public bool CanRestore
        {
            get
            {
                if (!IsLocked)
                    return false;
                else if (sHTTP == cHTTP && sHTTPS == cHTTPS)
                    return false;
                else 
                    return true;
            }
        }

        public ICommand LockCommand
        {
            get
            {
                return new CommandHelper(Lock);
            }

        }

        private void Lock()
        {

        }

        private void SetLocked(bool locked)
        {
            if (locked)
            {
                Prop.Default.SavedHTTP = SavedHTTP = null;
                Prop.Default.SavedHTTPS = SavedHTTPS = null;
            }
            else
            {
                Prop.Default.SavedHTTP = SavedHTTP = CurrentHTTP;
                Prop.Default.SavedHTTPS = SavedHTTPS = CurrentHTTPS;

            }
            
            RaisePropertyChanged("CanRestore");
        }
    }
}

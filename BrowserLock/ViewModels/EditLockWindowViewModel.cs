using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BrowserLock.Models;

namespace BrowserLock.ViewModels
{
    class EditLockWindowViewModel
    {
        private RuleViewModel existing;
        private Window window;

        public EditLockWindowViewModel(Window window, RuleViewModel existing = null)
        {
            this.existing = existing;
            this.window = window;
        }

        public bool Cancelled { get; private set; }

        public RuleInfo GetRule()
        {
            throw new NotImplementedException();
        }
    }
}

using BrowserLock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserLock.ViewModels
{
    class ChangesViewModel
    {
        private ExtensionInfo current, saved;

        public ChangesViewModel(ExtensionInfo current, ExtensionInfo saved)
        {
            this.current = current;
            this.saved = saved;
        }


    }
}

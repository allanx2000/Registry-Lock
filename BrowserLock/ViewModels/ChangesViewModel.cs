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
        private RuleInfo current, saved;

        public ChangesViewModel(RuleInfo current, RuleInfo saved)
        {
            this.current = current;
            this.saved = saved;
        }


    }
}

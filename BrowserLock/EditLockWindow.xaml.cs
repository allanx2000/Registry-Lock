using BrowserLock.Models;
using BrowserLock.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BrowserLock
{

    /// <summary>
    /// Interaction logic for EditLockWindow.xaml
    /// </summary>
    public partial class EditLockWindow : Window
    {
        private readonly EditLockWindowViewModel vm;

        public EditLockWindow()
        {
            InitializeComponent();

            vm = new EditLockWindowViewModel(this);
            this.DataContext = vm;
        }

        public WindowResult Result { get { return vm.Result; } }

        public RuleInfo GetRule()
        {
            return vm.Rule;
        }
    }
}

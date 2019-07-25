using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp2
{
    class GameStartCommandImpl : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action ExecuteHandler { get; set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (ExecuteHandler != null)
            {
                ExecuteHandler();
            }
        }

    }
}

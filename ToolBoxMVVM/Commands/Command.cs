using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ToolBoxMVVM.Commands
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;


        private readonly Action _Methode;
        private readonly Func<bool> _canExecuteMethode;

        public Command(Action methode, Func<bool> canExecuteMethode = null)
        {
            _Methode = methode;
            _canExecuteMethode = canExecuteMethode;

        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteMethode == null)
            {
                return true;
            }
            return _canExecuteMethode.Invoke();
        }
        

        public void Execute(object parameter)
        {

            _Methode?.Invoke();

        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }
}

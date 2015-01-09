using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LigerShark.TemplatePack.ViewModels
{
    class DelegateCommand : ICommand
    {
        /*
            *  An Action<T> delegate is used to perform the execution since it encapsulates methods
            *  that do not return a value (void function).
            */
        private Action<object> _execute;

        /*
            *  A Predicate<T> delegate is used to determine if the object meets the criteria required
            *  for it to be executed.
            */
        private Predicate<object> _canExecute;


        //  Since we are inheriting ICommand we must implement the CanExecuteChanged event handler
        public event EventHandler CanExecuteChanged;

        /*
            *  This allows us to pass the DelegateCommand class a function or method to perform.
            *  Then it will take the passed object and pass it again to the 2nd constructor.
            */
        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {

        }

        /*
            *  If the DelegateCommand was not passed a Predicate<T> object then when the parameter is
            *  passed to this contructor it also will pass a null Predicate<T>.
            */
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /*
            *  Since we assigned the null Predicate<T> object to the _canExecute field we are now checking
            *  to see if it is null. If it is null then we return true. This means that the method that we
            *  originally passed to this class can be executed. If we originally passed a Predicate<T> object
            *  then it would not have been null and a boolean value for it will be returned. Again this lets us
            *  know if the method can be executed.
            */
        public bool CanExecute(object parameter)
        {
            return _canExecute == null;
        }

        /*
            *  When the button is clicked and we know that the method can be executed then we will call this
            *  Execute method, which will call the method that was passed as the parameter. 
            */
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Crossed_Miner
{
    /// <summary>
    /// Used to attach function pointers to button actions
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields
        readonly Action<Object> _execute;
        readonly Predicate<Object> _canExecute;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<Object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<Object> execute, Predicate<Object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }
        #endregion Constructors

        #region ICommand Members
        /// <summary>
        /// Verifies that the passed in parameters are valid / not null
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Boolean CanExecute(Object parameters)
        {
            return _canExecute == null ? true : _canExecute(parameters);
        }

        /// <summary>
        /// Adds or removes from the Command event quarry
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Pass the parameters to the action
        /// </summary>
        /// <param name="parameters"></param>
        public void Execute(Object parameters)
        {
            _execute(parameters);
        }
        #endregion ICommand Members
    }
}

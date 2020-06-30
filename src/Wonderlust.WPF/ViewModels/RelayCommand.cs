using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Wonderlust.WPF.ViewModels
{
    public class RelayCommand : ICommand
    {
        public static RelayCommand<T> MakeEmpty<T>()
        {
            return new RelayCommand<T>();
        }

        public static RelayCommand<T> Make<T>(Action<T> execute, Predicate<T> canExecute, bool bEnable)
        {
            return new RelayCommand<T>(execute, canExecute, bEnable);
        }
        
        bool bEnable;
        Action execute;
        Func<bool> canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand()
            : this( () => { }, () => false, false)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute, bool bEnable)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.bEnable = bEnable;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            execute.Invoke();
        }

        public bool GetEnable()
        {
            return bEnable;
        }

        public void SetEnable(bool bEnable)
        {
            if (this.bEnable != bEnable)
            {
                this.bEnable = bEnable;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
    }

    public class RelayCommand<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        bool bEnable;
        Action<T> execute;
        Predicate<T> canExecute;

        internal RelayCommand()
            : this((T t) => { }, (T t) => false, false)
        {
        }
        
        internal RelayCommand(Action<T> execute, Predicate<T> canExecute, bool bEnable)
        {
            this.execute = execute;
            this.bEnable = bEnable;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (!bEnable) return false;

            return canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            execute.Invoke((T)parameter);
        }

        public bool GetEnable()
        {
            return bEnable;
        }

        public void SetEnable(bool bEnable)
        {
            if (this.bEnable != bEnable)
            {
                this.bEnable = bEnable;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
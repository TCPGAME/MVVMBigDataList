using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMBigDataList.ViewModel
{
    public class VMBase:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性发生改变时调用该方法发出通知
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void SetAndNotifyIfChanged<T>(string propertyName, ref T oldValue, T newValue)
        {
            if (oldValue == null && newValue == null) return;
            if (oldValue != null && oldValue.Equals(newValue)) return;
            if (newValue != null && newValue.Equals(oldValue)) return;
            oldValue = newValue;
            RaisePropertyChanged(propertyName);
        }


        public class RelayCommand<T> : ICommand where T : class
        {
            #region 字段
            readonly Func<T, Boolean> _canExecute;
            readonly Action<T> _execute;
            #endregion

            #region 构造函数
            public RelayCommand(Action<T> execute)
                : this(execute, null)
            {
            }

            public RelayCommand(Action<T> execute, Func<T, Boolean> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");
                _execute = execute;
                _canExecute = canExecute;
            }
            #endregion

            #region ICommand的成员
            public event EventHandler CanExecuteChanged
            {
                add
                {

                    if (_canExecute != null)
                        CommandManager.RequerySuggested += value;
                }
                remove
                {

                    if (_canExecute != null)
                        CommandManager.RequerySuggested -= value;
                }
            }

            [DebuggerStepThrough]
            public Boolean CanExecute(Object parameter)
            {
                return _canExecute == null ? true : _canExecute((T)parameter);
            }

            public void Execute(Object parameter)
            {
                _execute(parameter as T);
            }
            #endregion
        }

    }
}

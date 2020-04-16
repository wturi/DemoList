using System;
using System.Diagnostics;
using System.Windows.Input;

namespace WFHelper.ViewModels
{
    public class RelayCommand : ICommand
    {
        #region Constants and Fields

        /// <summary>
        /// 如果一个对象可以执行谓词来决定
        /// </summary>
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// 调用命令时执行的动作
        /// </summary>
        private readonly Action<object> execute;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        /// 初始化一个新的实例
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        #endregion Constructors and Destructors

        #region Events

        /// <summary>
        /// 可以执行改变
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;

            remove => CommandManager.RequerySuggested -= value;
        }

        #endregion Events

        #region Implemented Interfaces

        #region ICommand

        /// <summary>
        /// 确定是否可以执行的命令
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        #endregion ICommand

        #endregion Implemented Interfaces
    }
}
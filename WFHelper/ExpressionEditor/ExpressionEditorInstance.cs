using System;
using System.Activities.Presentation.View;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace WFHelper.ExpressionEditor
{
    public class ExpressionEditorInstance : IExpressionEditorInstance
    {
        private readonly TextBox _textBox = new TextBox();

        public bool AcceptsReturn { get; set; }
        public bool AcceptsTab { get; set; }
        public bool HasAggregateFocus => true;

        public ScrollBarVisibility HorizontalScrollBarVisibility { get; set; }
        public Control HostControl => _textBox;
        public int MaxLines { get; set; }
        public int MinLines { get; set; }
        public string Text { get; set; }
        public ScrollBarVisibility VerticalScrollBarVisibility { get; set; }

        public event EventHandler Closing;

        public event EventHandler GotAggregateFocus;

        public event EventHandler LostAggregateFocus;

        public event EventHandler TextChanged;

        public bool CanCompleteWord()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool CanCopy()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool CanCut()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool CanDecreaseFilterLevel()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool CanGlobalIntellisense()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool CanIncreaseFilterLevel()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool CanParameterInfo()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool CanPaste()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool CanQuickInfo()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool CanRedo()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool CanUndo()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public void ClearSelection()
        {
            MessageBox.Show(MethodBase.GetCurrentMethod().Name);
        }

        public void Close()
        {
            MessageBox.Show(MethodBase.GetCurrentMethod().Name);
        }

        public bool CompleteWord()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool Copy()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool Cut()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool DecreaseFilterLevel()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public void Focus()
        {
            MessageBox.Show(MethodBase.GetCurrentMethod().Name);
        }

        public string GetCommittedText()
        {
            return Text;
        }

        public bool GlobalIntellisense()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool IncreaseFilterLevel()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool ParameterInfo()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool Paste()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool QuickInfo()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool Redo()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        public bool Undo()
        {
            return (MessageBox.Show(MethodBase.GetCurrentMethod().Name, "ExpressionEditorInstance", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }
    }
}
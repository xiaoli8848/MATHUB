using MATHUB;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MATHUB_WIN
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalculatorPage : Page
    {
        private ObservableCollection<ComputeResultHistory> resultHistory = new ObservableCollection<ComputeResultHistory>();
        public CalculatorPage()
        {
            this.InitializeComponent();
            //TODO 显示提示
        }
        private void actionButton_Click(object sender, RoutedEventArgs e)
        {
            string res;
            inputBox.Text = inputBox.Text.Replace('÷', '/').Replace('×', '*').Replace('＋', '+').Replace('－', '-');
            try
            {
                res = CalculatorRealization.Compute(inputBox.Text);
            }
            catch (Exception ex)
            {
                InfoBar infoBar = new();
                infoBar.IsOpen = true;
                infoBar.Severity = InfoBarSeverity.Error;
                infoBar.Title = "计算遇到了错误";
                infoBar.Message = ex.Message;
                mainGrid.Children.Add(infoBar);
                res = null;
                return;
            }
            var deleteCommand = new StandardUICommand(StandardUICommandKind.Delete);
            deleteCommand.ExecuteRequested += DeleteCommand_ExecuteRequested;
            resultHistory.Add(new ComputeResultHistory(inputBox.Text, res, deleteCommand));
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            inputBox.Text = "";
        }
        private void DeleteCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            if (args.Parameter != null)
            {
                foreach (var i in resultHistory)
                {
                    if (i.id == (args.Parameter as string))
                    {
                        resultHistory.Remove(i);
                        return;
                    }
                }
            }
            if (HistoryListView.SelectedIndex != -1)
            {
                resultHistory.RemoveAt(HistoryListView.SelectedIndex);
            }
        }
        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            var content = (sender as Button).Content.ToString();
            inputBox.Focus(FocusState.Keyboard);
            inputBox.Text += content;
            if (content.Length >= 3)
            {
                inputBox.Text += "()";
                inputBox.Select(inputBox.Text.Length - 1, 0);
            }
            else
            {
                if (content != "!" && content != "e" && content != "i" && content != "pi")
                {
                    inputBox.Select(inputBox.Text.Length, 0);
                }
            }
        }

        private void NumButton_Click(object sender, RoutedEventArgs e)
        {
            var content = (sender as Button).Content.ToString();
            inputBox.Focus(FocusState.Keyboard);
            inputBox.Text += content;
            inputBox.Select(inputBox.Text.Length, 0);
        }

        private void leftButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rightButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void resultHistory_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            (sender as Grid).Children.ElementAt((sender as Grid).Children.Count - 1).Visibility = Visibility.Visible;
        }

        private void resultHistory_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            (sender as Grid).Children.ElementAt((sender as Grid).Children.Count - 1).Visibility = Visibility.Collapsed;
        }

        private void inputGrid_DropCompleted(UIElement sender, DropCompletedEventArgs args)
        {

        }

        private async void inputGrid_Drop(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            DragOperationDeferral def = e.GetDeferral();
            string s = await e.DataView.GetTextAsync();
            inputBox.Text += s;
            def.Complete();
        }

        private void HistoryListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            StringBuilder items = new StringBuilder();
            foreach (ComputeResultHistory item in e.Items)
            {
                if (items.Length > 0) { items.AppendLine(); }
                items.Append(item.expression);
            }
            // Set the content of the DataPackage
            e.Data.SetText(items.ToString());

            e.Data.RequestedOperation = DataPackageOperation.Copy;
        }
    }
}

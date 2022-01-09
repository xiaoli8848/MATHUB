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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public CalculatorPage()
        {
            this.InitializeComponent();
        }
        private void actionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                resultText.Text = CalculatorRealization.Compute(inputBox.Text);
            }
            catch (Exception ex)
            {
                InfoBar infoBar = new();
                infoBar.IsOpen = true;
                infoBar.Severity = InfoBarSeverity.Error;
                infoBar.Title = "计算遇到了错误";
                infoBar.Message = ex.Message;
                mainGrid.Children.Add(infoBar);
            }
        }

        private void NavLinksList_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            switch (args.InvokedItem)
            {
                case "求值":
                    Binding binding = new();
                    binding.Path = new PropertyPath("equalResult");
                    resultText.SetBinding(TextBlock.TextProperty, binding);
                    break;
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

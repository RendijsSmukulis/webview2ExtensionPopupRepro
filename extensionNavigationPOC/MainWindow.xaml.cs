using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace extensionNavigationPOC
{
    using System.Diagnostics;
    using System.Reflection;
    using Microsoft.Web.WebView2.Core;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var envOptions = new CoreWebView2EnvironmentOptions
            {
                AreBrowserExtensionsEnabled = true
            };

            var env = CoreWebView2Environment.CreateAsync(options: envOptions).Result;

            this.WebView2.CoreWebView2InitializationCompleted += WebView2OnCoreWebView2InitializationCompleted;
            this.WebView2.EnsureCoreWebView2Async(env);
        }

        private async void WebView2OnCoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            // Set up new-window-requested handler
            this.WebView2.CoreWebView2.NewWindowRequested += CoreWebView2OnNewWindowRequested;

            // Register extension
            var exePath = Assembly.GetExecutingAssembly().Location;
            var currentDir = System.IO.Path.GetDirectoryName(exePath);
            var extensionPath = System.IO.Path.Combine(currentDir, "extensions\\popups");
            var ext = await this.WebView2.CoreWebView2.Profile.AddBrowserExtensionAsync(
                extensionPath);
        }

        private void CoreWebView2OnNewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            // Claim we've handled this - no new window should open
            e.Handled = true;
            Debug.WriteLine($"+++> Hit 'new window requested'");
        }
    }
}

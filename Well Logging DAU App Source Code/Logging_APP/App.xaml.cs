using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions;
using System.Web.Services.Configuration;
using System.Security.Permissions;
using System.Reflection;

namespace Logging_App
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            RegisterSoapExtension();
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private static void RegisterSoapExtension()
        {
            WebServicesSection wss = WebServicesSection.Current;
            FieldInfo readOnlyField = typeof(ConfigurationElementCollection).GetField("bReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
            readOnlyField.SetValue(wss.SoapExtensionTypes, false);
            wss.SoapExtensionTypes.Add(new SoapExtensionTypeElement(typeof(Logging_App.SoapWatchExtension), 1, PriorityGroup.High));
            MethodInfo setReadOnlyMethod = typeof(ConfigurationElement).GetMethod("SetReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
            setReadOnlyMethod.Invoke(wss.SoapExtensionTypes, null);
        }

        protected override void OnStartup(StartupEventArgs e)
        {

        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.ExceptionObject is System.Exception)
                {
                    HandleException((System.Exception)e.ExceptionObject);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public static void HandleException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            var e = ex;
            while (e != null)
            {
                if (!string.IsNullOrEmpty(e.Message))
                    sb.Append(e.Message + "\r\n");
                e = e.InnerException;
            }
            MessageBox.Show(sb.ToString(), "错误!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // MessageBox.Show(e.Exception.Message);
            try
            {
                HandleException(e.Exception);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}

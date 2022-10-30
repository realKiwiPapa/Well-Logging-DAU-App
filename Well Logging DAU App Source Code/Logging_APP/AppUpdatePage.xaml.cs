using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Logging_App
{
    /// <summary>
    /// AppUpdatePage.xaml 的交互逻辑
    /// </summary>
    public partial class AppUpdatePage : Page
    {
        public AppUpdatePage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Multiselect = false;
            ofd.CheckFileExists = true;
            ofd.Filter = "安装包(*.msi)|*.msi";
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            var tasks = new Utility.DataCollection<UploadController.UploadTask>();
            tasks.Add(new UploadController.UploadTask { FullName = ofd.FileName });
            new PackUpload().BeginUpload(tasks);
#if !DEBUG
            UpdateDownload.Update();
#endif
        }

    }
}

using HouseOffice.WPF.ViewModels;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace HouseOffice.WPF.Views
{
    public partial class AddUserRequestView : UserControl
    {
        public AddUserRequestView()
        {
            InitializeComponent();
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var openFileDialog = new OpenFileDialog
            {
                Filter = "Все файлы (*.*)|*.*",
                Title = "Выберите файл"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var safefileName = openFileDialog.SafeFileName;
                var fileName = openFileDialog.FileName;
                switch (button.Tag.ToString())
                {
                    case "LandRegistration":
                        LandRegistrationFileName.Text = safefileName;
                        ((AddUserRequestViewModel)DataContext).LandRegistrationFile = fileName;
                        break;
                    case "Passport":
                        PassportFileName.Text = safefileName;
                        ((AddUserRequestViewModel)DataContext).PassportFile = fileName;
                        break;
                    case "Proxy":
                        ProxyFileName.Text = safefileName;
                        ((AddUserRequestViewModel)DataContext).ProxyFile = fileName;
                        break;
                }
            }
        }
    }
}

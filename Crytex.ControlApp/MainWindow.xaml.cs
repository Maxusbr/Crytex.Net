using System.Windows;
using Microsoft.Practices.Unity;

namespace Crytex.ControlApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var window= UnityConfig.GetConfiguredContainer().Resolve<CreateVm>();
     
            window.ShowDialog();
        }
    }
}

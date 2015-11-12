using System;
using System.Windows;
using Crytex.Service.IService;
using Crytex.Model.Models;

namespace Crytex.ControlApp
{
    /// <summary>
    /// Логика взаимодействия для CreateVm.xaml
    /// </summary>
    public partial class CreateVm : Window
    {
        public CreateVm()
        {
            InitializeComponent();
        }

        private void btnCreateVm_Click(object sender, RoutedEventArgs e)
        {
            var name = txtName.Text;
            var cpu = Convert.ToInt32(txtCore.Text);
            var ram = Convert.ToInt32(txtRam.Text);
            var hdd = Convert.ToInt32(txtHdd.Text);
        }
    }
}

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
        private ITaskVmService _taskVmBackGroundService;
    
        public CreateVm(ITaskVmService taskVmBackGroundService)
        {
            InitializeComponent();
            _taskVmBackGroundService = taskVmBackGroundService;
        }

        private void btnCreateVm_Click(object sender, RoutedEventArgs e)
        {
            var name = txtName.Text;
            var cpu = Convert.ToInt32(txtCore.Text);
            var ram = Convert.ToInt32(txtRam.Text);
            var hdd = Convert.ToInt32(txtHdd.Text);
            _taskVmBackGroundService.CreateVm(new CreateVmTask()
            {
                Cpu = cpu,
                Hdd = hdd,
                Name=name,
                Ram = ram,
                UserId = ""
            });
        }
    }
}

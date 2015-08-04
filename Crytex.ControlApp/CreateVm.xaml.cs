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
using System.Windows.Shapes;
using Project.Service.IService;
using Project.Service.Model;

namespace Crytex.ControlApp
{
    /// <summary>
    /// Логика взаимодействия для CreateVm.xaml
    /// </summary>
    public partial class CreateVm : Window
    {
        private ITaskVmBackGroundService _taskVmBackGroundService;
    
        public CreateVm(ITaskVmBackGroundService taskVmBackGroundService)
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
            _taskVmBackGroundService.CreateVm(new CreateVmOption()
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

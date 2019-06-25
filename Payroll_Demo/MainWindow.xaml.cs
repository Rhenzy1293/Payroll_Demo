using MahApps.Metro.Controls;
using Payroll_Demo.Module;
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

namespace Payroll_Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_ShowMenu_Click(object sender, RoutedEventArgs e)
        {
            DrawerHost1.IsTopDrawerOpen = true;
        }

        private void menuItem_UserMaintenance_Click(object sender, RoutedEventArgs e)
        {
            EmployeeMaintenance uc = new EmployeeMaintenance();
            MainContent.Content = uc;
            DrawerHost1.IsTopDrawerOpen = false;
        }

        private void menuItem_PayrollEntries_Click(object sender, RoutedEventArgs e)
        {
            Payroll_Transactions pt = new Payroll_Transactions();
            MainContent.Content = pt;
            DrawerHost1.IsTopDrawerOpen = false;
        }
    }
}

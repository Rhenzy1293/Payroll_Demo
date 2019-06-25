using System;
using System.Collections.Generic;
using System.Data;
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

namespace Payroll_Demo.Module
{
    /// <summary>
    /// Interaction logic for EmployeeMaintenance.xaml
    /// </summary>
    public partial class EmployeeMaintenance : UserControl
    {
        public EmployeeMaintenance()
        {
            InitializeComponent();
        }

        bool IsEditing = false;

        private void Button_New_Click(object sender, RoutedEventArgs e)
        {
            TextBox_EmployeeCode.Clear();
            TextBox_Firstname.Clear();
            TextBox_MiddleName.Clear();
            TextBox_LastName.Clear();
            TextBox_Address.Clear();
            TextBox_Rate.Clear();

            IsEditing = false;
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            string query;
            if (IsEditing == true)
            {
                //currently editing record

                query = $@"
UPDATE EmployeeInformation
SET s_first_name = '{TextBox_Firstname.Text}'
,s_middle_name = '{TextBox_MiddleName.Text}'
,s_last_name = '{TextBox_LastName.Text}'
,s_address = '{TextBox_Address.Text}'
,d_rate_per_month = '{TextBox_Rate.Text}'

WHERE s_employee_id = '{TextBox_EmployeeCode.Text}'
";
                DatabaseConnection.Connection1(query);

                MessageBox.Show("The record has been updated!");

            }
            else
            {
                //inserts new record

               query  = $@"
INSERT INTO EmployeeInformation(s_employee_id
,s_first_name
,s_middle_name
,s_last_name
,s_address
,d_rate_per_month)


VALUES('{TextBox_EmployeeCode.Text}'
,'{TextBox_Firstname.Text}'
,'{TextBox_MiddleName.Text}'
,'{TextBox_LastName.Text}'
,'{TextBox_Address.Text}'
,'{TextBox_Rate.Text}')
";
                DatabaseConnection.Connection1(query);

                MessageBox.Show("The process has been performed!");
            }


        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            string query;
            DataSet ds;
            DataRow drow;

            query = $@"
SELECT s_employee_id
,s_first_name
,s_middle_name
,s_last_name
,s_address
,d_rate_per_month

from EmployeeInformation
WHERE s_employee_id = '{TextBox_EmployeeCode.Text}'
";
            ds = DatabaseConnection.Connection1(query);

            try
            {
                drow = ds.Tables[0].Rows[0];

                TextBox_EmployeeCode.Text = drow.ItemArray.GetValue(0).ToString();
                TextBox_Firstname.Text = drow.ItemArray.GetValue(1).ToString();
                TextBox_MiddleName.Text = drow.ItemArray.GetValue(2).ToString();
                TextBox_LastName.Text = drow.ItemArray.GetValue(3).ToString();
                TextBox_Address.Text = drow.ItemArray.GetValue(4).ToString();
                TextBox_Rate.Text = drow.ItemArray.GetValue(5).ToString();

                IsEditing = true;
            }
            catch (Exception)
            {
                MessageBox.Show("The employee code you've entered does not exist!");
                IsEditing = false;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Payroll_Transactions.xaml
    /// </summary>
    public partial class Payroll_Transactions : UserControl
    {

        ObservableCollection<AdditionalEntries> source = new ObservableCollection<AdditionalEntries>();
        Transactiontypes Txntype = Transactiontypes.New;

        private enum Transactiontypes
        {
            New,
            Existing
        }

        public Payroll_Transactions()
        {
            InitializeComponent();
            source.Add(new AdditionalEntries { Entry = "D", Amount = 299.75, Remarks = "Deduction sa uniform" });



            Datagrid_Entries.ItemsSource = source;
            source.Add(new AdditionalEntries { Entry = "A", Amount = 500.00, Remarks = "Overtime" });


        }


        private class AdditionalEntries
        {
            public string Entry { get; set; }
            public double Amount { get; set; }
            public string Remarks { get; set; }
        }

        private void Button_EmployeeSearch_Click(object sender, RoutedEventArgs e)
        {
            string query;
            DataSet ds;
            DataRow drow;

            query = $@"
select s_first_name + ' ' + s_last_name as s_employee_name
,d_rate_per_month
from EmployeeInformation

where s_employee_id = '{TextBox_EmployeeCode.Text}'
";
            ds = DatabaseConnection.Connection1(query);

            try
            {
                drow = ds.Tables[0].Rows[0];

                TextBox_Employee.Text = drow.ItemArray.GetValue(0).ToString();
                TextBox_BasicPay.Text = drow.ItemArray.GetValue(1).ToString();

                source.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("The employee code does not exist!");
            }

        }

        private void TextBox_NoOfDays_LostFocus(object sender, RoutedEventArgs e)
        {
            double d_basic_pay, d_cutoff_pay;

            d_basic_pay = double.Parse(TextBox_BasicPay.Text);
            d_cutoff_pay = d_basic_pay / 2;

            TextBox_CutoffPay.Text = d_cutoff_pay.ToString();
        }


        private double get_rate_per_minute(double CutOffPay, double NoOfDays)
        {
            double d_overall_minute = (NoOfDays * 8) * 60;
            double d_rate_per_minute = CutOffPay / d_overall_minute;

            return d_rate_per_minute;
        }

        private void TextBox_AbsencesEntry_LostFocus(object sender, RoutedEventArgs e)
        {
            double d_rate_min = get_rate_per_minute(double.Parse(TextBox_CutoffPay.Text), double.Parse(TextBox_NoOfDays.Text));

            double d_days_absent = double.Parse(TextBox_AbsencesEntry.Text);

            double absent_amount = ((d_rate_min * 60) * 8) * d_days_absent;

            TextBox_AbsencesAmount.Text = absent_amount.ToString("0.00");
        }

        private void TextBox_LateEntry_LostFocus(object sender, RoutedEventArgs e)
        {
            double d_late_undertime = double.Parse(TextBox_LateEntry.Text);
            double d_rate_min = get_rate_per_minute(double.Parse(TextBox_CutoffPay.Text), double.Parse(TextBox_NoOfDays.Text));

            double d_late_amount = d_late_undertime * d_rate_min;

            TextBox_LateAmount.Text = d_late_amount.ToString("0.00");

        }

        private void TextBox_Adjustment_LostFocus(object sender, RoutedEventArgs e)
        {
            double d_cutoff_pay, d_absences_deduction, d_late_deduction
                , d_allowance_amount, d_adjustment_amount;

            d_cutoff_pay = double.Parse(TextBox_CutoffPay.Text);
            d_absences_deduction = double.Parse(TextBox_AbsencesAmount.Text);
            d_late_deduction = double.Parse(TextBox_LateAmount.Text);
            d_allowance_amount = double.Parse(TextBox_Allowance.Text);
            d_adjustment_amount = double.Parse(TextBox_Adjustment.Text);

            double d_gross = ((d_cutoff_pay - d_absences_deduction - d_late_deduction) + d_allowance_amount) + d_adjustment_amount;


            Label_Gross.Text = d_gross.ToString("0.00");

        }

        private void Label_Refresh_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int index, total;

            double d_overall_sum = 0;
            double d_current_amount;

            total = source.Count;

            AdditionalEntries currentRow;

            for (index = 0; index < total - 1; index++)
            {
                currentRow = source[index];

                if (currentRow.Entry.Equals("A"))
                {
                    d_current_amount = currentRow.Amount;
                }
                else
                {
                    d_current_amount = currentRow.Amount * -1;
                }

                d_overall_sum = d_overall_sum + d_current_amount;
            }

            double d_gross = double.Parse(Label_Gross.Text);
            double d_net = d_gross + d_overall_sum;

            Label_Net_Amount.Text = d_net.ToString("0.00");
        }


        private string GetReferenceNumber()
        {
            string s_reference_number = "PR00000001";

            return s_reference_number;
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            string query;

            if (Txntype == Transactiontypes.New)
            {
                //create a new record
                string s_transaction_number = GetReferenceNumber();

                query = $@"
INSERT INTO payroll_header(s_reference_number
,s_employee_code
,dt_period_from
,dt_period_to
,d_no_of_days
,d_cutoff_pay
,d_absences
,d_late_undertime
,d_allowance
,d_adjustment
,d_gross_pay
,d_net_pay)

VALUES('{s_transaction_number}'
,'{TextBox_EmployeeCode.Text}'
,'{DatePicker_PeriodFrom.Text}'
,'{DatePicker_PeriodTo.Text}'
,'{TextBox_NoOfDays.Text}'
,'{TextBox_CutoffPay.Text}'
,'{TextBox_LateEntry.Text}'
,'{TextBox_LateEntry.Text}'
,'{TextBox_Allowance.Text}'
,'{TextBox_Adjustment.Text}'
,'{Label_Gross.Text}'
,'{Label_Net_Amount.Text}')
";

                DatabaseConnection.Connection1(query);

                //for the payroll_detail
                int index;
                int total;

                total = source.Count;
                AdditionalEntries currentRow;

                for (index = 0; index < total; index++)
                {
                    currentRow = source[index];

                    query = $@"
insert into payroll_detail(s_reference_number
,s_entry_type
,d_amount
,s_remarks)

values('{s_transaction_number}'
,'{currentRow.Entry}'
,'{currentRow.Amount}'
,'{currentRow.Remarks}')
";
                    DatabaseConnection.Connection1(query);
                }

            }
            else
            {
                //update new record
            }
        }

        private void Button_Post_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Payroll_Demo
{
    public class DatabaseConnection
    {
        static SqlConnection con = new SqlConnection();
        static SqlCommand cmd = new SqlCommand();
        static SqlDataAdapter da = new SqlDataAdapter();
        static DataSet ds;

        static string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Michael\Desktop\pagaaral\NET\Payroll_Demo\Payroll_Demo\Payroll_Demo\SystemDatabase.mdf;Integrated Security=True";

        public static DataSet Connection1(string query)
        {
            try
            {
                con = new SqlConnection(ConnectionString);
                cmd = new SqlCommand(query, con);
                con.Open();

                ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Table1");

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return ds;
        }

    }
}

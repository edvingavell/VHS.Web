using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormVHS.Entity;

namespace WindowsFormVHS
{
    public partial class FormAddStatus : Form
    {
        public FormAddStatus()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            bool checkRegNo = Misc.CheckRegNo(textBox1.Text);
            bool checkBatteryStatus = Misc.CheckBatteryStatus(textBox2.Text);
            bool checkGPS = Misc.CheckGPS(textBox3.Text);
            bool checkTripMeter = Misc.CheckTripMeter(textBox4.Text);
            bool checkLockStatus = Misc.CheckLockStatus(textBox5.Text);
            bool checkAlarmStatus = Misc.CheckAlarmStatus(textBox6.Text);
            bool checkTirePressures = Misc.CheckTirePressures(textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text);

            if (!checkRegNo)
            {
                MessageBox.Show("Regnumber", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
            else if (!checkBatteryStatus)
            {
                MessageBox.Show("Battery status", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkGPS)
            {
                MessageBox.Show("GPS", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkTripMeter)
            {
                MessageBox.Show("Tripmeter", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkLockStatus)
            {
                MessageBox.Show("Lock status", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkAlarmStatus)
            {
                MessageBox.Show("Alarm status", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkTirePressures)
            {
                MessageBox.Show("Tirepressures", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var tirePressures = new List<double>() { double.Parse(textBox7.Text), double.Parse(textBox8.Text), double.Parse(textBox9.Text), double.Parse(textBox10.Text) };

                SqlConnection myConnection = new SqlConnection(DBstuffs.ConnectionString);

                myConnection.Open();

                SqlCommand cmd = new SqlCommand("dbo.sPost_Status", myConnection) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.Add(new SqlParameter("@RegistrationNumber", SqlDbType.NVarChar, 50) { Value = textBox1.Text });
                cmd.Parameters.Add(new SqlParameter("@BatteryStatus", SqlDbType.Int) { Value = textBox2.Text });
                cmd.Parameters.Add(new SqlParameter("@GPS", SqlDbType.NVarChar, 255) { Value = textBox3.Text });
                cmd.Parameters.Add(new SqlParameter("@TripMeter", SqlDbType.Float) { Value = textBox4.Text });
                cmd.Parameters.Add(new SqlParameter("@LockStatus", SqlDbType.Int) { Value = textBox5.Text });
                cmd.Parameters.Add(new SqlParameter("@AlarmStatus", SqlDbType.Int) { Value = textBox6.Text });
                cmd.Parameters.Add(new SqlParameter("@TirePressures", SqlDbType.NVarChar, 50) { Value = JsonConvert.SerializeObject(tirePressures) });
                cmd.Parameters.Add(new SqlParameter("@StatusId", SqlDbType.UniqueIdentifier)
                { Value = null, Direction = ParameterDirection.Output });

                cmd.ExecuteNonQuery();

                var userId = new Guid(cmd.Parameters[7].Value.ToString());

                myConnection.Close();

                MessageBox.Show($"Status added! with Guid: \n{userId}", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void label7_Click_1(object sender, EventArgs e)
        {

        }

        private void FormAddStatus_Load(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormVHS
{
    public partial class Status : Form
    {
        public Status()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form formAddStatus = new FormAddStatus();
            formAddStatus.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form formUpdateStatus = new FormUpdateStatus();
            formUpdateStatus.Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace filecheck
{
    public partial class loginform : Form
    {
        public loginform()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                comm.idname = textBox1.Text;
                comm.idpwd = textBox2.Text;
                comm.connstr = "initial catalog="+comm.vinid+";datasource=BABT-SQL01;connect Timeout=20"+"user id="+comm.idname+";"+"password="+comm.idpwd+";";
                this.Close();
                Form1.ActiveForm.Enabled = true;
            }
            else
            {
                MessageBox.Show("用户名不能为空");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

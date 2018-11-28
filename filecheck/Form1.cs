using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;


namespace filecheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        loginform lf1;
        string filename;
        StreamReader sr;
        StreamWriter sw;
        FileStream ws;
        String line;
        //public string  idname, idpwd;
        int ziduanshu=0,wenjianshu=0;
        bool wenjian = false;
        SqlConnection myconn;
        //myconn=new SqlConnectionStringBuilder();

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            pOpenFileDialog.Filter = "所有文件|*.*";//若打开指定类型的文件只需修改Filter，如打开txt文件，改为*.txt即可
            pOpenFileDialog.Multiselect = false;
            pOpenFileDialog.Title = "打开文件";
            if (pOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                filename = pOpenFileDialog.FileName;
                comm.wenjianlujing = Path.GetDirectoryName(filename)+"\\";
                label1.Text = filename;
                sr = new StreamReader(filename, Encoding.Default  );

                

            }



        }

        private void button2_Click(object sender, EventArgs e) //程序退出，关文件流

      
        {
            if (sr!= null) sr.Close();
            if (ws != null)
            {
               
                ws.Close();
            }
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)//读文件第一行取字段名，读文件第2行，确定拆分文件名，关文件，再开再读一行
        {
            string hang;
            int aa;

            if ((line = sr.ReadLine()) != null)
            {

                hang = line.Trim();
                switch (comm.fenge)
                {
                    case " ":
                        hang = hang.Replace(" ", ",");
                        //comm.biaotou = hang;
                        break;
                    case "tab":
                        hang = hang.Replace("\t", ",");
                        //comm.biaotou = hang;
                        break;
                    default:
                        //comm.biaotou = hang;
                        break;
                }
                string[] ziduan = hang.Split(new string[] { "," }, StringSplitOptions.None);
                //comm.biaotou = hang;
                comm.biaotou = ziduan[1] + "," + ziduan[40] + "," + ziduan[45] + "," + ziduan[46] + "," + ziduan[49] + "," + ziduan[50] + "," + ziduan[51] + "," + ziduan[52] + "," + ziduan[126];

                aa = ziduan.Length ;
                ziduanshu = aa;
                label3.Text = aa.ToString();
                label4.Visible = true;
                listView1.Visible = true;
                //this.listView1.Columns.Add("序号", 120, HorizontalAlignment.Center);
                //this.listView1.Columns.Add("字段名", 600, HorizontalAlignment.Center);
                this.listView1.BeginUpdate();

                for (int i=0;i<aa;i++)
                    {
                    //listBox1.Items.Add(ziduan[i]);
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text ="No"+ (i+1) ;
                    //lvi.SubItems.Add(i);
                    lvi.SubItems.Add(ziduan[i]);
                    this.listView1.Items.Add(lvi);
                }
                this.listView1.EndUpdate();

            }
            if((line=sr.ReadLine())!=null )
            {
                hang = line.Trim();
                switch (comm.fenge)
                {
                    case " ":
                        hang = hang.Replace(" ", ",");
                        break;
                    case "tab":
                        hang = hang.Replace("\t", ",");
                        break;
                    default:
                        break;
                }

                string[] ziduan = hang.Split(new string[] { "," }, StringSplitOptions.None);
                if (!wenjian)
                {
                    comm.vin = ziduan[0];
                    comm.dt = ziduan[1].Substring(0,10).Replace("/", "-");
                    comm.wenjianming = comm.vin +"-"+ comm.dt +"-"+ Convert.ToString(wenjianshu)+".csv";
                    wenjian = true;
                    sr.Close();
                    sr = new StreamReader(filename, Encoding.Default);
                    line = sr.ReadLine();

                }
                


            }
                  
        }

        private void button4_Click(object sender, EventArgs e) //检测数据，丢弃问题记录，写分隔文件
        {
            string hang,hang2;
            int sumline = 0;
            int aa=0,bb=0,cc=0;        //总行数，字段不用的，含空字段的
            string[] ziduan;   // = hang.Split(new string[] { "," }, StringSplitOptions.None);
           
            while ((line=sr.ReadLine()) != null)
            {
                sumline++;
                hang2 = "";
                hang = line.Trim();
                switch (comm.fenge)
                {
                    case " ":
                        hang = hang.Replace(" ", ",");
                        break;
                    case "tab":
                        hang = hang.Replace("\t", ",");
                        break;
                    default:
                        break;
                }

                ziduan = hang.Split(new string[] {"," }, StringSplitOptions.None);
                //if(!wenjian )
                //{
                //    comm.vin = ziduan[0];
                //    comm.dt = ziduan[1].Substring(0, 10).Replace("/", "-");
                //    comm.wenjianming =comm.vin+"-"+comm.dt+"-"+Convert.ToString(wenjianshu) + ".csv";
                //    wenjian = true;
                //}
                aa = ziduan.Length;
                //hang2 = hang.Remove(41) + hang.Substring(42);
                hang2 = ziduan[1] + "," + ziduan[40] + "," + ziduan[45] + "," + ziduan[46] + "," + ziduan[49] + "," + ziduan[50] + "," + ziduan[51] + "," + ziduan[52] + "," + ziduan[126];

                if (ziduanshu !=aa)
                {
                    bb++;
                    listBox1.Items.Add(sumline + 1);

                }
                else
                {
                    cc++;
                    if((cc<comm.jilushu) && (ws!=null) )
                    {
                        
                        sw.WriteLine(hang2);

                    }
                    else if ((cc==comm.jilushu) && (ws!=null) )
                    {
                        cc = 0;
                        wenjianshu++;
                        comm.wenjianming = comm.vin +"-"+ comm.dt +"-"+ Convert.ToString(wenjianshu)+".csv";
                        sw.WriteLine(hang2);
                        sw.Flush();
                        sw.Close();
                        ws = new FileStream(comm.wenjianlujing + comm.wenjianming, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        sw = new StreamWriter(ws, Encoding.GetEncoding("GB2312"));
                        sw.WriteLine(comm.biaotou);
                        //sw.WriteLine(hang);
                        listBox2.Items.Add(wenjianshu );


                        //ws =
                    }
                        //for (int i = 0; i < aa; i++)
                    //{
                    //    if (ziduan[i] == null)
                    //    {
                    //        cc++;
                    //        listBox2.Items.Add(sumline + 1);
                    //    }

                    //}bool exists = ((IList)strArr).Contains("a");
                   // bool youkong = ((IList)ziduan).Contains("");

                

                }
                


            //label6.Text = sumline.ToString();
            }
            label10.Text = bb.ToString();
            label12.Text = cc.ToString();
            label6.Text = sumline.ToString();
            //ws.Flush();
            //ws.Close();
            sw.Flush();
            sw.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“_babt_plandtDataSet._test188_1601”中。您可以根据需要移动或删除它。
            //this.test188_1601TableAdapter.Fill(this._babt_plandtDataSet._test188_1601);
            checkedListBox1.SetItemChecked(0, true);
            comm.fenge = ",";

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                lf1 = new loginform();
                lf1.Show();
                this.Enabled = false;

                //myconn =
            }
            catch
            {


            }
        }

       // private void CheckedListB
        private void CheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.GetItemChecked(0)) comm.fenge = ",";
            if (checkedListBox1.GetItemChecked(1)) comm.fenge = " ";
            if (checkedListBox1.GetItemChecked(2)) comm.fenge = "tab";

        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if(i!=e.Index)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
                

                //if (checkedListBox1.GetItemChecked(i))
                //{
                //    MessageBox.Show(checkedListBox1.GetItemText(checkedListBox1.Items[i]));
                //}
            }
            //checkedListBox1.SetItemChecked( e.Index , true);

            if (checkedListBox1.GetItemChecked(0)) comm.fenge = ",";
            if (checkedListBox1.GetItemChecked(1)) comm.fenge = " ";
            if (checkedListBox1.GetItemChecked(2)) comm.fenge = "tab";

        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool jieguo = false;
            try
            {
                comm.jilushu=Convert.ToInt32(textBox1.Text);
                jieguo = true;
            }
            catch
            {
                MessageBox.Show("最大记录数必须是数值，请修改后再试");
                jieguo = false;
            }
            if (jieguo)

            {
                textBox2.Text = comm.wenjianlujing+comm.wenjianming;
                if (ws == null)
                {

                ws = new FileStream(comm.wenjianlujing+comm.wenjianming , FileMode.OpenOrCreate, FileAccess.ReadWrite);
                sw = new StreamWriter(ws,Encoding.GetEncoding("GB2312"));
                    sw.WriteLine(comm.biaotou);

                }
            }
        }
    }
}

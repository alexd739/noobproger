using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FCM_GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public event EventHandler<int> ActivateAnalysis;

        public void SetText(double[,] text)
        {
            string toshow=null;
            for(int i =0;i<text.GetLength(0);i++)
            {
                for(int j=0;j<text.GetLength(1);j++)
                {
                    toshow += text[i,j].ToString();
                    toshow += "\n";
                }
                
            }
            richTextBox1.Text = toshow;
        }

        public void SetChart(double[,] X,double[,] Y)
        {
            chart1.DataBindTable(Y); 
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (ActivateAnalysis != null)
                ActivateAnalysis(this,int.Parse(textBox1.Text));
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

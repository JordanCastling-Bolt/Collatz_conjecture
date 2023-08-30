using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace StockTicker
{
    public partial class Form1 : Form
    {
        Thread thread;
        int listCount = 0; // New variable to count the list items

        public Form1()
        {
            InitializeComponent();
            chart1.Series["MH 50"].ChartType = SeriesChartType.Line;
        }

        private void btnMH50_Click(object sender, EventArgs e)
        {
            if (Int32.TryParse(textBox1.Text, out int n) && n > 0)
            {
                InitializeUI();
                thread = new Thread(() => CalculateCollatz(n));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Please enter a number greater than zero.");
            }
        }

        private void CalculateCollatz(int n)
        {
            listCount = 0; // Reset list count

            while (n > 1)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    chart1.Series["MH 50"].Points.AddY(n);
                    textBox2.AppendText(n.ToString() + Environment.NewLine);
                });

                listCount++; // Increment list count

                if (n % 2 == 0)
                {
                    n /= 2;
                }
                else
                {
                    n = 3 * n + 1;
                }
            }

            this.Invoke((MethodInvoker)delegate
            {
                chart1.Series["MH 50"].Points.AddY(1);
                textBox2.AppendText("1" + Environment.NewLine);
                ListSize.Text = listCount.ToString(); // Display the list count in the text box
            });
        }

        private void btnClearOutput_Click(object sender, EventArgs e)
        {
            InitializeUI();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
                Close();
            }
            Close();
        }

        private void InitializeUI()
        {
            textBox2.Text = "";
            chart1.Series["MH 50"].Points.Clear();
            ListSize.Text = "0"; // Reset list count in the UI
            listCount = 0; // Reset list count
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
            }
        }
    }
}

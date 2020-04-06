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
using CellToolDK;
using System.IO;

namespace Plot_Profile
{
    public partial class MyForm : Form
    {
        private TifFileInfo fi;
        public MyForm(TifFileInfo fi)
        {
            this.fi = fi;
            InitializeComponent();
            this.Load += this.myForm_onLoading;            
        }
        private void myForm_onLoading(object sender, EventArgs e)
        {
            this.comboBox_Dimention.SelectedIndex = 1;
            this.comboBox_Type.SelectedIndex = 1;

            this.button_Refresh.Click += this.button_Refresh_Click;
            this.button_Save.Click += this.button_Save_Clicked;
            this.comboBox_Dimention.SelectedIndexChanged += this.button_Refresh_Click;
            this.comboBox_Type.SelectedIndexChanged += this.button_Refresh_Click;
        }
        private void button_Refresh_Click(object sender, EventArgs e)
        {
            Data.Dimention dim = (Data.Dimention) this.comboBox_Dimention.SelectedIndex;
            Data.ProfileType type = (Data.ProfileType)this.comboBox_Type.SelectedIndex;
            int[] frames;

            if(!Data.GetFrtames(this.textBox_Frames.Text, out frames) && this.textBox_Frames.Text != "")
            {
                MessageBox.Show("Incorrect frames!");
                return;
            }

            chart1.SuspendLayout();
            chart1.Series.Clear();

            if (fi.image16bitFilter == null) fi.image16bitFilter = fi.image16bit;

            foreach (int frame in frames)
                if (frame < fi.image16bitFilter.Length)
                {
                    chart1.Series.Add(Data.AddSeries(frame, fi.image16bitFilter[frame], dim, type));
                }
                else
                {
                    MessageBox.Show("Selected frames out of range!");
                    return;
                }

            chart1.ResumeLayout();
            chart1.Invalidate();
            chart1.Update();           
        }
        private void button_Save_Clicked(object sender, EventArgs e)
        {
            if (chart1.Series.Count == 0) return;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "TAB delimited text file (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.CheckFileExists = false;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bool writeTime;
                    int i;

                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                    {
                        switch ((Data.Dimention)comboBox_Dimention.SelectedIndex)
                        {
                            case Data.Dimention.X:
                                sw.Write("X:\t");
                                break;
                            case Data.Dimention.Y:
                                sw.Write("Y:\t");
                                break;
                        }
                        

                        foreach (var ser in chart1.Series)
                            sw.Write(ser.Name + "\t");

                        sw.WriteLine();

                        if (chart1.Series.Count > 1)

                            for (i = 0; i < chart1.Series.ElementAt(0).Points.Count; i++)
                            {
                                writeTime = true;
                                foreach (var ser in chart1.Series)
                                {
                                    if (writeTime)
                                    {
                                        sw.Write(ser.Points[i].XValue.ToString() + "\t");
                                        writeTime = false;
                                    }

                                    sw.Write(ser.Points[i].YValues[0].ToString() + "\t");
                                }
                                sw.WriteLine();
                            }
                        sw.Close();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

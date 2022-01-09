using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace Build2Evenize
{
    public partial class FormProjectInfo : Form
    {
        private Common common;
        int id;
        public FormProjectInfo(int projectId, Common common)
        {
            this.id = projectId;
            this.common = common;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            common.Country(comboBox2.Text, label6);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public System.Data.DataTable formofDataTable(Microsoft.Office.Interop.Excel.Worksheet ws)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string worksheetName = ws.Name;
            dt.TableName = worksheetName;
            Microsoft.Office.Interop.Excel.Range xlRange = ws.UsedRange;
            object[,] valueArray = (object[,])xlRange.get_Value(XlRangeValueDataType.xlRangeValueDefault);
            for (int k = 1; k <= valueArray.GetLength(1); k++)
            {
                dt.Columns.Add((string)valueArray[1, k]);  //add columns to the data table.
            }
            object[] singleDValue = new object[valueArray.GetLength(1)]; //value array first row contains column names. so loop starts from 2 instead of 1
            for (int i = 2; i <= valueArray.GetLength(0); i++)
            {
                for (int j = 0; j < valueArray.GetLength(1); j++)
                {
                    if (valueArray[i, j + 1] != null)
                    {
                        singleDValue[j] = valueArray[i, j + 1].ToString();
                    }
                    else
                    {
                        singleDValue[j] = valueArray[i, j + 1];
                    }
                }
                dt.LoadDataRow(singleDValue, System.Data.LoadOption.PreserveChanges);
            }

            return dt;
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "All Files (*.*)|*.*";
            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                string sFileName = choofdlog.FileName;
                string path = System.IO.Path.GetFullPath(choofdlog.FileName);
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                DataSet ds = new DataSet();
                Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks.Open(path);
                foreach (Microsoft.Office.Interop.Excel.Worksheet ws in wb.Worksheets)
                {
                    System.Data.DataTable td = new System.Data.DataTable();
                    td = await Task.Run(() => formofDataTable(ws));
                    ds.Tables.Add(td);//This will give the DataTable from Excel file in Dataset
                }
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
                wb.Close();
            }
        }

        private void FormProjectInfo_Load(object sender, EventArgs e)
        {
            common.Filters("area", "name", comboBox1);
            common.Filters("institution", "name", comboBox2);
            string query = "select * from Project where project_id = " + this.id;
            SqlCommand cmd = new SqlCommand(query, common.con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                textBox1.Text = dr.GetString(1);
                textBox2.Text = dr.GetString(2);
            }
            dr.Close();
            dr.Dispose();
            query = "select area_id from Area where area_id in (select area_id from Project_Area where project_id = " + this.id + ")";
            cmd = new SqlCommand(query, common.con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                comboBox1.SelectedIndex = dr.GetInt32(0) - 1;
            }
            dr.Close();
            dr.Dispose();
            query = "select institution_id from Institution where institution_id in (select institution_id from Project where project_id = " + this.id + ")";
            cmd = new SqlCommand(query, common.con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                comboBox2.SelectedIndex = dr.GetInt32(0) - 1;
            }
            dr.Close();
            dr.Dispose();

            
            common.Country(comboBox2.Text, label6);           
            
            int partnersNumber = common.Partner(this.id, comboBox3);
            common.Partner(this.id, comboBox4);
            common.Partner(this.id, comboBox9);
            comboBox3.SelectedIndex = 0;
            switch (partnersNumber)
            {
                case 1:
                    comboBox4.Visible = false;
                    comboBox9.Visible = false;
                    break;
                case 2:
                    comboBox4.SelectedIndex = 1;
                    comboBox3.Visible = true;
                    comboBox4.Visible = true;
                    comboBox9.Visible = false;
                    break;
                case 3:
                    comboBox4.SelectedIndex = 1;
                    comboBox9.SelectedIndex = 2;
                    comboBox3.Visible = true;
                    comboBox4.Visible = true;
                    comboBox9.Visible = true;
                    break;
            }

        }
    }
}

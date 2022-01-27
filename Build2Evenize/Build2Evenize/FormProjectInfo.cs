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
        int id,coordinatorId;
        FormProject f;
        public FormProjectInfo(FormProject form, int projectId,int coordinatorId, Common common)
        {
            this.f = form;
            this.id = projectId;
            this.common = common;
            this.coordinatorId = coordinatorId;
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
            common.InstitutionCountry(comboBox2.Text, label6);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            comboBox3.ResetText();
            comboBox3.Focus();
            button17.Visible =false;
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
            bool isAdmin = false;
            string query = "SELECT C.coordinator_id from Coordinator C where C.institution_id in (SELECT P.institution_id FROM Project P where P.project_id =" + this.id + ");";
            SqlCommand cmd = new SqlCommand(query, common.con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                isAdmin = (dr.GetInt32(0) == coordinatorId);
            }
            dr.Close();
            dr.Dispose();

            if (isAdmin)
            {
                button10.Visible = true;
                button11.Visible = true;
            }

            common.Filters("area", "name", comboBox1);
            common.Filters("institution", "name", comboBox2);
            query = "select * from Project where project_id = " + this.id;
            cmd = new SqlCommand(query, common.con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                textBox1.Text = dr.GetString(1);
                textBox2.Text = dr.GetString(2);
                dateTimePicker1.Value = dr.GetDateTime(4);
                dateTimePicker2.Value = dr.GetDateTime(5);
                numericUpDown1.Value = dr.GetInt32(3);

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

            
            common.InstitutionCountry(comboBox2.Text, label6);
            common.Fill("select distinct name from Institution", comboBox3, comboBox4, comboBox9);
            common.Switcher(0,"select I.institution_id,I.name from Project_Partner PP JOIN Institution I on PP.institution_id = I.institution_id where project_id = " + this.id, comboBox3, comboBox4, comboBox9, button1,button3,button12,button17,button15);

            common.Fill("select distinct name from Tech", comboBox6, comboBox5, comboBox10);
            common.Switcher(1,"select T.tech_id, T.name from Project_Tech PT JOIN Tech T on PT.tech_id = T.tech_id where project_id = "+ this.id, comboBox6, comboBox5, comboBox10, button2, button4, button13, button18, button16);

            common.Fill("select distinct name from Social_Skill", comboBox8, comboBox7, comboBox11);
            common.Switcher(2, "select SS.social_skill_id, SS.name from Project_SK PS JOIN Social_Skill SS on SS.social_skill_id = PS.social_skill_id where project_id = " + this.id, comboBox8, comboBox7, comboBox11, button5, button6, button14, button20, button19);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            comboBox9.ResetText();
            comboBox4.Focus();
            button12.Visible = false;
            comboBox9.Visible = false;
            button3.Visible = true;
            button15.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox4.ResetText();
            comboBox3.Focus();
            button1.Visible = true;
            button3.Visible = false;
            comboBox4.Visible = false;
            button17.Visible = true;
            button15.Visible = false;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button3.Visible = true;
            comboBox4.Visible = true;
            button17.Visible = false;
            button15.Visible = true;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            button12.Visible = true;
            comboBox9.Visible = true;
            button3.Visible = false;
            button15.Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button18.Visible = false;
            comboBox6.ResetText();
            comboBox6.Focus();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            comboBox10.ResetText();
            button13.Visible = false;
            comboBox10.Visible = false;
            button4.Visible = true;
            button16.Visible = true;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            button4.Visible = true;
            comboBox5.Visible = true;
            button18.Visible = false;
            button16.Visible = true;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            button13.Visible = true;
            comboBox10.Visible = true;
            button4.Visible = false;
            button16.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            comboBox5.ResetText();
            button2.Visible = true;
            button4.Visible = false;
            comboBox5.Visible = false;
            button18.Visible = true;
            button16.Visible = false;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            comboBox8.ResetText();
            comboBox8.Focus();
        }


        private void button19_Click(object sender, EventArgs e)
        {
            button5.Visible = false;
            button14.Visible = true;
            comboBox11.Visible = true;
            button6.Visible = false;
            button19.Visible = false;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            button5.Visible= false;
            button6.Visible = true;
            comboBox7.Visible = true;
            button20.Visible = false;
            button19.Visible = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            button14.Visible = false;
            comboBox11.Visible = false;
            button6.Visible = true;
            button19.Visible = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            panel3.Enabled = true;
            panel2.Enabled = true;
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            const string message = "Are you sure that you want to delete this project?";
            const string caption = "Delete Project";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Project_Area WHERE project_id =" + this.id + ";" +
                    "DELETE FROM Project_Partner WHERE project_id =" + this.id + ";" +
                    "DELETE FROM Project_SK WHERE project_id =" + this.id + ";" +
                    "DELETE FROM Project_Team WHERE project_id =" + this.id + ";" +
                    "DELETE FROM Project_Tech WHERE project_id =" + this.id + ";" +
                    "DELETE FROM Project WHERE project_id =" + this.id;
                SqlCommand cmd = new SqlCommand(query, common.con);
                int project =+ cmd.ExecuteNonQuery();


                if (project != 0)
                    MessageBox.Show("Project Deleted!");
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != -1)
            {
                button17.Visible = true;
                button1.Visible = true;
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex != -1)
            {
                button18.Visible = true;
                button2.Visible = true;
            }
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox8.SelectedIndex != -1)
            {
                button20.Visible = true;
                button5.Visible = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button5.Visible = true;
            button6.Visible = false;
            comboBox7.Visible = false;
            button20.Visible = true;
            button19.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            common.UpdateProject(this.id,textBox1.Text,textBox2.Text,(int)numericUpDown1.Value, dateTimePicker1.Text,dateTimePicker2.Text,comboBox2.Text,comboBox1.Text,comboBox3.Text,comboBox4.Text,comboBox9.Text, comboBox6.Text, comboBox5.Text, comboBox10.Text);
            

            MessageBox.Show(textBox1.Text + " Updated Successfully!", "Success", MessageBoxButtons.OK);
            f.Refresh();
        }

        public void setPanelVisible(bool flag)
        {
            this.panel2.Enabled = flag;
            this.panel3.Enabled = flag;
        }
    }
}

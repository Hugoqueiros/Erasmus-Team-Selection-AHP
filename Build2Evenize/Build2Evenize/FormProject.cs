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

namespace Build2Evenize
{
    public partial class FormProject : Form
    {
        SqlConnection con;
        int id;
        String name, area, institution, country;
        private Common common;

        public FormProject(string name, int id, Common common)
        {
            InitializeComponent();
            label6.Text = name;
            this.id = id;
            this.common = common;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProjectManagementPanel.Visible = false;
            SimulationPanel.Visible = true;
            button1.BackColor = Color.FromArgb(37, 55, 127);
            button1.ForeColor = Color.White;
            button2.BackColor = Color.FromArgb(250, 218, 24);
            button2.ForeColor = Color.FromArgb(37, 55, 127);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProjectManagementPanel.Visible = true;
            SimulationPanel.Visible = false;
            button2.BackColor = Color.FromArgb(37, 55, 127);
            button2.ForeColor = Color.White;
            button1.BackColor = Color.FromArgb(250, 218, 24);
            button1.ForeColor = Color.FromArgb(37, 55, 127);
        }
        private void CheckFilters()
        {
            String query=null;
            DataView DV = new DataView(this.build2evenizeDataSet.View_1);
            if (name != null)
            {
                if (query == null)
                {
                    query = "name LIKE '%" + name + "%'";
                }
                else
                {
                    query += " AND name LIKE '%" + name + "%'";
                }
            };
            if (area != "All")
            {
                if (query == null)
                {
                    query = "area LIKE '%" + area + "%'";
                }
                else
                {
                    query += " AND area LIKE '%" + area + "%'";
                }
            };
            if (institution != "All")
            {
                if (query == null)
                {
                    query = "institution LIKE '%" + institution + "%'";
                }
                else
                {
                    query += " AND institution LIKE '%" + institution + "%'";
                }
            };
            if (country != "All")
            {
                if (query == null)
                {
                    query = "country LIKE '%" + country + "%'";
                }
                else
                {
                    query += " AND country LIKE '%" + country + "%'";
                }
            };
            DV.RowFilter = query;
            dataGridView1.DataSource = DV;
        }
        private void FormProject_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("All");
            common.Filters("area", "name", comboBox1); // filter of area 
            comboBox2.Items.Add("All");
            common.Filters("institution", "name", comboBox2); //filter of institution name
            comboBox3.Items.Add("All");
            common.Filters("institution", "country", comboBox3); //filter of institution country
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            // TODO: This line of code loads data into the 'build2evenizeDataSet.View_1' table. You can move, or remove it, as needed.
            this.view_1TableAdapter.Fill(this.build2evenizeDataSet.View_1);

        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void label3_Click(object sender, EventArgs e)
        {

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            area = comboBox1.Text;
            CheckFilters();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            name = textBox1.Text;
            CheckFilters();
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            country = comboBox3.Text;
            CheckFilters();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int projectId = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            
            FormProjectInfo fpi = new FormProjectInfo(projectId,id, common);
            fpi.ShowDialog();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            institution = comboBox2.Text;         
            CheckFilters();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormProjectInfo fpi = new FormProjectInfo(0,id, common);
            fpi.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Login_Register lr = new Login_Register();
            lr.ShowDialog();
        }
    }


}

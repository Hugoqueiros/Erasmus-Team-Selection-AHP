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
        String name, area, institution, country;

        public FormProject(string name, int id, SqlConnection con)
        {
            InitializeComponent();
            label6.Text = name;
            this.con =con;
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
        private void Filters(string table, string columnName, ComboBox comboBox) //function for searching filters
        {

            // query to get columnname from table and fill in the combobox items
            string query = "select distinct " + columnName+" from " + table;
            SqlCommand cmd = new SqlCommand(query, this.con);
            comboBox.Items.Add("All");
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                //add every row found on database to combobox
                string name = (string)dr[columnName];
                comboBox.Items.Add(name);
            }
            dr.Close();
        }
        private void CheckFilters()
        {
            String query=null;
            DataView DV = new DataView(this.build2evenizeDataSet.View_1);
            if (name != "All")
            {
                if (query == null)
                {
                    query = "Name LIKE '%" + name + "%'";
                }
                else
                {
                    query += " AND Name LIKE '%" + name + "%'";
                }
            };
            if (area != "All")
            {
                if (query == null)
                {
                    query = "Expr1 LIKE '%" + area + "%'";
                }
                else
                {
                    query += " AND Expr1 LIKE '%" + area + "%'";
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
            Filters("area", "name", comboBox1); // filter of area 
            Filters("institution", "name", comboBox2); //filter of institution name
            Filters("institution", "country", comboBox3); //filter of institution country
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
            country = comboBox2.Text;
            CheckFilters();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int projectId = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            
            FormProjectInfo fpi = new FormProjectInfo(projectId);
            fpi.ShowDialog();
        }

        private int ToInt32(string v)
        {
            throw new NotImplementedException();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            institution = comboBox2.Text;         
            CheckFilters();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormProjectInfo fpi = new FormProjectInfo(0);
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

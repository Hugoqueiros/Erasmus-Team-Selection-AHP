using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Build2Evenize
{
    public partial class FormProject : Form
    {
        public FormProject(string name, int id)
        {
            InitializeComponent();
            label6.Text = name;
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

        private void FormProject_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'build2evenizeDataSet.View_1' table. You can move, or remove it, as needed.
            this.view_1TableAdapter.Fill(this.build2evenizeDataSet.View_1);


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Build2Evenize
{
    public partial class Login_Register : Form
    {
        public string server = @"DESKTOP-I51RAV\SQLEXPRESS";
        public string database = "SAD";
        public string user = @"AzureAD\FranciscoPereira";
        public string pass = "";

        public Login_Register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Register_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void loginUser_TextChanged(object sender, EventArgs e)
        {

        }

        public string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            String str = $"server={server};database={database};UID={user};password={pass};Trusted_Connection=True";
            SqlConnection con = new SqlConnection(str);
            con.Open();

            if (loginUser.Text != String.Empty && loginPass.Text != String.Empty) {
                String passEnc = MD5Hash(loginPass.Text);
                SqlCommand cmd = new SqlCommand("select * from Coordinator where email='" + loginUser.Text + "' and password='" + passEnc+"'", con);
                SqlDataReader  dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    this.Hide();
                    FormProject fP = new FormProject();
                    fP.ShowDialog();
                    this.Close();
                }
                else
                {
                    dr.Close();
                    MessageBox.Show("Não existe conta com os dados inseridos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor preencha todos os campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}

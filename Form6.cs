using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace medicineManagement
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        readonly SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-IR8P1CI\TANVIR1;Initial Catalog=Medicine_management;Integrated Security=True");
        public int userId;
        private void Full_Click(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
        }

        private void Registration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                TopMost = false;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            SqlCommand cmd2 = new SqlCommand("SELECT * FROM [User] WHERE userName = " + "'" + textBox1.Text + "'", con);

            con.Open();

            int count = 0;
            SqlDataReader DR2 = cmd2.ExecuteReader();
            while (DR2.Read()) {
                if (DR2[1].ToString().Equals(textBox1.Text.ToString())) {
                    MessageBox.Show("Name is not valid!\nTake another name, plz.");
                    count++;
                    break;
                }
            }
            DR2.Close();

            if (count == 0) {

                if (MessageBox.Show("Are you Confirm?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (IsValid())
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO [User] values (@Name,@phoneNo,@Address,@Password,@userType)", con);
                        cmd.CommandType = CommandType.Text;

                        String cmbItemValue = userType2.SelectedItem.ToString();

                        cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                        cmd.Parameters.AddWithValue("@phoneNo", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Address", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Password", textBox4.Text);
                        cmd.Parameters.AddWithValue("@userType", cmbItemValue);

                       
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("User added successfully", "saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        getUserRecord();
                        ResetFormControls();
                    }
                }

            }



            cmd2.CommandType = CommandType.Text;
            con.Close();

        }
        private bool IsValid()
        {
            if (textBox1.Text == String.Empty)
            {
                MessageBox.Show("User name is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void ResetFormControls()
        {
            userId = 0;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();

            textBox1.Focus();

        }


        private void Registration_Load(object sender, EventArgs e)
        {
            getUserRecord();
        }

        private void getUserRecord()
        {
            SqlCommand cmd = new SqlCommand("Select * from [User]", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

        }
    }
}

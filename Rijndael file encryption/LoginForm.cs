using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Rijndael_file_encryption
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        internal static string GetStringSha256Hash(string text)
        {
            if(String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            //pswdHashed
            if (string.IsNullOrEmpty(Properties.Settings.Default["pswdHashed"].ToString()))
            {
                DialogResult dialogResult = MessageBox.Show("No Password Created Yet! Do you want to create one now?", "Create Password", MessageBoxButtons.YesNo);
                if(dialogResult == DialogResult.Yes)
                {
                    CreatePasswordForm f1 = new CreatePasswordForm();
                    f1.ShowDialog();

                }
                else
                {
                    MessageBox.Show("Come back when you are ready");
                    Application.Exit();

                }
            }

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //pswdHashed
            if (string.IsNullOrEmpty(Properties.Settings.Default["pswdHashed"].ToString()))
            {
                Form1 f1 = new Form1();
                MessageBox.Show("Please Create a Password to ensure the best security for your files");
                f1.Show();
            }
            else
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Please Enter a Password!");
                }
                else
                {
                    string crypt = GetStringSha256Hash(textBox1.Text);
                    if (crypt == Properties.Settings.Default["pswdHashed"].ToString())
                    {
                        Form1 f1 = new Form1();
                        f1.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect Password!");
                    }
                }
            }
        }
    }
}

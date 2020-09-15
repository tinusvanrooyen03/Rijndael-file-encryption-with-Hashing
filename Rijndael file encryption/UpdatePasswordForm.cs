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
    public partial class UpdatePasswordForm : Form
    {
        public UpdatePasswordForm()
        {
            InitializeComponent();
        }

        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
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

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "" || txtOld.Text == "")
            {
                MessageBox.Show("Please Complete All Fields");
            }
            else
            {
                string crypt = GetStringSha256Hash(txtOld.Text);
                if(crypt == Properties.Settings.Default["pswdHashed"].ToString())
                {
                    if(textBox1.Text == textBox2.Text)
                    {
                        crypt = GetStringSha256Hash(textBox1.Text);
                        Properties.Settings.Default["pswdHashed"] = crypt;
                        Properties.Settings.Default.Save();
                        MessageBox.Show("Password Updated Successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Passwords does not match!");
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect Password");
                }
            }
            this.Hide();
        }
    }
}

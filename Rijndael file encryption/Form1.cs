using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;


namespace Rijndael_file_encryption
{
    public partial class Form1 : Form
    {
        public Form1()
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
        // Encrypt a file into another file using a password 
        public static void EncryptFile(string fileIn,
                    string fileOut, string Password)
        {
            
            
                // First we are going to open the file streams 
                FileStream fsIn = new FileStream(fileIn,
                    FileMode.Open, FileAccess.Read);
                FileStream fsOut = new FileStream(fileOut,
                    FileMode.OpenOrCreate, FileAccess.Write);

                // Then we are going to derive a Key and an IV from the
                // Password and create an algorithm 
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });

                Rijndael alg = Rijndael.Create();
                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                // Now create a crypto stream through which we are going
                // to be pumping data. 
                // Our fileOut is going to be receiving the encrypted bytes. 
                CryptoStream cs = new CryptoStream(fsOut,
                    alg.CreateEncryptor(), CryptoStreamMode.Write);

                // Now will will initialize a buffer and will be processing
                // the input file in chunks. 
                // This is done to avoid reading the whole file (which can
                // be huge) into memory. 
                int bufferLen = 4096;
                byte[] buffer = new byte[bufferLen];
                int bytesRead;

                do
                {
                    // read a chunk of data from the input file 
                    bytesRead = fsIn.Read(buffer, 0, bufferLen);

                    // encrypt it 
                    cs.Write(buffer, 0, bytesRead);
                } while (bytesRead != 0);

                // close everything 

                // this will also close the unrelying fsOut stream
                cs.Close();
                fsIn.Close();
                MessageBox.Show("File Encrypted");
            
            
            
                
            
            
        }

        // Decrypt a file into another file using a password 
        public static void DecryptFile(string fileIn,
                    string fileOut, string Password)
        {
            
            
                // First we are going to open the file streams 
                FileStream fsIn = new FileStream(fileIn,
                            FileMode.Open, FileAccess.Read);
                FileStream fsOut = new FileStream(fileOut,
                            FileMode.OpenOrCreate, FileAccess.Write);

                // Then we are going to derive a Key and an IV from
                // the Password and create an algorithm 
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });
                Rijndael alg = Rijndael.Create();

                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                // Now create a crypto stream through which we are going
                // to be pumping data. 
                // Our fileOut is going to be receiving the Decrypted bytes. 
                CryptoStream cs = new CryptoStream(fsOut,
                    alg.CreateDecryptor(), CryptoStreamMode.Write);

                // Now will will initialize a buffer and will be 
                // processing the input file in chunks. 
                // This is done to avoid reading the whole file (which can be
                // huge) into memory. 
                int bufferLen = 4096;
                byte[] buffer = new byte[bufferLen];
                int bytesRead;

                do
                {
                    // read a chunk of data from the input file 
                    bytesRead = fsIn.Read(buffer, 0, bufferLen);

                    // Decrypt it 
                    cs.Write(buffer, 0, bytesRead);

                } while (bytesRead != 0);

                // close everything 
                cs.Close(); // this will also close the unrelying fsOut stream 
                fsIn.Close();
                MessageBox.Show("Decrypted");
            
          
            
               
            
        }


        private void Encrypt_Click(object sender, EventArgs e)
        {

            string password = GetStringSha256Hash(txtPassword.Text);
            string infile = txtFile.Text;
            
            string outfile = txtNewFile.Text;
            MessageBox.Show(password);

            EncryptFile(infile, outfile, password);

            File.Delete(@txtFile.Text);

            rdbDecrypt.Checked = false;
            rdbEncrypt.Checked = false;

            txtPassword.Text = "";
            txtFile.Text = "";
            txtFinal.Text = "";
            txtNewFile.Text = "";

            btnBrowse.Enabled = false;
            txtPassword.Enabled = false;
            Encrypt.Enabled = false;
            txtFinal.Enabled = false;
            btnLocation1.Enabled = false;





        }
       
        private void Decrypt_Click(object sender, EventArgs e)
        {
            string password = GetStringSha256Hash(textBox1.Text);
            string infile = txtDecryptfile.Text;
            
            string outfile = txtFinal2.Text;

            DecryptFile(infile, outfile, password);
            MessageBox.Show(password);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Multiselect = false;
            if(od.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = od.FileName;
            }
        }

        private void btnDecripBrowse_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Multiselect = false;
            if (od.ShowDialog() == DialogResult.OK)
            {
                txtDecryptfile.Text = od.FileName;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            UpdatePasswordForm f1 = new UpdatePasswordForm();
            f1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["pswdHashed"] = null;
            Properties.Settings.Default.Save();
            MessageBox.Show("Program has been reset");
        }

        private void btnShowHashed_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Properties.Settings.Default["pswdHashed"].ToString());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goodbye");
            Application.Exit();
        }

        private void btnEncryptBrose_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew)) 
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(txtNewFile.Text);
                }

            }

        }

        private void btnBrowseDe_Click(object sender, EventArgs e)
        {
            
        }

        private void txtNewFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rdbEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            btnBrowse.Enabled = true;
            txtPassword.Enabled = true;
            Encrypt.Enabled = true;
            txtFinal.Enabled = true;
            btnLocation1.Enabled = true;

            button1.Enabled = false;
            textBox1.Enabled = false;
            txtFilename.Enabled = false;
            button3.Enabled = false;
            Decrypt.Enabled = false;

            pictureBox2.Visible = true;
            pictureBox3.Visible = false;


        }

        private void btnLocation1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtNewFile.Text = fbd.SelectedPath + "\\" + txtFinal.Text;


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFinal2.Text = fbd.SelectedPath + "\\" + txtFilename.Text;


            }
        }

        private void rdbDecrypt_CheckedChanged(object sender, EventArgs e)
        {
            btnBrowse.Enabled = false;
            txtPassword.Enabled = false;
            Encrypt.Enabled = false;
            txtFinal.Enabled = false;
            btnLocation1.Enabled = false;

            button1.Enabled = true;
            textBox1.Enabled = true;
            txtFilename.Enabled = true;
            button3.Enabled = true;
            Decrypt.Enabled = true;

            pictureBox2.Visible = false;
            pictureBox3.Visible = true;


        }
    }
}

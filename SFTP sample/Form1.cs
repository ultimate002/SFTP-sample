using System;
using System.IO;
using Renci.SshNet;
using System.Windows.Forms;

namespace SFTP_sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                // SFTP server connection details
                string host = txtHost.Text;
                string username = txtUsername.Text;
                string privateKeyFilePath = txtPpkPathFile.Text;
                string privateKeyPassphrase = txtPkPassphrase.Text;
                string localFilePath = txtFilePath.Text; // Replace with the path to the local file you want to send

                PrivateKeyFile privateKeyFile = new PrivateKeyFile(privateKeyFilePath, privateKeyPassphrase);
                PrivateKeyAuthenticationMethod privateKeyAuth = new PrivateKeyAuthenticationMethod(username, privateKeyFile);

                // Connect to the SFTP server
                ConnectionInfo connectionInfo = new ConnectionInfo(host, username, privateKeyAuth);
                using (SftpClient sftpClient = new SftpClient(connectionInfo))
                {
                    sftpClient.Connect();
                    if (sftpClient.IsConnected)
                    {
                        MessageBox.Show("Connected");
                        // Upload the file to the SFTP server
                        using (FileStream oFileStream = new FileStream(localFilePath, FileMode.Open))
                        {
                            string filename = Path.GetFileName(localFilePath);
                            sftpClient.UploadFile(oFileStream, filename);
                        }
                        // Check if the file exists on the server
                        bool fileExists = sftpClient.Exists(Path.GetFileName(localFilePath));
                        if (fileExists)
                        {
                            MessageBox.Show("File uploaded successfully.");
                        }
                        else
                        {
                            MessageBox.Show("File upload failed or file does not exist on the server.");
                        }
                        sftpClient.Disconnect();
                    }
                    else
                    {
                        MessageBox.Show("Not Connected");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
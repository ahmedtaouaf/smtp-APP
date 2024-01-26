using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using OfficeOpenXml;
using System.IO;


namespace Email_Sender_APP
{
    public partial class Form1 : Form
    {
        NetworkCredential login;
        SmtpClient client;
        MailMessage msg;

        public Form1()
        {
            InitializeComponent();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        string smtpServer = "smtp.gmail.com";
        int port = 587;
        string username = "taouafhero@gmail.com";
        string password = "urcp sngb ranb tjwx";
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
               
                //string smtpServer = "smtp.gmail.com";
                //int port = 587;
                //string username = "dinfo@far.ma";
                //string password = "123456789";

                string to = txtTo.Text;
                string cc = txtCC.Text;
                string subject = txtSubject.Text;
                string message = txtMessage.Text;

                using (SmtpClient client = new SmtpClient(smtpServer))
                {
                    client.Port = port;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(username, password);

                    string[] toAddresses = to.Split(',');

                    label9.Text = $"Nbr : {toAddresses.Length}";

                    txtConsole.Clear();
                   

                        foreach (var toAddress in toAddresses)
                    {
                        using (MailMessage mailMessage = new MailMessage())
                        {
                            mailMessage.From = new MailAddress(username, "Sender Name", Encoding.UTF8);
                            mailMessage.To.Add(new MailAddress(toAddress.Trim()));  

                            if (!string.IsNullOrEmpty(cc))
                                mailMessage.CC.Add(cc);

                            mailMessage.Subject = subject;
                            mailMessage.Body = message;
                            mailMessage.BodyEncoding = Encoding.UTF8;
                            mailMessage.IsBodyHtml = true;
                            mailMessage.Priority = MailPriority.Normal;

                            try
                            {
                                client.Send(mailMessage);
                                txtConsole.AppendText($"[{DateTime.Now}] Email sent successfully to: {toAddress.Trim()}\n\n");
                            }
                            catch (Exception ex)
                            {
                                txtConsole.AppendText($"[{DateTime.Now}] Failed to send email to {toAddress.Trim()}. Error: {ex.Message}\n\n");
                            }
                        }

                        txtConsole.SelectionStart = txtConsole.Text.Length;
                        txtConsole.ScrollToCaret();
                        txtConsole.Refresh();
                    }

                    MessageBox.Show("Your messages have been successfully sent.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    Title = "Select an Excel File"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string excelFilePath = openFileDialog.FileName;
                    List<string> emailAddresses = ReadEmailAddressesFromExcel(excelFilePath);

                    
                    txtTo.Text = string.Join(",", emailAddresses);

                    label9.Text = $"Nbr : {emailAddresses.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private List<string> ReadEmailAddressesFromExcel(string filePath)
        {
            List<string> emailAddresses = new List<string>();

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet != null)
                {
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        string emailAddress = worksheet.Cells[row, 1].Value?.ToString();

                        if (!string.IsNullOrEmpty(emailAddress) && IsValidEmail(emailAddress))
                        {
                            emailAddresses.Add(emailAddress);
                        }
                    }
                }
            }

            return emailAddresses;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

            txtSmtp.Text = smtpServer;
            txtPort.Text = "587";
            txtUsername.Text = username;
            txtPassword.Text = password;
        }
    }
}
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
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string smtpServer = "smtp.gmail.com";
                int port = 465;
                string username = "taouafhero@gmail.com";
                string password = "urcp sngb ranb tjwx";

                string to = txtTo.Text;
                string cc = txtCC.Text;
                string subject = txtSubject.Text;
                string message = txtMessage.Text;

                using (SmtpClient client = new SmtpClient(smtpServer))
                {
                    client.Port = port;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(username, password);

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(username, "Sender Name", Encoding.UTF8);
                        mailMessage.To.Add(new MailAddress(to));

                        if (!string.IsNullOrEmpty(cc))
                            mailMessage.CC.Add(cc);

                        mailMessage.Subject = subject;
                        mailMessage.Body = message;
                        mailMessage.BodyEncoding = Encoding.UTF8;
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Priority = MailPriority.Normal;

                        client.Send(mailMessage);

                        MessageBox.Show("Your message has been successfully sent.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
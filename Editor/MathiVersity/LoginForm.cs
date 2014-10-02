using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Editor
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            userNameBox.Text = ConfigManager.GetConfigurationValue_AES(KeyName.loginName);
            passwordBox.Text = ConfigManager.GetConfigurationValue_AES(KeyName.pass);
            if (passwordBox.Text.Length > 0)
            {
                rememberCheckBox.Checked = true;
            }
        }

        private void createNewAccountButton_Click(object sender, EventArgs e)
        {
            RegistrationForm form = new RegistrationForm();
            form.StartPosition = FormStartPosition.CenterParent;            
            form.ShowDialog(this);
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if ((MVService.Login(userNameBox.Text, passwordBox.Text, rememberCheckBox.Checked)))
                {
                    this.Close();
                }
                else
                {
                    errorLabel.Text = "Login failed. Please try again.";
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Error");
            }
            this.Cursor = Cursors.Arrow;
        }
    }
}

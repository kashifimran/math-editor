using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Editor.MVAuthentication;

namespace Editor
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {

        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            if (userNameBox.Text.Trim().Length <= 0)
            {
                errorLabel.Text = "User name cannot be empty";
            }
            else if (firstNameBox.Text.Trim().Length <= 0)
            {
                errorLabel.Text = "First name cannot be empty";
            }
            else if (lastNameBox.Text.Trim().Length <= 0)
            {
                errorLabel.Text = "Last name cannot be empty";
            } 
            else if (emailBox.Text.Trim().Length <= 0)
            {
                errorLabel.Text = "Email cannot be empty";
            }
            else if (!EmailIsValid(emailBox.Text.Trim()))
            {
                errorLabel.Text = "Email address format is invalid";
            }
            else if (passwordBox.Text.Length <= 0)
            {
                errorLabel.Text = "Password cannot be empty";
            }
            else if (passwordBox.Text != confirmPasswordBox.Text)
            {
                errorLabel.Text = "Password and Confirm password do not match";
            }
            else
            {
                RegisterModel registerModel = new RegisterModel()
                {
                    Username = userNameBox.Text.Trim(),
                    FirstName = firstNameBox.Text.Trim(),
                    LastName = lastNameBox.Text.Trim(),
                    Email = emailBox.Text.Trim(),
                    Password = passwordBox.Text,
                    PasswordConfirm = confirmPasswordBox.Text
                };
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    errorLabel.Text = "";
                    RegisterResult registerResult = MVService.Register(registerModel);
                    if (registerResult.Success)
                    {
                        this.Close();
                        MessageBox.Show("Account registration successful.\r\nPlease check your email account for activation email.", "Congratulations");
                    }
                    else
                    {
                        errorLabel.Text = registerResult.Message;
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message, "An error occured");
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                }
            }            
        }

        bool EmailIsValid(string emailAddress)
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}

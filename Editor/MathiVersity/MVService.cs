using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor.MVAuthentication;
using Editor.MVStorage;
using System.Windows.Forms;
using System.IO;

namespace Editor
{   
    public static class MVService
    {
        public static event EventHandler UserLoggedIn = (x, y) => { };
        public static event EventHandler UserLoggedOut = (x, y) => { };
        
        static string loginToken = "";
        public static string LoginName { get; private set; }
        public static string UserName { get; private set; }

        public static bool IsLoggedIn { get { return loginToken.Length > 0; } }

        public static bool AddFile(string title, string body)
        {
            if (loginToken.Length > 0)
            {
                try
                {
                    using (StorageClient sc = new StorageClient())
                    {
                        return sc.AddFile(loginToken, new StorageFile() { Title = title, Body = body });
                    }
                }
                catch { }
            }
            return false;
        }

        public static StorageFile[] GetFiles()
        {
            try
            {
                using (StorageClient sc = new StorageClient())
                {
                    return sc.GetFiles(loginToken);
                }
            }
            catch
            {
                MessageBox.Show("An error occured while trying to retrieve file list from server.\r\nMake sure you are connected to the Internet.", "Error");
                return null;
            }
        }

        public static StorageFile GetFile(int id)
        {
            try
            {
                using (StorageClient sc = new StorageClient())
                {
                    return sc.GetFile(loginToken, id);
                }
            }
            catch
            {
                MessageBox.Show("An error occured while trying to retrieve file list from server.\r\nMake sure you are connected to the Internet.", "Error");
                return null;
            }
        }

        public static bool DeleteFile(int id)
        {
            using (StorageClient sc = new StorageClient())
            {
                return sc.DeleteFile(loginToken, id);
            }
        }
        
        public static bool Login(string loginName, string passWord, bool rememberPass)
        {
            if (!string.IsNullOrEmpty(loginName) && !string.IsNullOrEmpty(passWord))
            {
                using (AuthenticationClient ac = new AuthenticationClient())
                {
                    string [] loginResult = ac.Login(loginName, passWord);
                    if (loginResult.Length >= 2)
                    {
                        string token = loginResult[0];
                        if (token != null && token.Length > 0)
                        {
                            ConfigManager.SetConfigurationValue_AES(KeyName.loginName, loginName);
                            if (rememberPass)
                            {
                                ConfigManager.SetConfigurationValue_AES(KeyName.pass, passWord);
                            }
                            else
                            {
                                ConfigManager.SetConfigurationValue(KeyName.pass, "");
                            }
                            loginToken = token;
                            LoginName = loginName;
                            UserName = loginResult[1];
                            UserLoggedIn(null, EventArgs.Empty);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static RegisterResult Register(RegisterModel registerModel)
        {
            using (AuthenticationClient ac = new AuthenticationClient())
            {
                return ac.Register(registerModel);
            }
        }

        public static void Logout()
        {
            loginToken = "";
            UserLoggedOut(null, EventArgs.Empty);
        }

        public static void UpdateFile(StorageFile file)
        {
            using (StorageClient sc = new StorageClient())
            {
                sc.UpdateFile(loginToken, file);
            }
        }
    }
}

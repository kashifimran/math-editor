using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Resources;
using System.Collections;
using System.Security.Cryptography;

namespace Editor
{
    public enum KeyName { symbols, pass, loginName, version, default_font, default_mode, s01, s02, firstTime, checkUpdates };

    static class ConfigManager
    {
        static string exePath = Assembly.GetEntryAssembly().Location;
        static string appVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
        static AppSettingsSection appSection = null;
        static Configuration config = null;        

        static ConfigManager()
        {
            try
            {
                if (!Directory.Exists(PublicFolderPath))
                {
                    Directory.CreateDirectory(PublicFolderPath);
                }
                bool existed = true;
                if (!File.Exists(PublicConfigFilePath))
                {
                    CopyConfigFile();
                    existed = false;
                }
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = PublicConfigFilePath };
                config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                appSection = config.AppSettings;//(AppSettingsSection)config.GetSection("appSettings");
                if (!existed)
                {
                    SetConfigurationValue(KeyName.version, Assembly.GetEntryAssembly().GetName().Version.ToString());
                }
            }
            catch { }
        }


        public static string GetConfigurationValue(KeyName key)
        {
            try
            {
                return appSection.Settings[key.ToString()].Value;
            }
            catch
            {
                return "";
            }
        }

        public static int GetNumber(KeyName key, int defaultNum)
        {
            try
            {
                string numString = GetConfigurationValue(key);                
                int num = int.Parse(numString);
                return num;
            }
            catch
            {
                return defaultNum;
            }
        }

        public static int GetEditorMode(int defaultMode){
            return GetNumber(KeyName.default_mode, defaultMode);
        }

        public static bool SetConfigurationValue(KeyName key, string value)
        {
            try
            {
                if (!File.Exists(PublicConfigFilePath))
                {
                    CopyConfigFile();
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = PublicConfigFilePath };
                    config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    appSection = config.AppSettings;
                    SetConfigurationValue(KeyName.version, Assembly.GetEntryAssembly().GetName().Version.ToString());
                }
                if (appSection.Settings.AllKeys.Contains(key.ToString()))
                {
                    appSection.Settings[key.ToString()].Value = value;
                }
                else
                {
                    appSection.Settings.Add(key.ToString(), value);
                }
                config.Save();
                return true;
            }
            catch { }
            return false;
        }

        public static bool SetConfigurationValues(Dictionary<KeyName, string> configItems)
        {
            try
            {
                if (!File.Exists(PublicConfigFilePath))
                {
                    CopyConfigFile();
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = PublicConfigFilePath };
                    config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    appSection = config.AppSettings;
                    SetConfigurationValue(KeyName.version, Assembly.GetEntryAssembly().GetName().Version.ToString());
                }
                foreach (var item in configItems)
                {
                    if (appSection.Settings.AllKeys.Contains(item.Key.ToString()))
                    {
                        appSection.Settings[item.Key.ToString()].Value = item.Value;
                    }
                    else
                    {
                        appSection.Settings.Add(item.Key.ToString(), item.Value);
                    }
                }                
                config.Save();
                return true;
            }
            catch { }
            return false;
        }


        public static string PublicConfigFilePath
        {
            get { return Path.Combine(PublicFolderPath, Path.GetFileName(Assembly.GetEntryAssembly().Location) + ".config"); }
        }

        public static string PublicFolderPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Math_Editor_MV");
            }
        }

        private static void CopyConfigFile()
        {
            try
            {
                File.Copy(exePath + ".config", PublicConfigFilePath);
            }
            catch { }
            //try
            //{
            //    var file = GetResourceStream("app.config");
            //    using (var reader = new StreamReader(file))
            //    {
            //        using (var stream = File.OpenWrite(PublicConfigFilePath))
            //        {
            //            using (StreamWriter writer = new StreamWriter(stream))
            //            {
            //                writer.Write(reader.ReadToEnd());
            //            }
            //        }
            //    }
            //}
            //catch { }
        }

        private static UnmanagedMemoryStream GetResourceStream(string resName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var strResources = assembly.GetName().Name + ".g.resources";
            var rStream = assembly.GetManifestResourceStream(strResources);
            var resourceReader = new ResourceReader(rStream);
            var items = resourceReader.OfType<DictionaryEntry>();
            var stream = items.First(x => (x.Key as string) == resName.ToLower()).Value;
            return (UnmanagedMemoryStream)stream;
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static bool SetConfigurationValue_AES(KeyName key, string value)
        {
            try
            {
                return SetConfigurationValue(key, EncryptText(value,  "abcd_o82349834jefhdfer8&"));//GetConfigurationValue(KeyName.s01)));
            }
            catch { }
            return false;
        }

        public static string GetConfigurationValue_AES(KeyName key)
        {
            try
            {
                string base64String = GetConfigurationValue(key);
                return DecryptText(base64String, "abcd_o82349834jefhdfer8&");//GetConfigurationValue(KeyName.s01));
            }
            catch
            {
                return "";
            }
        }

        /*
        public static string EncryptString_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                    StreamWriter swEncrypt = new StreamWriter(csEncrypt);
                    swEncrypt.Write(plainText);
                    swEncrypt.Flush();
                    swEncrypt.Close();
                    encrypted = msEncrypt.ToArray();
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptString_Aes(string base64Text, byte[] Key, byte[] IV)
        {
            if (base64Text == null || base64Text.Length <= 0)
                throw new ArgumentNullException("base64Text");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            byte[] cipherText = Convert.FromBase64String(base64Text);
            string plaintext = null;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                    StreamReader srDecrypt = new StreamReader(csDecrypt);
                    plaintext = srDecrypt.ReadToEnd();
                }
            }
            return plaintext;
        }
        */

        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public static string EncryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        public static string DecryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }

        //32 bytes
        static byte[] AesKeyBytes
        {
            //get { return Convert.FromBase64String("/lQCPxfDQ4QaEkUsBYkcdkAm/CYeGnwOcoYcZTBAh68="); }
            get { return GetBytes(KeyName.s01, 32); }
        }

        //16 bytes
        static byte[] AesIVBytes
        {
            //get { return Convert.FromBase64String("ton4ck7hOjyMmuE5QsKXQA=="); }
            get { return GetBytes(KeyName.s02, 16); }
        }

        static byte[] GetBytes(KeyName key, int size)
        {
            string value = GetConfigurationValue(key);
            byte[] bytes = new byte[size];
            if (string.IsNullOrEmpty(value))
            {
                Random rand = new Random();
                rand.NextBytes(bytes);
                value = Convert.ToBase64String(bytes);
                SetConfigurationValue(key, value);
            }
            bytes = Convert.FromBase64String(value);
            return bytes;
        }
    }
}

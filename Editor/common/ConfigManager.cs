using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;

namespace Editor
{
    public enum KeyName { symbols, pass, loginName, version, default_font, default_mode, s01, s02, firstTime, checkUpdates };

    internal static class ConfigManager
    {
        static readonly string exePath = Assembly.GetEntryAssembly().Location;
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
                var fileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = PublicConfigFilePath
                };
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
                if (appSection.Settings.AllKeys.Contains(key.ToString()))
                {
                    return appSection.Settings[key.ToString()].Value;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static int GetNumber(KeyName key, int defaultNum)
        {
            try
            {
                return int.Parse(GetConfigurationValue(key), CultureInfo.InvariantCulture);
            }
            catch
            {
                return defaultNum;
            }
        }

        public static int GetEditorMode(int defaultMode) => GetNumber(KeyName.default_mode, defaultMode);

        public static bool SetConfigurationValue(KeyName key, string value)
        {
            try
            {
                if (!File.Exists(PublicConfigFilePath))
                {
                    CopyConfigFile();
                    var fileMap = new ExeConfigurationFileMap()
                    {
                        ExeConfigFilename = PublicConfigFilePath
                    };
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

        public static string PublicConfigFilePath =>
            Path.Combine(PublicFolderPath, Path.GetFileName(Assembly.GetEntryAssembly().Location) + ".config");

        public static string PublicFolderPath =>
            Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Math_Editor_MV");

        private static void CopyConfigFile()
        {
            try
            {
                File.Copy(exePath + ".config", PublicConfigFilePath);
            }
            catch { }
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
                var codeBase = Assembly.GetExecutingAssembly().Location;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static bool SetConfigurationValue_AES(KeyName key, string value)
        {
            try
            {
                return SetConfigurationValue(key, EncryptText(value, "abcd_o82349834jefhdfer8&"));//GetConfigurationValue(KeyName.s01)));
            }
            catch { }
            return false;
        }

        public static string GetConfigurationValue_AES(KeyName key)
        {
            try
            {
                var base64String = GetConfigurationValue(key);
                return DecryptText(base64String, "abcd_o82349834jefhdfer8&");//GetConfigurationValue(KeyName.s01));
            }
            catch
            {
                return string.Empty;
            }
        }

        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using MemoryStream ms = new MemoryStream();
            using RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;

            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            AES.Mode = CipherMode.CBC;

            using var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
            cs.Close();
            return ms.ToArray();
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using var AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;

            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            AES.Mode = CipherMode.CBC;

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
            cs.Close();
            return ms.ToArray();
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

            return Encoding.UTF8.GetString(bytesDecrypted);
        }
    }
}

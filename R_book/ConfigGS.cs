using System;
using System.Configuration;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace R_book
{
    //操作appconfig文件
    class ConfigGS
    {
        /// <summary>
        /// 获取"key"的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        /// <summary>
        /// 修改"key"的值
        /// </summary>
        /// <param name="key"  key></param>
        /// <param name="newValue"  新值></param>
        /// <returns></returns>
        public bool setConfigValue(string key,string newValue)
        {
            try
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                cfa.AppSettings.Settings[key].Value = newValue;
                cfa.Save();
                ConfigurationManager.RefreshSection("appSettings");
                return true;
            }
            catch
            {
                return false;
            }
           
        }

    }

    class WebToStream
    {
        public string getWebData(string url)
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据
                return Encoding.Default.GetString(pageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("网址请求错误\r\n" + ex.Message.ToString());
                return "";
            }
        }

        public string getNewVersion(string url)
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据
                return Encoding.Default.GetString(pageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("网址请求错误\r\n" + ex.Message.ToString());
                return "";
            }
        }
    }
}

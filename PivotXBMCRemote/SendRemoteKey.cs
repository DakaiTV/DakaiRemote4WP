using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.IO.IsolatedStorage;

namespace PivotXBMCRemote
{

    public class SendRemoteKey
    {
        
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
        
        public void SendKey(String data)
        {
            WebClient client = new WebClient();

            // XBMC媒体中心服务器
            String JSON_RPC = "http://" + userSettings["host"] + "/jsonrpc?SendRemoteKey";

            // WebClient 加载完成后的事件
            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);

            try
            {
                // String data = "{\"jsonrpc\": \"2.0\", \"method\": \"Input.Up\", \"id\": 1}";
                client.Headers[HttpRequestHeader.ContentLength] = data.Length.ToString();
                client.UploadStringAsync(new Uri(JSON_RPC), "POST", data);
            }
            catch { }
        }

        private void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                // textBlockDebug.Text = "Successfully.";
                // MessageBox.Show("成功请求：" + userSettings["host"]);
            }
            else
            {
                MessageBox.Show("无法访问XBMC媒体中心服务器："+ userSettings["host"]);
            }
        }
    }
}

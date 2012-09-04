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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace PivotXBMCRemote
{

    public class SendRemoteKey
    {
        
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;

        public void SendKey(String key, String data)
        {
            WebClient client = new WebClient();

            // XBMC媒体中心服务器
            String JSON_RPC = "http://" + userSettings["host"] + "/jsonrpc?SendRemoteKey";

            // WebClient 加载完成后的事件
            // application/json; charset=UTF-8
            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            switch (key)
            {
                case "VolumeUp":
                    client.UploadStringCompleted += new UploadStringCompletedEventHandler(user_VolumeUpCallback);
                    break;
                case "VolumeDown":
                    client.UploadStringCompleted += new UploadStringCompletedEventHandler(user_VolumeDownCallback);
                    break;
                default:
                    client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
                    break;
            }

            try
            {
                // String data = "{\"jsonrpc\": \"2.0\", \"method\": \"Input.Up\", \"id\": 1}";
                client.Headers[HttpRequestHeader.ContentLength] = data.Length.ToString();
                client.UploadStringAsync(new Uri(JSON_RPC), "POST", data);
            }
            catch { }
        }

        private void user_VolumeUpCallback(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                JsonVolume obj = new JsonVolume();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(JsonVolume));
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(e.Result));
                obj = serializer.ReadObject(stream) as JsonVolume;
                stream.Close();
                int volume = obj.result.volume;
                if (volume < 100)
                {
                    volume += 10;
                    if (volume > 100) volume = 100;
                    this.SendKey("Application.SetVolume", "{\"jsonrpc\": \"2.0\", \"method\": \"Application.SetVolume\", \"params\": { \"volume\": "+ volume.ToString() +" }, \"id\": 1}");
                }
            }
            else
            {
                // MessageBox.Show("无法访问XBMC媒体中心服务器：" + userSettings["host"]);
            }
        }

        private void user_VolumeDownCallback(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                JsonVolume obj = new JsonVolume();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(JsonVolume));
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(e.Result));
                obj = serializer.ReadObject(stream) as JsonVolume;
                stream.Close();
                int volume = obj.result.volume;
                if (volume > 0)
                {
                    volume -= 10;
                    if (volume < 0) volume = 0;
                    this.SendKey("Application.SetVolume", "{\"jsonrpc\": \"2.0\", \"method\": \"Application.SetVolume\", \"params\": { \"volume\": " + volume.ToString() + " }, \"id\": 1}");
                }
            }
            else
            {
                // MessageBox.Show("无法访问XBMC媒体中心服务器：" + userSettings["host"]);
            }
        }

        private void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                // textBlockDebug.Text = "Successfully.";
                // MessageBox.Show("成功请求：" + userSettings["host"]);
                // StreamReader reader = new StreamReader(e.Result);
                // String contents = reader.ReadToEnd();
                // MessageBox.Show(e.Result);
                //reader.Close();
            }
            else
            {
                // MessageBox.Show("无法访问XBMC媒体中心服务器："+ userSettings["host"]);
            }
        }
    }
}

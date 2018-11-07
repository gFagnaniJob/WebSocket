using System;
using System.IO;
using WebSocketSharp;
using Newtonsoft.Json;
using System.Text;
using System.Threading;

namespace WebSocketClient
{
    class Program
    {
        static Foo f;
        static bool file = false;

        static void Main(string[] args)
        {

            try
            {
                using (var ws = new WebSocket("ws://localhost:1337"))
                {
                    ws.OnMessage += OnMessageHandler;
                    ws.Connect();
                    ws.Send("Connection UP");
                    byte[] fileBytes = File.ReadAllBytes("test.json");
                    while (true)
                    {
                        if (f != null)
                        {
                            if (!file)
                                ws.Send("key = " + f.key);
                            else

                                ws.Send(fileBytes);
                            file = !file;
                            Thread.Sleep(2000);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection Lost");
                Console.ReadKey(true);
            }

        }

        private static void OnMessageHandler(object sender, MessageEventArgs e)
        {
            if (e.IsText)
            {
                Console.WriteLine("\nFROM STRING");
                string json = e.Data;
                Console.WriteLine("\tReceived: " + json);
                f = JsonConvert.DeserializeObject<Foo>(json);
                Console.WriteLine("\tkey = " + f.key);
            }
            else if (e.IsBinary)
            {
                Console.WriteLine("\nFROM JSON FILE");
                byte[] bytes = e.RawData;
                var json = Encoding.UTF8.GetString(bytes);
                Foo f = JsonConvert.DeserializeObject<Foo>(json);
                json = json.Remove(json.Length - 1, 1);
                Console.WriteLine("\tReceived: " + json);
                Console.WriteLine("\tkey = " + f.key);
            }
        }
    }
}

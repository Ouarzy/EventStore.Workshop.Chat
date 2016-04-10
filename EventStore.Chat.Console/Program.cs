using System.Linq;
using System.Net;
using System.Threading;
using Core;

namespace EventStore.Chat.Console
{
    class Program
    {
        private static bool Running = true;
        static void Main(string[] args)
        {
            var user = args[0];
            var chatRoom = args[1];
            var serverName = args.Length > 2
                ? args[3]
                : "ubuntustore.cloudapp.net";
            IPAddress[] ipAddresses = Dns.GetHostAddresses(serverName);
            Chatter.Init(ipAddresses.First().ToString());
            Chatter.Subscribe(
                chatRoom,
                OnRecieved);
            while (Running)
            {
                if (Chatter.Connected)
                {
                    var message = System.Console.ReadLine();
                    if (message == "!q")
                        Running = false;
                    else
                        Chatter.SendMessage(message, user, chatRoom);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private static void OnRecieved(ChatMessage message)
        {
            var text = string.Format(
                "{0} says:\n{1}",
                message.User,
                message.Message);

            System.Console.WriteLine(text);
        }
    }
}

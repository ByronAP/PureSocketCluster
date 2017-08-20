using System;
using PureSocketCluster;
using PureWebSockets;

namespace PureSocketClusterTest
{
    public class Program
    {
        private static PureSocketClusterSocket _scc;

        public static void Main(string[] args)
        {
            // input credentials if used, different systems use different auth systems this however is the most common (passing 'auth' event with your credentials)
            var creds = new Creds
            {
                apiKey = "your apikey if used",
                apiSecret = "your api secret if used"
            };

            // initialize the client
            _scc = new PureSocketClusterSocket("wss://sc-02.coinigy.com/socketcluster/", new ReconnectStrategy(4000, 60000), creds);

            // hook up to some events
            _scc.OnOpened += Scc_OnOpened;
            _scc.OnMessage += _scc_OnMessage;
            _scc.OnStateChanged += _scc_OnStateChanged;
            _scc.OnSendFailed += _scc_OnSendFailed;
            _scc.OnError += _scc_OnError;
            _scc.OnClosed += _scc_OnClosed;
            _scc.OnData += _scc_OnData;
            _scc.OnFatality += _scc_OnFatality;
            var res = _scc.ConnectAsync().Result;

            // subscribe to some channels
            var cn = _scc.CreateChannel("TRADE-PLNX--BTC--ETC").Subscribe();
            cn.OnMessage(TradeData);
            var cn0 = _scc.CreateChannel("TRADE-PLNX--BTC--ETH").Subscribe();
            cn0.OnMessage(TradeData);
            var cn1 = _scc.CreateChannel("TRADE-OK--BTC--USD").Subscribe();
            cn1.OnMessage(TradeData);

            Console.ReadLine();
        }

        private static void _scc_OnFatality(string reason)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Fatality: {reason}");
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void _scc_OnData(byte[] data)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Binary: {data}");
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void _scc_OnClosed()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Socket Closed");
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void _scc_OnError(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Error: {ex}");
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void _scc_OnSendFailed(string data, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"send failed: {data} Ex: {ex}");
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void _scc_OnStateChanged(bool connected)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Connection state changed to {connected}");
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void TradeData(string name, object data)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(name + ": " + data);
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void _scc_OnMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("");
        }

        private static void Scc_OnOpened()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Opened");
            Console.ResetColor();
            Console.WriteLine("");
        }
    }
}
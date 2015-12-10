using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Net.Sockets.SocketException;
using MyRefrence;
namespace client
{

    class Program
    {

        static void Main(string[] args)
        {
            //send object
          myMessage  m1 = new myMessage(); 

            m1.asd = "zeko";
            m1.seq_no = 1;
            IFormatter formatter = new BinaryFormatter();


            byte[] bytes = null;




            using (var mystream = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(mystream, m1);
                bytes = mystream.ToArray();

            }
            //bytes= Encoding.ASCII.GetBytes("asda aaaaaaaaaaaaaaaaaaa ");




            byte[] data = new byte[1024];
            string input, stringData;
            int recv;

            IPEndPoint endpoint = new IPEndPoint(
                            IPAddress.Parse("127.0.0.1"), 950);

            Socket server = new Socket(AddressFamily.InterNetwork,
                           SocketType.Dgram, ProtocolType.Udp);


            string welcome = "Hello, are you there?";
        //    Console.WriteLine(welcome);
         //   data = Encoding.ASCII.GetBytes(welcome);

            Console.WriteLine("msg = "+ Encoding.ASCII.GetString( bytes) );
           server.SendTo(bytes, bytes.Length, SocketFlags.None, endpoint);


           // server.SendTo(data, data.Length, SocketFlags.None, endpoint);
            // Console.WriteLine("welcome1");
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)sender;

          //  server.SendTo(data, data.Length, SocketFlags.None, endpoint);

            data = new byte[1024];
            recv = server.ReceiveFrom(data, ref Remote);
            // Console.WriteLine("welcome3");
            Console.WriteLine("Message received from {0}:", Remote.ToString());
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
            
          //  add packet header ( zero or one ) to string
            //server.SendTo(data, data.Length, SocketFlags.None, endpoint);
            //wait 
            //if(0 received ) go to
            //resend packet
            // if 1 received 
            // send next packet


            while (true)
            {
                input = Console.ReadLine();
                // Console.WriteLine("welcome5");
                if (input == "exit")
                    break;
                server.SendTo(Encoding.ASCII.GetBytes(input), Remote);

                data = new byte[1024];
                recv = server.ReceiveFrom(data, ref Remote);
                stringData = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine(stringData);
            }
            Console.WriteLine("Stopping client");

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MyRefrence;
namespace server
{
    class Program
    {

       

        static void Main(string[] args)
        {
            //rrrr
            int x;
            int recv;
            byte[] data = new byte[1024];

            myMessage msg_recv=null;
            //LISTEN TO ANY ip on port 950
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 950);
            Socket newsocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //BIND ANY CONNECTION ON ENDPOINT TO NEWSOCKET
            newsocket.Bind(endpoint);
            Console.WriteLine("waiting for a client ...");

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);

            //WAIT FOR CLIENT TO SEND DATA
            recv = newsocket.ReceiveFrom(data, ref Remote);

          
            Console.WriteLine("avalaible: " + newsocket.Available);
            Console.WriteLine("received: " + Encoding.ASCII.GetString(data));

            using (var mystream = new MemoryStream(data))
            {
               var bf = new BinaryFormatter();
              msg_recv = (myMessage)bf.Deserialize(mystream);
            }

            Console.WriteLine("Message received from {0}:", Remote.ToString());
            Console.WriteLine(msg_recv.asd + " seq_no: " + msg_recv.seq_no);

            string welcome = "Welcome to my test server";
            Console.WriteLine(welcome);
            data = Encoding.ASCII.GetBytes(welcome);


            newsocket.SendTo(data, Remote);

            //  newsocket.SendTo(data);


            while (true)
            {

                data = new byte[1024];
                recv = newsocket.ReceiveFrom(data, ref Remote);
                if (recv == 0)
                    break;
                newsocket.SendTo(data, Remote);
                //newsocket.Send(data);
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));



                // Console.ReadLine();



            }
            Console.ReadLine();

        }
    }
}




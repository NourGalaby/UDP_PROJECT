//using System.Net.Sockets.SocketException;
using MyRefrence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
namespace client
{

    class Program
    {

        static object deserialize(byte[] data)
        {

           
            using (var mystream = new MemoryStream(data))
                {
                var bf = new BinaryFormatter();
                return bf.Deserialize(mystream);
            }

        }


  


        static void Main(string[] args)
        {
           
    
byte[] bytes = null;



myMessage m1 = new myMessage();
           

using (var mystream = new MemoryStream())
{
    var bf = new BinaryFormatter();
    bf.Serialize(mystream, m1);
    bytes = mystream.ToArray();

}
bytes= Encoding.ASCII.GetBytes("asda aaaaaaaaaaaaaaaaaaa ");


            byte[] data = new byte[1024];
            string input, stringData="";
            int recv;

            IPEndPoint endpoint = new IPEndPoint(
                            IPAddress.Parse("127.0.0.1"),   950   );

            Socket server = new Socket(AddressFamily.InterNetwork,
                           SocketType.Dgram, ProtocolType.Udp);


            string welcome = "Hello, are you there?";
        //    Console.WriteLine(welcome);
         //   data = Encoding.ASCII.GetBytes(welcome);

           // Console.WriteLine("msg = "+ Encoding.ASCII.GetString( bytes) );
           server.SendTo(bytes, bytes.Length, SocketFlags.None, endpoint);


           // server.SendTo(data, data.Length, SocketFlags.None, endpoint);
            // Console.WriteLine("welcome1");
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)sender;

          //  server.SendTo(data, data.Length, SocketFlags.None, endpoint);



            data = new byte[1024];

            int Size=0;
            
            try
            {
                recv = server.ReceiveFrom(data, ref Remote);
                Size = int.Parse(Encoding.ASCII.GetString(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Maybe server is not running");
                
             //   return;
            }
            // Console.WriteLine("welcome3");
            Console.WriteLine("Message received from {0}:", Remote.ToString());
        //    Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
            
          //  add packet header ( zero or one ) to string
            //server.SendTo(data, data.Length, SocketFlags.None, endpoint);
            //wait 
            //if(0 received ) go to
            //resend packet
            // if 1 received 
            // send next packet



            for (int i = 0; i < Size; i++) 
            {
              
            
                recv = server.ReceiveFrom(data, ref Remote);


                myMessage msg_rec = (myMessage) deserialize(data);

               
              //i-  server.SendTo(Encoding.ASCII.GetBytes("0"), Remote);

             
                try
                {
                    stringData = Encoding.ASCII.GetString(msg_rec.data);
                }
                catch(Exception e) {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine(i+" "+stringData);
            }
            Console.WriteLine("Stopping client");
            Console.ReadLine();
        }

    }

}
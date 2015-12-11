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

      static  byte[] serialize(object obj)
        {

            using (var mystream = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(mystream, obj);
                return mystream.ToArray();

            }
        }




        static void Main(string[] args)
        {
           
        
            int recv;
            byte[] data = new byte[1024];
            byte[] bytes = new byte[1024];
            myMessage[] msgs=new myMessage[20];

            for (int i = 0; i < 20; i++)
            {

            msgs[i]= new myMessage();

            msgs[i].data = Encoding.ASCII.GetBytes(i*111+"HELLO ITS ME ");
            }
    


   

            //LISTEN TO ANY ip on port 950
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 950);
            Socket newsocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);

   
            //BIND ANY CONNECTION ON ENDPOINT TO NEWSOCKET
            newsocket.Bind(endpoint);
            Console.WriteLine("waiting for a client ...");

      

            //WAIT FOR CLIENT TO Request data 
            recv = newsocket.ReceiveFrom(data, ref Remote);
        


                //store end point in Remote
            int Size = 20;
            newsocket.SendTo(Encoding.ASCII.GetBytes(Size.ToString()), Remote);


          //  Console.WriteLine("avalaible: " + newsocket.Available);
          //  Console.WriteLine("received: " + Encoding.ASCII.GetString(data));

            ////using (var mystream = new MemoryStream(data))
            ////{
            ////   var bf = new BinaryFormatter();
            ////  msg_recv = (myMessage)bf.Deserialize(mystream);
            ////}

            Console.WriteLine("Message received from {0}:", Remote.ToString());
        

        //    string welcome = "Welcome to my test server";
           // Console.WriteLine(welcome);
           // data = Encoding.ASCII.GetBytes(welcome);


      


            //  newsocket.SendTo(data);
            int tries = 0;
        newsocket.ReceiveTimeout=1000;
            for (int i = 0; i < 20; i++) 
            {
                //covert object to data 
             data= serialize(msgs[i]);

  
             newsocket.SendTo(data, Remote);

             Console.WriteLine("Waiting for Ack");
              


               try
               {
                   recv = newsocket.ReceiveFrom(data, ref Remote);

               }
               catch (SocketException e)
               {

                   if (e.ErrorCode == 10060)
                   {

                       if (tries >= 3)
                       {

                           System.Console.WriteLine("Disconnected");
                           Console.ReadLine();
                           return;
                       }
                       System.Console.WriteLine("Timed out.. Trying again");
                       i--;
                       tries++;
                    
                   }

               } 
                //newsocket.Send(data);
              //  Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));



                // Console.ReadLine();

            }
            Console.ReadLine();

        }
    }

}





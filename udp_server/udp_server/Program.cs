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


        public static int PORT;

        public static int size;

        public static int random_seed;


     public  static  int BUFFER_SIZE=512-8;
       
    static int plp=2;

      static  byte[] serialize(object obj)
        {

            using (var mystream = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(mystream, obj);
                return mystream.ToArray();

            }
        }



    static  bool simulateLoss()
      {
          Random rand = new Random();

        int x = rand.Next(1, 101);
        //  Console.WriteLine(x);
              if (x >= plp)
              {
                  return true;
              }
            //  Console.WriteLine("LOSS");
              return false;
      }

    static public void serverin(string file)
    {


  
        var reader = new System.IO.StreamReader(file, System.Text.Encoding.UTF8);
        var text = reader.ReadLine();
        PORT = Convert.ToInt32(text);

        string[] line = new string[17];
        int j = 0;
        while ((text = reader.ReadLine()) != null)
        {


            line[j] = text;

            j++;
        }
        size = Convert.ToInt32(line[0]);

        random_seed = Convert.ToInt32(line[1]);
        float probability = float.Parse(line[2]);
        Console.WriteLine("port_server" + " " + PORT);
        Console.WriteLine("size" + " " + size);

        Console.WriteLine("random_seed" + " " + random_seed);

        Console.WriteLine(" probability" + " " + probability);


        // Console.ReadLine();

    }

      static void startChild(string filename, EndPoint Remote , Socket newsocket )
      {
         List< myMessage> msgs = chunks.streamfile(filename);
         int current_seq = msgs[0].seq_no ;
            
          byte[] data = new byte[BUFFER_SIZE];
          int recv;
       
          int Size = msgs.Count;


       
          newsocket.SendTo(Encoding.ASCII.GetBytes(Size.ToString()), Remote);
    
          //connected
          Console.WriteLine("Message received from {0}:", Remote.ToString());

          Console.WriteLine("No Of Packets =" + Size);

          //  newsocket.SendTo(data);
          int tries = 0;
         
          newsocket.ReceiveTimeout = 100;
          for (int i = 0; i < Size; i++)
          {

              Console.WriteLine("Sending Packet no:" + i);
              //covert object to data 
              current_seq = msgs[i].seq_no;
              data = serialize(msgs[i]);

              if (simulateLoss())
              {
                  newsocket.SendTo(data, Remote);
              }
              else { Console.WriteLine("Loss"); }
              Console.WriteLine("Waiting for Ack "+ current_seq);



              try
              {
                  recv = newsocket.ReceiveFrom(data, ref Remote);
                  //check seq no
                  tries = 0;
                  byte ack = data[0];
                 
                  if (ack == current_seq)
                  {
                      //if same sequnce number
                      Console.WriteLine("same seq no");
                      throw new SocketException() ;
                  }
                  //bug fix for reseting tries no
                 
              }
              catch (SocketException e)
              {

                  if (e.ErrorCode == 10060)
                  {
                      System.Console.WriteLine("Timed out.. Trying again");
                      if (tries >= 5)
                      {

                          System.Console.WriteLine("Disconnected, Max time out reached");
                          Console.ReadLine();
                          return;
                      }
                   
                 

                  }
                  i--;
                  tries++;
              }
              //newsocket.Send(data);
              //  Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));



              // Console.ReadLine();

          }
      }

        static void Main(string[] args)
        {


           
            int recv;
          
            byte[] data = new byte[1024];
           

            Socket newsocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      

            serverin("files/serverin.txt");
   

            //LISTEN TO ANY ip on port 950
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, PORT);
      

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);

   
            //BIND ANY CONNECTION ON ENDPOINT TO NEWSOCKET
            newsocket.Bind(endpoint);
            Console.WriteLine("waiting for a client ...");

      

            //WAIT FOR CLIENT TO Request data 
            recv = newsocket.ReceiveFrom(data, ref Remote);
            //store end point in Remote
            string filename = Encoding.ASCII.GetString(data);
             filename = filename.Replace("\0", string.Empty);

            Console.WriteLine("Received request for file: " + filename);


            startChild(filename, Remote, newsocket);


            Console.WriteLine("Sending Complete");

            Console.ReadLine();

        }
    }

}





﻿//using System.Net.Sockets.SocketException;
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

        static int plp = 2;

        static bool simulateLoss()
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


        static int BUFFER_SIZE = 512;
        static object deserialize(byte[] data)
        {


            using (var mystream = new MemoryStream(data))
            {
                var bf = new BinaryFormatter();
                return bf.Deserialize(mystream);
            }

        }



        static int zero_one(int x)
        {
            if (x == 1)
                return 0;
            else return 1;

        }

        static public void bytestofile(List<myMessage> splitted, string fileName)
        {



            int size = splitted.Count * BUFFER_SIZE;

            byte[] file = new byte[size];
            int x = 0;
            foreach (myMessage y in splitted)
            {
                Array.Copy(y.data, 0, file, x, y.data.Length);
                x += y.data.Length;
            }

            using (FileStream
                fileStream = new FileStream(fileName, FileMode.Create))
            {
                // Write the data to the file, byte by byte.
                for (int i = 0; i < file.Length; i++)
                {
                    fileStream.WriteByte(file[i]);
                }
                fileStream.Seek(0, SeekOrigin.Begin);
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
            //string filename = "SamSmith.mp3";
            string filename = "SamSmith.mp3";

            bytes = Encoding.ASCII.GetBytes(filename);


            byte[] data = new byte[1024];
            int recv;

            IPEndPoint endpoint = new IPEndPoint(
                            IPAddress.Parse("127.0.0.1"), 950);

            Socket server = new Socket(AddressFamily.InterNetwork,
                           SocketType.Dgram, ProtocolType.Udp);
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

            int Size = 0;

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


            //      byte current_seq = 1;
            List<myMessage> messages = new List<myMessage>();

            byte[] ack = new byte[1];
            ack[0] = 1;

            int k = 0;
            for (int i = 0; i < Size; i++)
            {

                //receive data
                recv = server.ReceiveFrom(data, ref Remote);

                //convert from data to Message object
                myMessage msg_rec = (myMessage)deserialize(data);

                if (msg_rec.seq_no == ack[0])
                {
                    i--;
                    Console.WriteLine("DUPLICATE Packet");
                    //duplicate packet... dont add to list

                    if (simulateLoss())
                    {
                        // string  ack =zero_one(msg_rec.seq_no).ToString();

                        //send same ack again

                        //reverse ack , send, then reverse
                        ack[0] = (byte)zero_one(ack[0]);
                        server.SendTo(ack, 1, SocketFlags.None, Remote);
                        Console.WriteLine("Sent ACk  " + ack[0]);
                        ack[0] = (byte)zero_one(ack[0]);

                    }
                    else
                    {
                        ack[0] = (byte)zero_one(ack[0]);
                        Console.WriteLine("Loss ");

                    
                        ack[0] = (byte)zero_one(ack[0]);
                    }

                }
                else
                {
                    // put in a list
                    Console.WriteLine("Received Packet");
                    messages.Add(msg_rec);

                    //send ACK
                    if (simulateLoss())
                    {
                        // string  ack =zero_one(msg_rec.seq_no).ToString();
                        server.SendTo(ack, 1, SocketFlags.None, Remote);

                        Console.WriteLine("Sent ACk " + ack[0]);
                        int x = zero_one(ack[0]);
                        ack[0] = (byte)x;


                    }
                    else
                    {
                        Console.WriteLine("Loss ");
                        ack[0] = (byte)zero_one(ack[0]);
                    }

                }






            }

            //here all data is done receving so 
            //we want to covert from data to file
            // so  use the function you made to convert it to files

            bytestofile(messages, filename);


            Console.WriteLine("Stopping client");
            Console.ReadLine();
        }

    }

}
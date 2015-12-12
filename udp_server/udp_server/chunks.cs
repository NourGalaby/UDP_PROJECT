using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MyRefrence;
namespace server
{
    class chunks
    {
      static  public List<myMessage> streamfile(string filename)
        {
            byte[] file;
            try
            {
                Console.WriteLine(filename);
               file = File.ReadAllBytes("files/"+filename);

            }
            catch(Exception e)
            {

                Console.WriteLine("error openeing file: "  );
                Console.ReadLine();
                return null;
            }

        //    Console.WriteLine(Encoding.Default.GetString(file));
            int file_length = file.Length;
         //   Console.WriteLine(file_length);
            int chunk_length = Program.BUFFER_SIZE ;
           
            List<myMessage> splitted = new List<myMessage>();
            myMessage buffer = new myMessage();
            int k=2;
            for (int i = 0; i < file_length; i =i+chunk_length)
            {
               // byte[] buffer = new byte[chunk_length];
                buffer = new myMessage();
                buffer.data = new byte[chunk_length];
                buffer.seq_no = k++ % 2;  //add 1 and 0 sequece no
                
                if (file_length < i + chunk_length)
                {
                    chunk_length = file_length - i;
                }
                Array.Copy(file, i, buffer.data, 0, chunk_length);

                splitted.Add(buffer);
                // Console.WriteLine(buffer[j]);
              //  Console.WriteLine(Encoding.Default.GetString(buffer.data));
              // return buffer;
                // Console.WriteLine("j:"+j+" "+Encoding.Default.GetString(buffer));
                // j++;
            }
           // foreach (myMessage x in splitted)
            return splitted ;
          //  Console.ReadLine();

        }
      
    }
}

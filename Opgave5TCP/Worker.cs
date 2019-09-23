using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.Win32.SafeHandles;
using Opgave1UnitTest;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Opgave5TCP
{
    public class Worker
    {
        private static List<Bog> books = new List<Bog>()
        {
            new Bog("Resul", 15, "ABJDILEKDJ32J", "Deez nuts"),
            new Bog("Resul", 15, "ABJDILEKDJ32J", "Deez nuts"),
            new Bog("Resul", 15, "ABJDILEKDJ32J", "Deez nuts"),
            new Bog("Resul", 15, "YHDLOKJMHNBFH", "Deez nuts"),
            new Bog("Resul", 15, "HJDOSKLDMNI25", "Deez nuts")
        };

        public void Start()
        {
            TcpListener socket = new TcpListener(IPAddress.Loopback, 4646);

            try
            {
                socket.Start();
                Console.WriteLine("Server is listening");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            while (true)
            {
                TcpClient client = socket.AcceptTcpClient();

                Task.Run(() =>
                {
                    TcpClient tempsocket = client;
                    DoClient(tempsocket);
                });

            }





        }


        public void DoClient(TcpClient tempsocket)
        {

            using (StreamReader reader = new StreamReader(tempsocket.GetStream()))
            using (StreamWriter writer = new StreamWriter(tempsocket.GetStream()))
            {

                while (true)
                {
                    string readString = reader.ReadLine();
                    string[] myline = readString.Split(" ");

                    if (myline[0] == "Hent" && myline[1] == "Alle")
                    {
                        foreach (Bog bok in books)
                        {
                            string jsonBog = JsonConvert.SerializeObject(bok);
                            Console.WriteLine(jsonBog);
                            writer.WriteLine(jsonBog);



                        }


                    }
                    else if (myline[0] == "Hent" && myline[1].Length == 13)
                    {
                        Bog bogJson = sortList(myline[1]);
                        string jsonSend = JsonConvert.SerializeObject(bogJson);
                        Console.WriteLine(jsonSend);
                        writer.WriteLine(jsonSend);

                    }
                    else if (myline[0] == "Gem")
                    {

                        string sr = reader.ReadLine();
                        Bog bog = JsonConvert.DeserializeObject<Bog>(sr);
                        books.Add(bog);
                    }
                    writer.Flush();
                    
                  
                }

         











            }

            
        }

        public Bog sortList(string myarray)
        {
            Bog mybog = new Bog();
            foreach (var bog in books)
            {
                if (bog.Isbn13 == myarray)
                {
                    mybog = bog;
                }
            }

            return mybog;
        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyConsole;
using YarpManagerCS;
using Sense.Lib.FACELibrary;
using System.Globalization;
using System.IO;
namespace TerminalFace
{
    public class Runner
    {
        static YarpPort yarpPortLookAt;
        static YarpPort yarpPortECS;

        static void Main(string[] args)
        {


            InitYarp();


            new TerminalMenu().Run();



            //while (true) // Loop indefinitely
            //{
            //    Console.WriteLine("Welcome in terminal faceEnter input:"); // Prompt


            //    Console.WriteLine("Enter input:"); // Prompt
            //    string line = Console.ReadLine(); // Get string from user
            //    string[] command = line.Split(' ');
            //    if (command[0] == "lookat")
            //    { }
            //    else if (command[0] == "ecs") { }
            //    else if (command[0] == "sleep") { }
            //    else if (command[0] == "script") { }

            //    else if (command[0] == "help") { }

            //    else if (command[0] == "exit")
            //        break;
            //    Console.Write("You typed "); // Report output
            //    Console.Write(line.Length);
            //    Console.WriteLine(" character(s)");
            //}
        }


        private static void InitYarp()
        {


            yarpPortLookAt = new YarpPort();
            yarpPortLookAt.openSender("/TerminalFace/LookAt:o");

            yarpPortECS = new YarpPort();
            yarpPortECS.openSender("/TerminalFace/ECS:o");


            if (yarpPortECS.NetworkExists())
            {
                Output.WriteLine("Yarp Server is running...");

            }

        }

        
        public static void sendECS(decimal v, decimal a)
        {
            FaceExpression f = new FaceExpression((float)v, (float)a);
            yarpPortECS.sendData(ComUtils.XmlUtils.Serialize<FaceExpression>(f));

        }
        public static void sendLookAt(decimal x, decimal y, decimal z)            // decimal (z)   ALTILIA
        {
            Winner w = new Winner(0, (float)x, (float)y, (float)z);               // FLOAT (Z)     ALTILIA
            yarpPortLookAt.sendData(ComUtils.XmlUtils.Serialize<Winner>(w));

        }
        public static void sendScript(string path)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");

            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    switch (line.Split(' ')[0])
                    {
                        case "ecs":
                            Output.WriteLine("Send command: "+line);
                            sendECS(Convert.ToDecimal(line.Split(' ')[1],culture), Convert.ToDecimal(line.Split(' ')[2],culture));
                            break;
                        case "lookat":
                            Output.WriteLine("Send command: " + line);

                            sendLookAt(Convert.ToDecimal(line.Split(' ')[1],culture), Convert.ToDecimal(line.Split(' ')[2],culture), Convert.ToDecimal(line.Split(' ')[3], culture));     // aggiunta campo 3   ALTILIA
                            break;
                        case "sleep":
                            Output.WriteLine("Timer: "+line);

                            System.Threading.Thread.Sleep(Convert.ToInt32(line.Split(' ')[1]));
                            break;
                        default:
                            Output.WriteLine(line);
                            break;
                    }
                }
            }
        }

    }



}

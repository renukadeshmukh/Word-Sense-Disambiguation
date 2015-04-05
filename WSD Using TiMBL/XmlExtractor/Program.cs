using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace XmlExtractor
{
    class Program
    {
        //method to extract features as per SETA used REGEX HEere . Kept for reference
        public static void ExtractFeaturesSetA(String context)
        {
            // Here we call Regex.Match.
            Match match = Regex.Match(context, @"&&([A-Za-z]+)&&",
                RegexOptions.IgnoreCase);

            // Here we check the Match instance.
            if (match.Success)
            {
                // Finally, we get the Group value and display it.
                string key = match.Groups[1].Value;
                //Console.WriteLine(key);
                //WriteToFile("--SubSubName: " + key);
            }
            else
            {
                //WriteToFile("!!!! REGEX Failure" + match.Value);
            }
        }

        public static void ExtractFeaturesTypeSetA(String context, List<string> senseIds,String fileName)
        {
            string[] words = context.Split(' ');
            int len = words.Length;
            string wl2 = "_", wl1 = "_", w0 = "_", wr1 = "_", wr2 = "_";
            int j = 0;
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Contains("%%"))
                {
                    j = i;

                    break;
                }
            }
            #region BuildMap
            if (j == 0)
            {

                wr1 = words[1];
                wr2 = words[2];
            }
            else if (j == 1)
            {
                wl1 = words[0];
                wr1 = words[2];
                wr2 = words[3];
            }
            else if (j == len - 1)
            {
                wl2 = words[len - 3];
                wl1 = words[len - 2];

            }
            else if (j == len - 2)
            {
                wl2 = words[len - 4];
                wl1 = words[len - 3];
                wr1 = words[len - 1];
            }
            else
            {
                wl2 = words[j - 2];
                wl1 = words[j - 1];
                wr1 = words[j + 1];
                wr2 = words[j + 2];
            }

            #endregion
            //THIS PART OF CODE MIGHT CHANGE
            string[] endOfLine = { ".", "!", "?" };

            if (endOfLine.Contains(wl2))
            {
                wl2 = "_";
            }
            if (endOfLine.Contains(wl1))
            {
                wl2 = "_";
                wl1 = "_";
            }
            if (endOfLine.Contains(wr1))
            {
                wr1 = "_";
                wr2 = "_";
            }
            if (endOfLine.Contains(wr2))
            {
                wr2 = "_";
            }

            foreach (var item in senseIds)
            {
                string SetALine = string.Concat(wl2, " ",
                                                  wl1, " ",
                                                  wr1, " ",
                                                  wr2, " ",
                    //"(", wl2, " ", wl1, ")", " ",
                    //"(", wl1, " ", wr1, ")", " ",
                    //"(", wr1, " ", wr2, ")", " ",

                                                  wl2, wl1, " ",
                                                  wl1, wr1, " ",
                                                  wr1, wr2, " ",

                                                  item);
                Console.WriteLine(SetALine);
                WriteToFile(SetALine, fileName);
            }

        }

        //method for writing to file.
        public static void WriteToFile(String input, String fileName)
        {
            string path = @"G:\GitHub\XmlExtractor\XmlExtractor\" + fileName;
            // This text is added only once to the file. 
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(input);
                }
            }
            else
            {

                // This text is always added, making the file longer over time 
                // if it is not deleted. 
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(input);
                }
            }
            // Open the file to read from. 
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
        }

        static void Main(string[] args)
        {
           CreateTrainFile();

           GenerateKeyXml();

           CreateTestFile();
           Console.ReadKey();
        }

        static List<String> GetSenseIdFromKeyFile(string id)
        {
            List<String> senseids = new List<string>();
            XElement root = XElement.Load(@"G:\GitHub\XmlExtractor\XmlExtractor\KeyXml.xml");
            IEnumerable<XElement> keys =
                from el in root.Elements("key")
                where string.Equals((string)el.Attribute("id"), id)
                select el;
            foreach (XElement el in keys)
            {
                String str = (string)el.Attribute("senseid");
                senseids.AddRange(str.Split(' ').ToList());
            }
            return senseids;

        }

        static void CreateTestFile()
        {
            string path = @"G:\GitHub\XmlExtractor\XmlExtractor\MyTest.txt";
            // This text is added only once to the file. 
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            XElement po = XElement.Load(@"G:\GitHub\XmlExtractor\XmlExtractor\Xtract.test.xml");
            IEnumerable<XElement> lexelts =
                from el in po.Descendants("lexelt")
                select el;
            foreach (XElement el in lexelts)
            {   //Iterating on lexelt
                IEnumerable<XElement> instance =
                  from ins in el.Descendants("instance")
                  select ins;
                foreach (XElement ins in instance)
                {   //iterating on instance element
                    String id = (string)ins.Attribute("id");
                    List<String> senseids = GetSenseIdFromKeyFile(id);
                    IEnumerable<XElement> context =
                      from cont in ins.Descendants("context")
                      select cont;
                    foreach (XElement cont in context)
                    {   //iterating on context element
                        String str = cont.Value.ToString();
                        ExtractFeaturesTypeSetA(str, senseids, "MyTest.txt");
                    }
                }
            }
            Console.ReadKey();


        }

        static void GenerateKeyXml()
        {
            string path = @"G:\GitHub\XmlExtractor\XmlExtractor\KeyXml.xml";
            // This text is added only once to the file. 
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("<home>");
            }
            String[] lines = File.ReadAllLines(@"G:\GitHub\XmlExtractor\XmlExtractor\EnglishLS.test.key");
            foreach (var line in lines)
            {
                string[] words = line.Split(' ');
                String senseid = "";
                for (int i = 2; i < words.Length; i++)
                {
                    senseid = senseid + words[i] + " ";
                }
                senseid = senseid.Trim();
                string keyLine = string.Concat("<key item=\"", words[0], "\"", " id=\"", words[1], "\" senseid=\"", senseid, "\" />");
                
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(keyLine);
                }

            }
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("</home>");
            }

        }

        static void CreateTrainFile()
        {
            string path = @"G:\GitHub\XmlExtractor\XmlExtractor\MyTrain.txt";
            // This text is added only once to the file. 
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            XElement po = XElement.Load(@"G:\GitHub\XmlExtractor\XmlExtractor\Xtract.train.xml");
            IEnumerable<XElement> lexelts =
                from el in po.Descendants("lexelt")
                select el;
            foreach (XElement el in lexelts)
            {   //Iterating on lexelt
                IEnumerable<XElement> instance =
                  from ins in el.Descendants("instance")
                  select ins;
                foreach (XElement ins in instance)
                {   //iterating on instance element
         
                    List<string> senseIds = new List<string>();
                    IEnumerable<XElement> answer =
                      from ans in ins.Descendants("answer")
                      select ans;
                    foreach (XElement ans in answer)
                    {   //iterating on answer element
                        senseIds.Add((String)ans.Attribute("senseid"));
                    }
                    IEnumerable<XElement> context =
                      from cont in ins.Descendants("context")
                      select cont;
                    foreach (XElement cont in context)
                    {   //iterating on context element
                        String str = cont.Value.ToString();
                        ExtractFeaturesTypeSetA(str, senseIds,"MyTrain.txt");
                       
                    }
                }
            }
            Console.ReadKey();
        }

    }
}

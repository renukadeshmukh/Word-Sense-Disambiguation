using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SVMFeatureExtractor
{
    class Program
    {
        static Dictionary<string, int> FeatureMap = new Dictionary<string, int>();
        static Dictionary<string, int> ClassMap = new Dictionary<string, int>();


        public static void ExtractFeaturesTypeSetA(String context, List<string> senseIds, String fileName)
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


            #region FeatureMap
            Dictionary<int, int> featureNums = new Dictionary<int, int>();
            string f1 = wl2;
            string f2 = wl1;
            string f3 = wr1;
            string f4 = wr2;
            string f5 = string.Concat(wl2, wl1);
            string f6 = string.Concat(wl1, wr1);
            string f7 = string.Concat(wr1, wr2);


            if (FeatureMap.ContainsKey(f1))
            {
                if (!featureNums.ContainsKey(FeatureMap[f1]))
                    featureNums.Add(FeatureMap[f1], 1);
                else featureNums[FeatureMap[f1]] = ++featureNums[FeatureMap[f1]];
            }
            if (FeatureMap.ContainsKey(f2))
            {
                if (!featureNums.ContainsKey(FeatureMap[f2]))
                    featureNums.Add(FeatureMap[f2], 1);
                else featureNums[FeatureMap[f2]] = ++featureNums[FeatureMap[f2]];
            }
            if (FeatureMap.ContainsKey(f3))
            {
                if (!featureNums.ContainsKey(FeatureMap[f3]))
                    featureNums.Add(FeatureMap[f3], 1);
                else featureNums[FeatureMap[f3]] = ++featureNums[FeatureMap[f3]];
            }
            if (FeatureMap.ContainsKey(f4))
            {
                if (!featureNums.ContainsKey(FeatureMap[f4]))
                    featureNums.Add(FeatureMap[f4], 1);
                else featureNums[FeatureMap[f4]] = ++featureNums[FeatureMap[f4]];
            }
            if (FeatureMap.ContainsKey(f5))
            {
                if (!featureNums.ContainsKey(FeatureMap[f5]))
                    featureNums.Add(FeatureMap[f5], 1);
                else featureNums[FeatureMap[f5]] = ++featureNums[FeatureMap[f5]];
            }
            if (FeatureMap.ContainsKey(f6))
            {
                if (!featureNums.ContainsKey(FeatureMap[f6]))
                    featureNums.Add(FeatureMap[f6], 1);
                else featureNums[FeatureMap[f6]] = ++featureNums[FeatureMap[f6]];
            }
            if (FeatureMap.ContainsKey(f7))
            {
                if (!featureNums.ContainsKey(FeatureMap[f7]))
                    featureNums.Add(FeatureMap[f7], 1);
                else featureNums[FeatureMap[f7]] = ++featureNums[FeatureMap[f7]];
            }

            List<int> features = featureNums.Keys.ToList();
            features.Sort();
            string line = string.Empty;
            foreach (var feature in features)
            {
                line = string.Concat(line, feature, ":", featureNums[feature], " ");
            }
            line = line.Trim();
            #endregion

            foreach (var item in senseIds)
            {


                string SetALine = string.Concat(ClassMap[item], " ", line);
                Console.WriteLine(SetALine);
                WriteToFile(SetALine, fileName);
            }

        }

        //method for writing to file.
        public static void WriteToFile(String input, String fileName)
        {
            string path = @"G:\GitHub\SVMFeatureExtractor\SVMFeatureExtractor\" + fileName;
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
            //CreateFeatureAndClassMap();
            //CreateTrainFile();

            //GenerateKeyXml();

            //CreateTestFile();

            generateOptionsForSVM("SVMCommands_Arm.txt", "Arm");
            generateOptionsForSVM("SVMCommands_Diff.txt", "Difficulty");
            generateOptionsForSVM("SVMCommands_Interest.txt", "Interest");
            Console.WriteLine("Done !!!");
            Console.ReadKey();
        }


        static List<String> GetSenseIdFromKeyFile(string id)
        {
            List<String> senseids = new List<string>();
            XElement root = XElement.Load(@"G:\GitHub\SVMFeatureExtractor\SVMFeatureExtractor\KeyXml.xml");
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
            string path = @"G:\GitHub\SVMFeatureExtractor\SVMFeatureExtractor\SVMTest_Interest.txt";
            // if exists file then delete 
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            XElement po = XElement.Load(@"G:\GitHub\SVMFeatureExtractor\SVMFeatureExtractor\Xtract.test_Interest.xml");
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
                        ExtractFeaturesTypeSetA(str, senseids, "SVMTest_Interest.txt");
                    }
                }
            }
            Console.ReadKey();


        }

        static void GenerateKeyXml()
        {
            string path = @"G:\GitHub\SVMFeatureExtractor\SVMFeatureExtractor\KeyXml.xml";
            // This text is added only once to the file. 
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("<home>");
            }
            String[] lines = File.ReadAllLines(@"G:\GitHub\SVMFeatureExtractor\SVMFeatureExtractor\EnglishLS.test.key");
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
            string path = @"G:\GitHub\SVMFeatureExtractor\SVMFeatureExtractor\SVMTrain_Interest.txt";
            // This text is added only once to the file. 
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            XElement po = XElement.Load(@"G:\GitHub\SVMFeatureExtractor\SVMFeatureExtractor\Xtract.train_Interest.xml");
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
                        ExtractFeaturesTypeSetA(str, senseIds, "SVMTrain_Interest.txt");

                    }
                }
            }
            Console.ReadKey();
        }


        static void CreateFeatureAndClassMap()
        {
            string line;
            int featureCnt = 1;
            int classCnt = 1;
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(@"G:\GitHub\SVMFeatureExtractor\SVMFeatureExtractor\mytrain.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(' ');

                if (!ClassMap.ContainsKey(words[words.Length - 1]))
                    ClassMap.Add(words[words.Length - 1], classCnt++);

                words[words.Length - 1] = "&&&&";
                foreach (var word in words)
                {
                    if (!FeatureMap.ContainsKey(word) && !string.Equals(word, "&&&&"))
                    {
                        FeatureMap.Add(word, featureCnt++);
                    }
                }

            }

            file.Close();
        }

        static void generateOptionsForSVM(String currWord, string word)
        {
            string path = @"G:\GitHub\SVMFeatureExtractor\SVMFeatureExtractor\" + currWord;
            // This text is added only once to the file. 
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            double[] c = { 0.1, 0.2, 0.3, 0.5 };
            int[] o = { 0, 1 };
            int[] w = { 0, 1, 2, 3, 4, 9 };
            double[] e = { 0.1, 0.2, 0.4, 0.5 };
            int[] k = { 50, 100, 150, 200, 250 };
            int[] f = { 5, 7, 9 };
            int[] n = { 2, 3, 4 };
            int[] b = { 50, 60, 80, 90, 100 };

            List<string> lines = new List<string>();
            // bool kFlag =true, fFlag=true, bFlag=true;

            StringBuilder line = new StringBuilder();
            //command line before options - header

            foreach (var C in c)
            {
                foreach (var O in o)
                {
                    foreach (var W in w)
                    {
                        foreach (var E in e)
                        {
                            foreach (var K in k)
                            {
                                foreach (var F in f)
                                {
                                    foreach (var N in n)
                                    {
                                        foreach (var B in b)
                                        {

                                            StringBuilder sb = new StringBuilder();
                                            sb.Append("svm_multiclass_learn ")
                                                            .Append(" -c " + C)
                                                            .Append(" -w " + W)
                                                            .Append(" -e " + E)
                                                            .Append(" -o " + O);
                                            if (W == 0 || W == 1)
                                                sb.Append(" -k " + K);
                                            if (W == 4)
                                                sb.Append(" -f " + F);
                                            if (W == 4)
                                                sb.Append(" -b " + B);

                                            sb.Append(" -n " + N)
                                            .Append(" SVM_Train_" + word + ".txt model_" + word);
                                            lines.Add(sb.ToString());

                                            if (lines.Count == 20)
                                            {
                                                Write_All_Lines(lines, currWord);
                                                lines = new List<string>();
                                            }

                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            Write_All_Lines(lines, currWord);

        }

        private static void Write_All_Lines(List<string> lines, string filename)
        {
            string path = @"G:\GitHub\SVMFeatureExtractor\SVMFeatureExtractor\" + filename;
            // This text is added only once to the file. 
            if (!File.Exists(path))
            {
                File.WriteAllLines(path, lines);
            }
            else
                File.AppendAllLines(path, lines);
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSDForSenseEval
{
    public class FileData
    {
        public static string Train_File = @"";
        public static string Test_File = @"";
        public static string Key_File = @"";
        public static string Train_Output = @"";
        public static string Test_Output = @"";

        public static void WriteToFile(String input, String fileName)
        {
            string path = fileName;
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
        }

    }
}

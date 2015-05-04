using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSDForSenseEval
{
    class Program
    {
        static void Main(string[] args)
        {
            GetDefaultSenseIdForEachWord();
        }

        public static void GenerateDefaultXML() {
            XMLParser xmlp = new XMLParser();
            // xmlp.GenerateKeyXml();

            //xmlp.CreateTrainFile();

            xmlp.CreateTestFile();

            Console.WriteLine("Ineration finished !!");
        }

        public static void GetDefaultSenseIdForEachWord()
        {
            DefaultSenses ds = new DefaultSenses();
            ds.GetDefaultSensesOfWord();
        }
    }
}

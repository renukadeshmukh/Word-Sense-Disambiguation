using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod()
        {
            String context = "\nAs a result ,  they were transferred to different prisons .  Orlando Azcu and two others were taken to Kilo 7 Maximum Security Prison in Camaguey where ,  on 17 January ,  they went on hunger - strike .  They were said to have been forcibly dressed in the prison uniform and held for at least 17 days with their arms&& chained to the cell bars to prevent them from removing the uniform . Orlando Azcu was also said to have been beaten by prison guards on at least three occasions .  In early February he was transferred to Pinar del Ro Provincial Prison after agreeing to give up his protest and end his hunger - strike .  \n";
            ExtractFeaturesSetA(context);
        }

        public static void ExtractFeaturesSetA(String context)
        {
            // Here we call Regex.Match.
            Match match = Regex.Match(context, @"(head)([A-Za-z]+)(/head)",
                RegexOptions.IgnoreCase);

            // Here we check the Match instance.
            if (match.Success)
            {
                // Finally, we get the Group value and display it.
                string key = match.Groups[1].Value;
                //Console.WriteLine(key);
                Assert.IsNotNull(key);
            }
            else
            {
                Assert.Fail();
            }
        }

    }
}

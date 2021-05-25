using System;

namespace TandemSoapTester
{
    internal class Program
    {
        public static void Main (string[] args)
        {
            Console.Write ("Username:");
            var username = Console.ReadLine ();
            
            Console.Write ("Password:");
            var password = Console.ReadLine ();

            Test ("envelope-template-1.xml", username, password);
            Test ("envelope-template-2.xml", username, password);
        }

        static void Test (string path, string username, string password)
        {
            var soapTester = new TandemSoapTester ();
            var response = soapTester.GetEnrollmentCampaignList (path, username, password);
            
            Console.WriteLine ("Response:");
            Console.WriteLine (response);
            Console.WriteLine ();
        }
    }
}

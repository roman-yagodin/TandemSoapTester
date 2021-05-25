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
            
            var soapTester = new TandemSoapTester ();
            var response = soapTester.GetEnrollmentCampaignList ("envelope-template-1.xml", username, password);
            
            Console.WriteLine ("Response:");
            Console.WriteLine (response);
            Console.WriteLine ();
        }
    }
}

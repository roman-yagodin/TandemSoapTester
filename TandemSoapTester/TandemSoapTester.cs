using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;

namespace TandemSoapTester
{
    public class TandemSoapTester
    {
        public string GetEnrollmentCampaignList (string path, string username, string password)
        {
            var envelope = PrepareEnvelope (path, username, password);
            
            Console.WriteLine ("Request:");
            Console.WriteLine (envelope);
            Console.WriteLine ();
            
            var soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml (envelope);
            
            var webRequest = CreateWebRequest ("http://tandem-test.volgau.com/services/EnrCampaignInfoService",
                "getEnrollmentCampaignList");
 
            using (var stream = webRequest.GetRequestStream()) {
                soapEnvelopeXml.Save (stream);
            }

            try {
                using (var response = webRequest.GetResponse ()) {
                    using (var rd = new StreamReader (response.GetResponseStream ())) {
                        return rd.ReadToEnd ();
                    }
                }
            }
            catch (WebException ex) {
                using (var rd = new StreamReader (ex.Response.GetResponseStream ())) {
                    return rd.ReadToEnd ();
                }
            }

            return string.Empty;
        }

        HttpWebRequest CreateWebRequest (string url, string action)
        {
            var webRequest = (HttpWebRequest) WebRequest.Create (url);
            webRequest.Headers.Add ("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            return webRequest;
        }

        public string PrepareEnvelope (string path, string username, string password)
        {
            var envelope = File.ReadAllText (path);

            envelope = envelope.Replace ("{wsu:Created}", DateTime.Now.ToUniversalTime ().ToString ("O"));
            envelope = envelope.Replace ("{wsu:Expires}", DateTime.Now.AddMinutes (5).ToUniversalTime ().ToString ("O"));

            envelope = envelope.Replace ("{wsse:Username}", username);
            envelope = envelope.Replace ("{wsse:Password}", password);
            
            return envelope;
        }

    }
}
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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

            var soapEnvelopeXml = new XmlDocument ();
            soapEnvelopeXml.LoadXml (envelope);

            var webRequest = CreateWebRequest ("http://tandem-test.volgau.com/services/EnrCampaignInfoService",
                "getEnrollmentCampaignList");

            using (var stream = webRequest.GetRequestStream ()) {
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

            var created = DateTime.Now.ToUniversalTime ();
            var createdStr = created.ToString ("O");

            var expires = created.AddMinutes (5);
            var expiresStr = expires.ToString ("O");

            var nonce = string.Empty;
            if (envelope.Contains ("{wsse:Nonce}")) {
                var nonceBytes = Encoding.UTF8.GetBytes (Guid.NewGuid ().ToString ());
                Array.Resize (ref nonceBytes, 16);
                nonce = Convert.ToBase64String (nonceBytes);
            }

            envelope = envelope.Replace ("{wsu:Created}", createdStr);
            envelope = envelope.Replace ("{wsu:Expires}", expiresStr);
            envelope = envelope.Replace ("{wsse:Username}", username);
            envelope = envelope.Replace ("{wsse:Password}", password);
            envelope = envelope.Replace ("{wsse:Nonce}", nonce);

            return envelope;
        }
    }
}
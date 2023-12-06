using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml.Schema;
using static System.Collections.Specialized.BitVector32;
using System.Text.Json;

namespace SoapTester
{
    public class TestCall
    {
        public static void Call()
        {
            try
            {
                string soapEndpoint = "http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso";
                string soapAction = "http://www.oorsprong.org/websamples.countryinfo";

                // Construct the SOAP request
                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                        <soapenv:Envelope xmlns:soapenv=""http://www.w3.org/2003/05/soap-envelope"">
                                          <soapenv:Body>
                                            <ListOfCurrenciesByName xmlns=""http://www.oorsprong.org/websamples.countryinfo"">
                                            </ListOfCurrenciesByName>
                                          </soapenv:Body>
                                        </soapenv:Envelope>";

                // Create the HTTP client
                using (HttpClient client = new HttpClient())
                {
                    // Set the SOAPAction header
                    client.DefaultRequestHeaders.Add("SOAPAction", soapAction);

                    // Create the HTTP request message with StringContent
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, soapEndpoint);
                    request.Content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

                    // Send the request and get the response
                    HttpResponseMessage response = client.SendAsync(request).Result;

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and process the SOAP response
                        string soapResponse = response.Content.ReadAsStringAsync().Result;

                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(soapResponse);
                        Read(xml);
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }

        public static void Read(XmlDocument xmlDoc)
        {

            // Root elementni olish
            XmlElement root = xmlDoc.DocumentElement;
            Console.WriteLine("Root tag: " + root.Name);

            // Elementlar orqali bo'lib borish
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node is XmlElement)
                {
                    XmlElement childElement = (XmlElement)node;
                    Console.WriteLine("Tag: " + childElement.Name + " | Inner Text: " + childElement.InnerText);
                }
            }

            // Xususiy elementlarni izlash
            XmlNodeList specificElements = xmlDoc.GetElementsByTagName("specific_element");
            foreach (XmlNode node in specificElements)
            {
                Console.WriteLine("Specific Element found: " + node.Name + " | Inner Text: " + node.InnerText);
            }
            Console.WriteLine("---------------------------------------------------------------------");
            GetSchema(xmlDoc);
        }

        public static void GetSchema(XmlDocument xmlDoc)
        {
            try
            {
                // Access the root element
                XmlElement root = xmlDoc.DocumentElement;
                Console.WriteLine("Root tag: " + root.Name);

                // Access child elements
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node is XmlElement)
                    {
                        XmlElement childElement = (XmlElement)node;
                        Console.WriteLine("Tag: " + childElement.Name + " | Inner Text: " + childElement.InnerText);
                    }
                }

                // Access specific elements by tag name
                XmlNodeList specificElements = xmlDoc.GetElementsByTagName("specific_element");
                foreach (XmlNode node in specificElements)
                {
                    Console.WriteLine("Specific Element found: " + node.Name + " | Inner Text: " + node.InnerText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        

    }
}

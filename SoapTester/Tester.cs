using CoAP.Server;
using CoAP;
using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;


namespace SoapTester 
{
    public class WsdlParser
    {
        public static void Run2()
        {
           
        }
        public static void Run()
        {

            string wsdlFilePath = "https://www.w3schools.com/xml/tempconvert.asmx?wsdl"; // Replace with your WSDL file path

            try
            {
                XDocument wsdlDocument = XDocument.Load(wsdlFilePath);

                // Define the namespaces used in the WSDL document
                XNamespace wsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";
                XNamespace soapNamespace = "http://schemas.xmlsoap.org/wsdl/soap/";

                // Extract operations (methods) from the WSDL
                var operations = wsdlDocument.Descendants(wsdlNamespace + "operation");

                foreach (var operation in operations)
                {
                    string methodName = operation.Attribute("name")?.Value;
                    
                    

                    Console.WriteLine($"Method Name: {methodName}");

                    // Extract input parameters
                    var inputMessage = operation.Descendants(wsdlNamespace + "input").FirstOrDefault();
                    if (inputMessage != null)
                    {
                        string inputMessageName = inputMessage.Attribute("message")?.Value;
                        Console.WriteLine($"Input: {inputMessageName}");
                        ExtractParameters(wsdlDocument, inputMessageName);
                    }

                    // Extract output parameters (if needed)
                    var outputMessage = operation.Descendants(wsdlNamespace + "output").FirstOrDefault();
                    if (outputMessage != null)
                    {
                        string outputMessageName = outputMessage.Attribute("message")?.Value;
                        Console.WriteLine($"Output: {outputMessageName}");
                        ExtractParameters(wsdlDocument, outputMessageName);
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ExtractParameters(XDocument wsdlDocument, string messageName)
        {
            XNamespace wsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";
            XNamespace soapNamespace = "http://schemas.xmlsoap.org/wsdl/soap/";

            var message = wsdlDocument.Descendants(wsdlNamespace + "message")
                                      .FirstOrDefault(m => m.Attribute("name")?.Value == "FahrenheitToCelsiusSoapIn");

            var sss = wsdlDocument.Elements();



            if (message != null)
            {
                var parts = message.Descendants(wsdlNamespace + "part");
                foreach (var part in parts)
                {
                    string parameterName = part.Attribute("name")?.Value;
                    string elementName = part.Attribute("element")?.Value;

                    var elements = wsdlDocument.Descendants(wsdlNamespace + "types");
                    var element = elements.Descendants("s:schema");
                    var element2 = element.Descendants("s:element");
                    var element3 = element2.Descendants("s:complexType");
                    var element4 = element3.Descendants("s:sequence");
                    var element5 = element4.Descendants("s:element");
              

                    Console.WriteLine($"Parameter Name: {parameterName}, Type: {elementName}");

                    foreach (var element6 in element5)
                    {
                        Console.WriteLine($"Elements Name: {element6.Attribute("name")}, Type: {element6.Attribute("type")}");
                    }

                }
            }
        }
    }
}

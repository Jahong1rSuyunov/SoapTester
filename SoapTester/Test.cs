using CoAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SoapTester
{
    public class Test
    {
        public static void Run()
        {
            // WSDL manzili
            string wsdlUrl = "http://www.dneonline.com/calculator.asmx?wsdl";

            XDocument xmlDoc = XDocument.Load(wsdlUrl);

            // Namespace larni e'lon qilish
            XNamespace wsdl = "http://schemas.xmlsoap.org/wsdl/";
            XNamespace xs = "http://www.w3.org/2001/XMLSchema";

            var operations = xmlDoc.Descendants(wsdl + "portType")
                .Elements(wsdl + "operation");
            var methodList = new List<query>();    

            foreach (var operation in operations)
            {
                var method = new query();
                method.MethodName = operation.Attribute("name")?.Value;
                method.OutputName = GetOutput(xmlDoc, operation);
                method.InputName = GetInput(xmlDoc, operation);
                
                methodList.Add(method);

            }

            foreach (var method in methodList)
            {
                Console.WriteLine($"    Method Name: {method.MethodName}");
                Console.WriteLine($"    Input: {method.InputName}");
                ConsoleWriteLine(xmlDoc, method.InputName);
                Console.WriteLine($"    Output: {method.OutputName}");
                ConsoleWriteLine(xmlDoc, method.OutputName);
                Console.WriteLine("-----------------------------");
                Console.WriteLine();
            }

        }
        public static void ConsoleWriteLine(XDocument xmlDoc, string messageName)
        {
            XNamespace wsdl = "http://schemas.xmlsoap.org/wsdl/";
            XNamespace xs = "http://www.w3.org/2001/XMLSchema";

            var elements = xmlDoc.Descendants(wsdl + "types")
                           .Elements(xs + "schema")
                           .Elements(xs + "element").FirstOrDefault(e => e.Attribute("name")?.Value == messageName)
                           .Elements(xs + "complexType")
                           .Elements(xs + "sequence")
                           .Elements(xs + "element");

            foreach (var element in elements)
            {
                string name = element.Attribute("name")?.Value;
                string type = element.Attribute("type")?.Value;

                Console.WriteLine($"    Name: {name}, Type: {type}");
            }

        }

        public static string GetInput(XDocument xmlDoc, XElement element)
        {
            var result = string.Empty;
            XNamespace wsdl = "http://schemas.xmlsoap.org/wsdl/";
            XNamespace xs = "http://www.w3.org/2001/XMLSchema";
            var inputMessage = element.Descendants(wsdl + "input").FirstOrDefault();
            if (inputMessage != null)
            {
                var messageName = inputMessage.Attribute("message")?.Value;
                if (messageName.StartsWith("tns:"))
                    messageName = messageName.Substring("tns:".Length);
                var message = xmlDoc.Descendants(wsdl + "message")
                                      .FirstOrDefault(m => m.Attribute("name")?.Value == messageName);
                var part = message.Descendants(wsdl + "part").FirstOrDefault();
                var elementName = part.Attribute("element")?.Value;
                if (elementName.StartsWith("tns:"))
                    result = elementName.Substring("tns:".Length);
                else
                    result = elementName;

            }
                
            return result;

        }
        public static string GetOutput(XDocument xmlDoc, XElement element)
        {
            var result = string.Empty;
            XNamespace wsdl = "http://schemas.xmlsoap.org/wsdl/";
            XNamespace xs = "http://www.w3.org/2001/XMLSchema";
            var inputMessage = element.Descendants(wsdl + "output").FirstOrDefault();
            if (inputMessage != null)
            {
                var messageName = inputMessage.Attribute("message")?.Value;
                if (messageName.StartsWith("tns:"))
                    messageName = messageName.Substring("tns:".Length);
                var message = xmlDoc.Descendants(wsdl + "message")
                                      .FirstOrDefault(m => m.Attribute("name")?.Value == messageName);
                var part = message.Descendants(wsdl + "part").FirstOrDefault();
                var elementName = part.Attribute("element")?.Value;
                if (elementName.StartsWith("tns:"))
                    result = elementName.Substring("tns:".Length);
                else
                    result = elementName;

            }


            return result;

        }

    }
}

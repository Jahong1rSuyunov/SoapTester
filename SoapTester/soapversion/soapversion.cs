using System.Web.Services.Description;
using System.Web.Services;
using System.Web.Services.Configuration;
using System.ServiceModel;
using System.Xml.Linq;
using System;
using System.Net;
using Castle.Components.DictionaryAdapter.Xml;
using System.Xml;
using System.Xml.Schema;
using System.Security.AccessControl;

namespace SoapTester.soapversion
{
    public class Soapversion
    {
        public static void GetSoapVersion(string url)
        {
            XDocument xmlDoc = XDocument.Load(url);

            ServiceDescription serviceDescription = ServiceDescription.Read(xmlDoc.CreateReader());

            // Choose the operation by name
            string operationName = "MyOperation";

            // Find the operation and its input message
            Operation operation = serviceDescription.PortTypes[0].Operations[0];
            OperationInput inputMessage = operation.Messages[0] as OperationInput;

            // Get the input message schema
            Message message = serviceDescription.Messages[inputMessage.Message.Name];
            
            string partName = message.Parts[0].Element.Name;

            // Print the input schema details
            Console.WriteLine("Input Schema:");
            foreach (MessagePart part in message.Parts)
            {
                var schemas = serviceDescription.Types.Schemas[0].Items;

                foreach (var schema in schemas)
                {
                    if (schema is XmlSchemaElement)
                    {
                        XmlSchemaElement schemaElement = schema as XmlSchemaElement;

                        if(partName != schemaElement.Name) continue;

                        Console.Out.WriteLine("Schema Element: {0}", schemaElement.Name);

                        XmlSchemaType schemaType = schemaElement.SchemaType;
                        XmlSchemaComplexType schemaComplexType = schemaType as XmlSchemaComplexType;

                        if (schemaComplexType != null)
                        {
                            XmlSchemaSequence schemaSequence = schemaComplexType.Particle as XmlSchemaSequence;

                            if (schemaSequence != null)
                            {
                                foreach (XmlSchemaElement childElement in schemaSequence.Items)
                                {
                                    Console.Out.WriteLine("    Element/Type: {0}:{1}", childElement.Name,
                                                      childElement.SchemaTypeName.Name);
                                }
                            }
                        }
                    }
                    else if (schema is XmlSchemaComplexType)
                    {
                        XmlSchemaComplexType schemaComplexType = schema as XmlSchemaComplexType;

                    }                    
                    else if (schema is XmlSchemaSimpleType)
                    {
                        XmlSchemaSimpleType schemaSimpleType = schema as XmlSchemaSimpleType;

                    }
                }

            }


        }
        public static string GetWsdlNamespace(ServiceDescription serviceDescription)
        {
            var result = string.Empty;
            if (serviceDescription == null)
            {
                throw new ArgumentNullException(nameof(serviceDescription));
            }

            const string soap11Binding = "http://schemas.xmlsoap.org/soap/http";
            const string soap12Binding = "http://www.w3.org/2003/05/soap/bindings/HTTP/";

            foreach (Binding binding in serviceDescription.Bindings)
            {
                foreach (object extensibilityElement in binding.Extensions)
                {
                    if (extensibilityElement is Soap12Binding)
                    {
                        if (((SoapBinding)extensibilityElement).Transport == soap11Binding)
                        {
                            result = "SOAP 1.1";
                        }
                        else if (((SoapBinding)extensibilityElement).Transport == soap12Binding)
                        {
                            result = "SOAP 1.2";
                        }
                        Console.WriteLine(result);
                    }
                }
            }

            return result;

        }
    }
}

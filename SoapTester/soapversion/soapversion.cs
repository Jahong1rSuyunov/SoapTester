using System.Web.Services.Description;
using System.Web.Services;
using System.Web.Services.Configuration;
using System.ServiceModel;
using System.Xml.Linq;
using System;
using System.Net;
using Castle.Components.DictionaryAdapter.Xml;
using System.Xml;

namespace SoapTester.soapversion
{
    public class Soapversion
    {
        public static void GetSoapVersion(string url)
        {
            XDocument xmlDoc = XDocument.Load(url);

            ServiceDescription serviceDescription = ServiceDescription.Read(xmlDoc.CreateReader());






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

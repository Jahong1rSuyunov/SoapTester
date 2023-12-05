using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml.Linq;

namespace SoapTester.soapversion
{
    public class Soapversion
    {
        public static void GetSoapVersion(string url)
        {
            XDocument xmlDoc = XDocument.Load(url);

            ServiceDescription serviceDescription = ServiceDescription.Read(xmlDoc.CreateReader());

            foreach (Binding binding in serviceDescription.Bindings)
            {
                Type bindingType = binding.GetType();


                if (bindingType == typeof(SoapBinding))
                {
                    // SOAP 1.1
                    Console.WriteLine("SOAP 1.1 is used.");
                }
                else if (bindingType == typeof(Soap12Binding))
                {
                    // SOAP 1.2
                    Console.WriteLine("SOAP 1.2 is used.");
                }
            }
        }
    }
}

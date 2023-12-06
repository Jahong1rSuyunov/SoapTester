﻿using SoapTester.Parser;
using System.Web.Services.Description;
using System.Xml.Linq;
using SoapTester.soapversion;
using System.Text.Json;
using System.Xml.Schema;

namespace SoapTester
{
    internal class Program
    {
        static void Main(string[] args)
        {


            string wsdlUrl = "http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso?wsdl";

            XDocument xmlDoc = XDocument.Load(wsdlUrl);

            var desc = ServiceDescription.Read(xmlDoc.CreateReader());
            var wsdl = new WSDL();
            //var result = wsdl.GetWSDLService(desc);
            wsdl.GetSchema(desc);


            //Console.WriteLine(JsonSerializer.Serialize(result));

            //var a = WSDLParser.Parse(desc, false);

            //foreach (var item in a.First().port.First().operations)
            //{
            //    Console.WriteLine($"    name: {item.name}");
            //    Console.WriteLine($"    action: {item.soapaction}");
            //    Console.WriteLine($"    input: {item.input}");
            //    Console.WriteLine($"    output: {item.output}");
            //    Console.WriteLine();

            //}
            //TestCall.Call();
            //Test.Run();




            var url = "http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso?wsdl";
            Soapversion.GetSoapVersion(url);



        }
    }
}

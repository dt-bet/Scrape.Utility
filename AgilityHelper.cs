using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Scrape.Utility
{
    public class AgilityHelper
    {
        public static HtmlDocument GetDocumentElement(HtmlWeb web, params string[] url)
        {

            ServicePointManager.SecurityProtocol =
      SecurityProtocolType.Tls |
 SecurityProtocolType.Tls11 |
 SecurityProtocolType.Tls12;
            // SecurityProtocolType.Ssl3;


            var doc = web.Load(String.Join("", url));

            return doc;

        }
    }

}

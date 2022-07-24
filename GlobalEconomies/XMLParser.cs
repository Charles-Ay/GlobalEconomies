using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GlobalEconomies
{
    public class XMLParser
    {
        private static XmlDocument? document;

        public XMLParser(string fileName)
        {
            document = new XmlDocument();
            document.Load(fileName);
        }

        public string GetAllRegionNames()
        {
            string results = "";
            XmlNodeList nodes = document?.SelectNodes("/global_economies/region/@rname");
            for (int i = 0; i < nodes.Count; i++)
            {
                results += nodes[i].Value + "\n";
            }
            return results;
        }
        public string GetAllRegionIDs()
        {
            string results = "";
            XmlNodeList nodes = document?.SelectNodes("/global_economies/region/@rid");
            for (int i = 0; i < nodes.Count; i++)
            {
                results += nodes[i].Value + "\n";
            }
            return results;
        }
        
        public XmlNode GetRegionByName(string name)
        {
            XmlNodeList nodes = document?.SelectNodes("/global_economies/region[@rname='" + name + "']");
            return nodes[0];
        }

        
        private List<XmlNode> GetRelevantLabelsAndAttributes()
        {
            List<XmlNode> returns = new();
            XmlNodeList labelNodes = document.SelectNodes(".//labels");
            var t = labelNodes[0].ChildNodes;
            for (int i = 3; i < t.Count; i++)
            {
                returns.Add(t[i]);
            }
            return returns;
        }

        //R option
        public void QueryXMLForRegionalSummary(int minYear, int maxYear, string region)
        {
            string result = "";
            var lbls = GetRelevantLabelsAndAttributes();
            var t = document.SelectNodes($"/global_economies/region[@rid = \"{GetRegionByName(region).Attributes["rid"].Value}\"]");
            XmlNodeList nodes = document.SelectNodes($"/global_economies/region[@rid = \"{GetRegionByName(region).Attributes["rid"].Value}\"]/year[@yid >= \"{minYear}\" and @yid <= \"{maxYear}\"]");

            Console.WriteLine($"Economic Information for {region}" +
                              $"\n----------------------------------\n");
            Console.WriteLine("Ecnomic Metric\t");
        }
    }
}

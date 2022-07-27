using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GlobalEconomies
{
    public class XMLParser
    {
        public struct RegionYearValues{
            public string Region;
            public int Year;
            public List<Label> Values;
        }
        
        private static XmlDocument? document;

        public XMLParser(string fileName)
        {
            document = new XmlDocument();
            document.Load(fileName);
        }

        public List<string> GetAllRegionNamesList()
        {
            List<string> returns = new();
            XmlNodeList nodes = document?.SelectNodes("/global_economies/region/@rname");
            for (int i = 0; i < nodes.Count; i++)
            {
                returns.Add(nodes[i].Value);
            }
            return returns;
        }     
        
        public XmlNode GetRegionByName(string name)
        {
            XmlNodeList nodes = document?.SelectNodes("/global_economies/region[@rname='" + name + "']");
            return nodes[0];
        }

        
        private List<Label> GetRelevantLabelsAndAttributes()
        {
            List<Label> returns = new();
            XmlNodeList labelNodes = document.SelectNodes(".//labels");
            XmlNodeList t = labelNodes[0].ChildNodes;
            for (int i = 3; i < t.Count; i++)
            {
                Label lbl = new Label(t[i].Name, t[i].Value);
                foreach(XmlNode child in t[i].Attributes)
                {
                    lbl.AddChild(child.Name, child.Value);
                }
                returns.Add(lbl);
            }
            
            return returns;
        }

        public List<string> GetRelevantLabels()
        {
            List<string> returns = new();
            List<Label> nodes = GetRelevantLabelsAndAttributes();
            for (int i = 0; i < nodes.Count; i++)
            {
                foreach (Label attr in nodes[i].children)
                {
                    returns.Add(attr.Value);
                }
            }
            return returns;
        }

        public List<RegionYearValues> GetLabelsWithRegionYearValues(string metric)
        {
            var templateLabels = GetRelevantLabelsAndAttributes();
            List<RegionYearValues> returns = new();
            XmlNodeList nodes = document?.SelectNodes("/global_economies/region");
            for (int i = 0; i < nodes.Count; i++)
            {
                foreach (XmlNode node in nodes[i].ChildNodes)
                {
                    RegionYearValues itemstruct = new();
                    itemstruct.Region = nodes[i].Attributes["rname"].Value;
                    itemstruct.Year = int.Parse(node.Attributes["yid"].Value);
                    itemstruct.Values = new();
                    if(node.HasChildNodes)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            foreach(XmlAttribute attr in child.Attributes)
                            {
                                foreach (Label lbl in templateLabels)
                                {
                                    var tmp = lbl.FindLabelByValue(metric).Name;
                                    if (!string.IsNullOrEmpty(tmp))
                                    {
                                        if (lbl.children.Any(x => x.Name == attr.Name && attr.Name == tmp))
                                        {
                                            Label lbl2 = new Label(lbl.Name);
                                            lbl2.AddChild(attr.Name, attr.Value);
                                            itemstruct.Values.Add(lbl2);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    returns.Add(itemstruct);
                }
            }
            return returns;
        }

        //R option
        public void QueryXMLForRegionalSummary(int minYear, int maxYear, string region)
        {
            string result = "";
            var lbls = GetRelevantLabelsAndAttributes();
            XmlNodeList nodes = document.SelectNodes($"/global_economies/region[@rid = \"{GetRegionByName(region).Attributes["rid"].Value}\"]/year[@yid >= \"{minYear}\" and @yid <= \"{maxYear}\"]");

            Console.WriteLine($"Economic Information for {region}" +
                              $"\n----------------------------------\n");
            Console.Write("\t\tEcnomic Metric");
            for (int i = minYear; i <= maxYear; i++)
            {
                Console.Write($"{i, 10}");
            }
            
            foreach (var lbl in lbls)
            {
                for (int j = 0; j < lbl.children.Count; ++j)
                {
                    Console.Write("\n{0, 30}", lbl.children[j].Value);
                    bool found = false;
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        foreach (XmlNode node in nodes[i].ChildNodes)
                        {
                            string s = lbl.children[j].Name;
                            if (node.Attributes[s] != null && !string.IsNullOrEmpty(node.Attributes[s].Name))
                            {
                                if (node.Attributes[s].Name == lbl.children[j].Name)
                                {
                                    if (node.Attributes[s].Value == "")
                                    {
                                        Console.Write("{0,10}", "-");
                                    }
                                    else
                                    {
                                        Console.Write($"{node.Attributes[s].Value,10}");
                                    }
                                }
                                found = true;
                                break;
                            }

                        }
                        if (!found)
                        {
                            Console.Write("{0,10}", "-");
                        }
                        else found = false;
                    }

                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        //M option
        public void QueryXMLForRegionalMetric(int minYear, int maxYear, string metric)
        {
            string result = "";
            var lbls = GetLabelsWithRegionYearValues(metric);

            Console.Write("{0,53}", "Region");
            for (int i = minYear; i <= maxYear; i++)
            {
                Console.Write($"{i,10}");
            }

            Console.WriteLine();
            Console.WriteLine();
            bool found = false;
            XmlNodeList nodes = document.SelectNodes($"/global_economies/region");
            #region remove this
            /*
            for (int i = 0; i < nodes.Count; ++i)
            {
                Console.Write($"{nodes[i].Attributes["rname"].Value,10}");
                foreach (XmlNode node in nodes[i].ChildNodes)
                {
                    if (int.Parse(node.Attributes["yid"].Value) >= minYear && int.Parse(node.Attributes["yid"].Value) <= maxYear)
                    {
                        foreach (XmlNode data in node)
                        {

                            if (data.Attributes.Count != 0)
                            {
                                for (int j = 0; j < lbls.Count; ++j)
                                {
                                    if (data.Attributes[lbls[j].Attributes[metric].Value] != null)
                                    {
                                        if (data.Attributes[lbls[j].Attributes[metric].Name].Name == metric)
                                        {
                                            Console.Write($"{data.Attributes[lbls[j].Attributes[metric].Name].Value,10}");
                                            found = true;
                                        }
                                    }
                                }
                            }
                            else Console.Write("{0,10}", "-");
                        }
                    }
                }
            }
            */
            #endregion
            var valuesInRange = new List<RegionYearValues>(lbls.Where(x => x.Year >= minYear && x.Year <= maxYear));

            string currentRegion = valuesInRange[0].Region;
            bool writen = false;
            foreach (RegionYearValues value in valuesInRange)
            {
                if (value.Region != currentRegion)
                {
                    Console.WriteLine();
                    currentRegion = value.Region;
                    writen = false;
                }
                if (!writen)
                {
                    Console.Write("{0,53}", value.Region);
                    writen = true;
                }
                if (value.Values.Count == 0)
                {
                    Console.Write("{0,10}", "-");
                }
                else
                {
                    Console.Write($"{value.Values[0].children[0].Value,10}");
                }

            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
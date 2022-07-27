namespace GlobalEconomies
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("World Economic Data");
            Console.WriteLine("===================");

            int yearlow = 1980;
            int yearhigh = 1983;
            XMLParser parser = new ("global_economies.xml");
            string numinp = "0";
            string inp = "";
            while (inp != "x")
            {
                menu(yearlow, yearhigh);
                Console.Write("You selected: ");
                inp = Console.ReadLine().ToLower();
                Console.WriteLine();
                if (inp == "y")
                {
                    bool valid = false;
                    do
                    {
                        try
                        {
                            Console.Write("Enter the low year: ");
                            numinp = Console.ReadLine();
                            yearlow = int.Parse(numinp);
                            Console.Write("Enter the high year: ");
                            numinp = Console.ReadLine();
                            yearhigh = int.Parse(numinp);
                            if ((yearhigh - yearlow) > 4)
                            {
                                yearlow = 1980;
                                yearhigh = 1984;
                                Console.WriteLine("The Range cannot be greater than 5 years(inclusive)");
                            }
                            else
                            {
                                Console.WriteLine("Range accepted, years updated");
                                valid = true;
                            }
                        }catch(FormatException)
                        {
                            Console.WriteLine("Invalid input try again");
                        }
                    } while (!valid);
                  
                }
                else if(inp == "r")
                {
                    var li = parser.GetAllRegionNamesList();
                    Console.WriteLine("Select a region by number as shown below...");
                    for (int i = 0; i < li.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {li[i]}");
                    }

                    string region = "";
                    
                    bool valid = false;
                    do
                    {
                        Console.Write("Enter a region #: ");
                        try
                        {
                            int val = int.Parse(Console.ReadLine());
                            if (val > li.Count || val < 1)
                            {
                                Console.WriteLine("Option not in correct range\n");
                            }
                            else
                            {
                                valid = true;
                                region = li[val - 1];
                            }
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid input try again");
                        }
                    } while (!valid);

                    Console.WriteLine();
                    parser.QueryXMLForRegionalSummary(yearlow, yearhigh, region);
                }
                else if(inp == "m")
                {
                    var lbls = parser.GetRelevantLabels();
                    Console.WriteLine("Select a metric by number as shown below...");
                    for (int i = 0; i < lbls.Count; i++)
                    {
                        Console.WriteLine($"  {i + 1}. {lbls[i]}");
                    }

                    string metric = "";

                    bool valid = false;
                    do
                    {
                        Console.Write("Enter a metric #: ");
                        try
                        {
                            int val = int.Parse(Console.ReadLine());
                            if (val > lbls.Count || val < 1)
                            {
                                Console.WriteLine("Option not in correct range\n");
                            }
                            else
                            {
                                valid = true;
                                metric = lbls[val - 1];
                            }
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid input try again");
                        }
                    } while (!valid);

                    Console.WriteLine();
                    Console.WriteLine($"{metric} By Region");
                    Console.WriteLine("----------------------------\n");
                    parser.QueryXMLForRegionalMetric(yearlow, yearhigh, metric);
                }
                else if(inp == "x")
                {
                    Console.WriteLine("Bye bye");
                    break; 
                }  
            }
            


        }

        public static void menu(int yearlow, int yearhigh)
        {
            Console.WriteLine("");
            Console.WriteLine("'Y' to change the year range for the data (currently " + yearlow + " to " + yearhigh + ")");
            Console.WriteLine("'R' to print all the metric data for a specific country");
            Console.WriteLine("'M' to print the specific metric for all regions");
            Console.WriteLine("'X' to exit the program");
        }


    }
}
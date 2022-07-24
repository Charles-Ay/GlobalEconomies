namespace GlobalEconomies
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("World Economic Data");
            Console.WriteLine("===================");

            int yearlow = 1990;
            int yearhigh = 1994;
            XMLParser parser = new ("global_economies.xml");
            string numinp = "0";
            string inp = "";
            while (inp != "x")
            {
                menu(yearlow, yearhigh);
                inp = Console.ReadLine().ToLower();
                if (inp == "y")
                {
                    bool valid = false;
                    do
                    {
                        try
                        {
                            Console.WriteLine("Enter the low year");
                            numinp = Console.ReadLine();
                            yearlow = int.Parse(numinp);
                            Console.WriteLine("Enter the high year");
                            numinp = Console.ReadLine();
                            yearhigh = int.Parse(numinp);
                            if ((yearhigh - yearlow) > 4)
                            {
                                yearlow = 1990;
                                yearhigh = 1994;
                                Console.WriteLine("The Range cannot be greater than 5 years");
                            }
                            else
                            {
                                Console.WriteLine("Range accepted, years updated");
                                valid = true;
                            }
                        }catch(Exception e)
                        {
                            Console.WriteLine("Invalid input try again");
                        }
                    } while (!valid);
                  
                }
                else if(inp == "r")
                {
                    parser.QueryXMLForRegionalSummary(yearlow, yearhigh, "Canada");
                }
                else if(inp == "m")
                {
                    Console.WriteLine("does nothing atm");
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ClientFolderFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("v1.20");
            bool valid = false;
            String filetype = "";
            String typepath = "";
            String userpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            while (!valid) {
                Console.Write("File Type: ");
                filetype = Console.ReadLine();
                if (filetype.ToLower().Equals("p"))
                {
                    valid = true;
                    Console.WriteLine("Purchase File Type Selected.");
                    typepath = userpath + "\\The Gradzki Law Firm LLC\\Real Estate - Documents\\1 PURCHASES";
                }
                else if (filetype.ToLower().Equals("r"))
                {
                    valid = true;
                    Console.WriteLine("Refinance File Type Selected.");
                    typepath = userpath + "\\The Gradzki Law Firm LLC\\Real Estate - Documents\\2 REFINANCE";
                }
                else if (filetype.ToLower().Equals("s"))
                {
                    valid = true;
                    Console.WriteLine("Sale File Type Selected.");
                    typepath = userpath + "\\The Gradzki Law Firm LLC\\Real Estate - Documents\\3 SALE";
                }
                else
                {
                    Console.WriteLine("Invalid. Try Again.");
                }
            }
            String filepath;
            String[] userinfo = { "", "", "" };

            Console.Write("Name: ");
            userinfo[0] = Console.ReadLine();

            Console.Write("Street (Include Street Type/Abbreviation): ");
            userinfo[1] = Console.ReadLine();
            string strtype = "";
            if(! userinfo[1].Equals(""))
            {
                strtype = userinfo[1].Substring(userinfo[1].LastIndexOf(" ") + 1, userinfo[1].Length - userinfo[1].LastIndexOf(" ") - 1);
                userinfo[1] = userinfo[1].Substring(0, userinfo[1].LastIndexOf(" "));
            }

            Console.Write("Unit #: ");
            string unitnumber = Console.ReadLine();
            if(!String.IsNullOrEmpty(unitnumber))
            {
                unitnumber = ", " + unitnumber;
            }

            Console.Write("Town: ");
            userinfo[2] = Console.ReadLine();
            
            

            String[] files = Directory.GetDirectories(typepath);

            String[] cutpath = new string[files.Length];

            for(int i = 0; i < files.Length; i++)
            {
                int x = 0;
                if (filetype.ToLower().Equals("p"))
                {
                    x = files[i].IndexOf("1 PURCHASES\\") + 12;
                }
                else if (filetype.ToLower().Equals("r"))
                {
                    x = files[i].IndexOf("2 REFINANCE\\") + 12;
                }
                else if (filetype.ToLower().Equals("s"))
                {
                    x = files[i].IndexOf("3 SALE\\") + 7;
                }
                
                cutpath[i] = files[i].Substring(x, files[i].Length - x);
            }
            Console.WriteLine("________________________________");
            List<int> found = new List<int>();
            for (int i = 0; i < cutpath.Length; i++) {
                //Console.WriteLine(str);
                int hits = 0;
                String str = cutpath[i].ToLower();
                String[] threedata = threewayseperation(userinfo[0]);
                if (userinfo[0].Length != 0)
                {
                    int index = 0;
                    if (str.IndexOf(threedata[0]) != -1)
                    {
                        hits++;
                        index = str.IndexOf(threedata[0]);
                    }
                    if (str.IndexOf(threedata[1]) != -1 && str.IndexOf(threedata[1]) <= (index + threedata[0].Length + 2)) hits++;
                    if (str.IndexOf(threedata[2]) != -1 && str.IndexOf(threedata[2]) <= (index + threedata[0].Length + threedata[1].Length + 3)) hits++;
                } if(userinfo[1].Length != 0)
                {
                    threedata = threewayseperation(userinfo[1]);
                    if (str.IndexOf(threedata[0]) != -1) hits++;
                    if (str.IndexOf(threedata[1]) != -1) hits++;
                    if (str.IndexOf(threedata[2]) != -1) hits++;
                }
                
                if(hits >= 4 || (hits >= 2 && (userinfo[1].Length == 0 || userinfo[0].Length == 0)))
                {
                    found.Add(i);
                }
            }
            if (found.Count > 1)
            {
                Console.WriteLine("Multiple Found.");
                for(int i = 0; i < found.Count; i++)
                {
                    Console.WriteLine(i+1 + ": " + cutpath[found[i]]);
                }
                while (true)
                {
                    Console.Write("Number To Open / 'N' for new folder: ");
                    string num = Console.ReadLine();
                    if(num.ToLower().Equals("n"))
                    {
                        String flname = userinfo[0] + " (" + userinfo[1] + " " + strtype + unitnumber + ", " + userinfo[2] + ")";
                        Console.WriteLine(flname);
                        Directory.CreateDirectory(typepath + "\\" + flname);
                        Process.Start(new ProcessStartInfo()
                        {
                            Arguments = "\"" + typepath + "\\" + flname + "\"",
                            FileName = "explorer.exe"
                        });
                        break;
                    }
                    try
                    {
                        int x = Int32.Parse(num);
                        if (x > found.Count)
                        {
                            Console.WriteLine("Invalid Number");
                        }
                        else
                        {
                            Process.Start(new ProcessStartInfo()
                            {
                                Arguments = "\"" + files[found[x-1]] + "\"",
                                FileName = "explorer.exe"
                            });
                            break;
                        }
                        
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Number");
                    }
                }
                
            } else if (found.Count == 0)
            {
                Console.Write("None Found... Do you want to make one? (y/n) : ");
                while(true)
                {
                    String ans = Console.ReadLine();
                    if (ans.ToLower().Equals("y"))
                    {
                        String flname = userinfo[0] + " (" + userinfo[1] + " " + strtype + unitnumber + ", " + userinfo[2] + ")";
                        Console.WriteLine(flname);
                        Directory.CreateDirectory(typepath + "\\" + flname);
                        Process.Start(new ProcessStartInfo()
                        {
                            Arguments = "\"" + typepath + "\\" + flname + "\"",
                            FileName = "explorer.exe"
                        });
                        break;
                    } else if(ans.ToLower().Equals("n"))
                    {
                        break;
                    } else
                    {
                        Console.Write("Invalid. Try Again (y/n) : ");
                    }
                }
            } else
            {
                Process.Start(new ProcessStartInfo()
                {
                    Arguments = "\"" + files[found[0]] + "\"",
                    FileName = "explorer.exe"
                });
            }
            Console.ReadLine();
        }

        public static String[] threewayseperation(String str)
        {
            int sepsz = str.Length / 3;
            int sepre = str.Length % 3;
            String[] sep = { "", "", "" };
            if (sepre == 1)
            {
                sep[0] = str.Substring(0, sepsz + 1).ToLower();
                sep[1] = str.Substring(sepsz+1, sepsz).ToLower();
                sep[2] = str.Substring(sepsz * 2 + 1, sepsz).ToLower();
            }
            else if (sepre == 2)
            {
                sep[0] = str.Substring(0, sepsz+1).ToLower();
                sep[1] = str.Substring(sepsz+1, sepsz+1).ToLower();
                sep[2] = str.Substring(sepsz * 2 + 2, sepsz).ToLower();
            }
            else
            {
                sep[0] = str.Substring(0, sepsz).ToLower();
                sep[1] = str.Substring(sepsz, sepsz).ToLower();
                sep[2] = str.Substring(sepsz*2, sepsz).ToLower();
            }

            return sep;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace Assignment1
{
    class DirWalker
    {
        //List<string> fields = new List<string>();
        string[] field;
        static void Main(string[] args)
        {
            DirWalker p = new DirWalker();
            string path1 = @"C:\MScCDA\5510\2_GIT_Class_Assignment\A00455851_MCDA5510\MCDA5510_Assignments\Sample Data\2019\1\1";
            //string fullpath = Path.GetFullPath(path1);
            Console.WriteLine("Hello World!" + path1);
            p.parseDir(path1);

            Console.ReadLine();
        }

        public void parseDir(String path)
        {
            //Console.WriteLine("Inside Func");
            String[] dirList = Directory.GetDirectories(path);

            if (dirList == null)
                return;

            foreach (string dirpath in dirList)
            {
                if (Directory.Exists(dirpath))
                {
                    
                    //Console.WriteLine("\n");
                    parseDir(dirpath);
                    //Console.WriteLine("\nDir:" + dirpath);
                                        
                }

                
            }

            String[] fileList = Directory.GetFiles(path);
            foreach(String file in fileList)
            {
                string fileExt =  System.IO.Path.GetExtension(file);
                if (fileExt == ".csv")
                {
                    //Console.WriteLine("\nFile :" + file);
                   
                    csvRead(file);
                 }

            }
             
                
           

        }
        public void csvRead(String filename)
        {
            Console.WriteLine("Filepath" + filename);
            String[] split_filepath = filename.Split('\\');
            string day = split_filepath[9];
            string month = split_filepath[8];
            string year = split_filepath[7];
            //Console.WriteLine("Date : " + day + "/" + month + "/" + year);
            
                
            try
            {
                using (TextFieldParser parser = new TextFieldParser(filename))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int row = 0;
                    
                    int invalid_rowcount = 0;
                    int valid_rowcount = 0;

                    String logpath = @"C:\Users\Rubin James\source\repos\A00455851_MCDA5510\Assignment1\Logfile.log";
                    String oppath = @"C:\Users\Rubin James\source\repos\A00455851_MCDA5510\Assignment1\opfile.txt";
                    if (!File.Exists(logpath))
                    {
                        using (StreamWriter sw = File.CreateText(logpath)) ;
                    }
                    if (!File.Exists(oppath))
                    {
                        using (StreamWriter sw = File.CreateText(oppath)) ;
                    }

                    DateTime start_time = DateTime.Now;
                    using (StreamWriter lw = File.AppendText(logpath))
                    {
                        lw.WriteLine("Starting processing of " + filename + " at time " + start_time);
                    }
                    while (!parser.EndOfData)
                    {
                        List<string> final_row = new List<string>();
                        ++row;
                        field = parser.ReadFields();
                       
                                       

                        for (int i = 0; i < 10; i++)
                        {
                            if (field[i] == "First Name")
                            {
                                --row;
                                break;
                            }
                            else if (field[i] == "")
                            {
                                final_row = new List<string>();
                                //row++;
                                invalid_rowcount++;


                                using (StreamWriter lw = File.AppendText(logpath))
                                {
                                    lw.WriteLine("Row " + row + " of " + split_filepath[10] + " is invalid as null values are present");
                                    //++row;
                                    break;
                                }

                            }
                            else
                            {
                                //row++;
                                final_row.Add(field[i]);
                                
                            }
                            

                        }
                        //string[] check = final_row.ToArray();
                        if (!(final_row?.Any() != true))
                        {
                            using (StreamWriter ow = File.AppendText(oppath))
                            {
                                ow.WriteLine(string.Join(", ", final_row));
                            }
                            valid_rowcount++;
                        }



                    }
                    DateTime end_time = DateTime.Now;

                    using (StreamWriter lw = File.AppendText(logpath))
                    {
                        lw.WriteLine("Ending processing of " + filename + " at time " + end_time);
                        lw.WriteLine("Total Time Taken :" + (end_time.Subtract(start_time).TotalMilliseconds+" Milliseconds"));
                        //++row;

                    }
                    /*for (int i = 0; i < fields.Count ;)
                    {
                        Console.WriteLine("Rec:{0} ",string.Join(", ",fields[i]));
                        i = i + 10;
                    }*/

                    /*for(int i=0; i < 27; i++)
                    {
                        Console.WriteLine("Fields:" + fields[i]);
                    }*/

                    //Console.WriteLine("FieldCount" + fields.Count);
                    //string[] fieldlist = fields.ToArray();
                    //Console.WriteLine("First Row " + fieldlist[0]);
                    //Console.WriteLine("Count" + fields.Count);

                    /*for (int i = 0; i < (fields.Count) / 10;)
                    {
                        
                        for(int j=i;j<10;j++)
                        {
                            Console.Write("Records are:{0}", string.Join(", ", fields[j]));
                            Console.Write("\n");
                            i                           
                        }
                        

                    }*/



                }

            }

            catch (IOException ioe)
            {
                Console.WriteLine(ioe.StackTrace);
            }
        }

    }
}

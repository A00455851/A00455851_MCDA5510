using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using log4net;
using log4net.Config;
using System.Reflection;

namespace Assignment1
{
    class DirWalker
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string[] field;
        int invalid_rowcount = 0;
        int valid_rowcount = 0;
        String oppath = @"../../../Output/opfile.csv";
        
        static void Main(string[] args)
        {
            DirWalker p = new DirWalker();
            string input_path = @"C:\MScCDA\5510\2_GIT_Class_Assignment\A00455851_MCDA5510\MCDA5510_Assignments\Sample Data";
            

            if (!File.Exists(p.oppath))
            {
                using (StreamWriter sw = File.CreateText(p.oppath)) ;
            }
            using (StreamWriter ow = File.AppendText(p.oppath))
            {
                ow.WriteLine("First Name, Last Name, Street Number, Street, City, Province, Country, Postal Code, Phone Number, Email Address, Date");
            }
            DateTime exec_start = DateTime.Now;
            p.parseDir(input_path);
            DateTime exec_end = DateTime.Now;
            log.Info("Total time elapsed : " + exec_end.Subtract(exec_start).TotalSeconds + " seconds");

        }

        public void parseDir(String path)
        {
            String[] dirList = Directory.GetDirectories(path);

            try
            {
                if (dirList == null)
                    return;

                foreach (string dirpath in dirList)
                {
                    if (Directory.Exists(dirpath))
                    {

                        parseDir(dirpath);

                    }


                }

                String[] fileList = Directory.GetFiles(path);
                foreach (String file in fileList)
                {
                    string fileExt = System.IO.Path.GetExtension(file);
                    if (fileExt == ".csv")
                    {

                        csvRead(file);
                    }

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file or directory cannot be found.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The file or directory cannot be found.");
            }
            catch (DriveNotFoundException)
            {
                Console.WriteLine("The drive specified in 'path' is invalid.");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("'path' exceeds the maxium supported path length.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to create this file.");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                Console.WriteLine("There is a sharing violation.");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
            {
                Console.WriteLine("The file already exists.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"An exception occurred:\nError code: " +
                                  $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception Encountered : "+ e.StackTrace);
            }





        }
        public void csvRead(String filename)
        {
            string day="", month="", year="";
            //Console.WriteLine("Filepath" + filename);
            String[] split_filepath = filename.Split('\\');
            int size = split_filepath.Length;
            if (Int32.Parse(split_filepath[size-2]) < 10)
            {
                day = "0" + split_filepath[size-2];
            }
            else
                day = split_filepath[9];
            if (Int32.Parse(split_filepath[size-3]) < 10)
            {
                month = "0" + split_filepath[size-3];
            }
            else
                month = split_filepath[size-3];

             year = split_filepath[size-4];
             string datecol = day + "/" + month + "/" + year;

            
            
            try
            {
                using (TextFieldParser parser = new TextFieldParser(filename))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int row = 0;
                                       
                                        
                    var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                    XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                    DateTime start_time = DateTime.Now;
                    
                    log.Info("Starting processing of " + filename);
                    while (!parser.EndOfData)
                    {
                        List<string> final_row = new List<string>();
                        ++row;
                        field = parser.ReadFields();

                                              
                        foreach (string elem in field)
                        {
                            if (elem == "First Name")
                            {
                                --row;
                                break;
                            }
                            else if (elem == "" || elem == null)
                            {
                                final_row = new List<string>();
                                invalid_rowcount++;


                                {
                                    
                                    log.Error("Row " + row + " of " + split_filepath[10] + " is invalid as null values are present");
                                    break;
                                }

                            }
                            else
                            {
                                final_row.Add(elem);
                                
                            }
                            

                        }
                        if (!(final_row?.Any() != true))
                        {
                            using (StreamWriter ow = File.AppendText(oppath))
                            {
                                ow.WriteLine(string.Join(", ", final_row) + ", "+datecol );
                            }
                            valid_rowcount++;
                        }


                    }
                    DateTime end_time = DateTime.Now;

                    
                    log.Info("Ending processing of " + filename);
                    log.Info("Total Time Taken :" + (end_time.Subtract(start_time).TotalMilliseconds + " Milliseconds"));
                    
                    
                }

            }

            catch (IOException ioe)
            {
                Console.WriteLine(ioe.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Encountered : " + e.StackTrace);
            }
            
            log.Info("Total no of records parsed :" + (valid_rowcount+invalid_rowcount));
            log.Info("Total no of valid records :" + valid_rowcount);
            log.Info("Tota no of invalid records :" + invalid_rowcount);
        }

    }
}

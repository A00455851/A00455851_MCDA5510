using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Assignment1
{
    class DirWalker
    {
        
        static void Main(string[] args)
        {
            DirWalker p = new DirWalker();
            string path1 = @"C:\MScCDA\5510\2_GIT_Class_Assignment\A00455851_MCDA5510\MCDA5510_Assignments\Assignment1\Assignment1";
            //string fullpath = Path.GetFullPath(path1);
            Console.WriteLine("Hello World!" + path1);
            p.parseDir(path1);
            Console.ReadLine();
        }

        public void parseDir(String path)
        {
            //Console.WriteLine("Inside Func");
            String[] dirList = Directory.GetDirectories(path);
            
            if (dirList == null) return;

            foreach (string dirpath in dirList)
            {
                if (Directory.Exists(dirpath))
                {
                    //Console.WriteLine("\n");
                    parseDir(dirpath);
                    Console.WriteLine("\nDir:" + dirpath);
                }

                
            }

            String[] fileList = Directory.GetFiles(path);
            foreach(String file in fileList)
            {
                string fileExt =  System.IO.Path.GetExtension(file);
                if (fileExt == ".csv")
                {
                    Console.WriteLine("\nFile :" + file);
                 }
            }
            

        }

       
    }
}

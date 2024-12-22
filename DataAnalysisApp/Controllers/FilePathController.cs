using DataAnalysisApp.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysisApp.Controllers
{
    public class FilePathController
    {
        /***** Fill these in if you wish to supply filepath from inside code *****/
        public string LOGIN_DATA_FILEPATH {  get; private set; }     = @"C:\Users\frede\source\repos\DataAnalysisApp\DataAnalysisApp\Data\login_data.csv";
        public string HISTORICAL_DATA_FILEPATH { get; private set; } = @"C:\Users\frede\source\repos\DataAnalysisApp\DataAnalysisApp\Data\historical_data.csv";

        public FilePathController(string[] args) 
        {
            CheckForParameterFilePath(args);
        }

        private void CheckForParameterFilePath(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("No arguments provided. Executing using filepaths defined in FilePathController.cs");
            }
            for (int i = 0; i < args.Length; i++) 
            { 
                if (args[i] == "-l" && i + 1 < args.Length) 
                {
                     LOGIN_DATA_FILEPATH = args[i + 1];
                } else if (args[i] == "-h" && i + 1 < args.Length) 
                {
                    HISTORICAL_DATA_FILEPATH = args[i + 1]; 
                } 
            }

            if (string.IsNullOrEmpty(LOGIN_DATA_FILEPATH) || string.IsNullOrEmpty(HISTORICAL_DATA_FILEPATH)) 
            { 
                Console.WriteLine("Usage: -l <login.csv> -h <history.csv>"); 
                return; 
            }
            if (!File.Exists(LOGIN_DATA_FILEPATH) || !File.Exists(HISTORICAL_DATA_FILEPATH)) 
            { 
                Console.WriteLine("One or both files do not exist."); 
                return; 
            }
            if (Path.GetExtension(LOGIN_DATA_FILEPATH).ToLower() != ".csv" || Path.GetExtension(HISTORICAL_DATA_FILEPATH).ToLower() != ".csv") 
            { 
                Console.WriteLine("Both files must be CSV files."); 
                return; 
            }

            /*if (args.Length == 0)
            {
                Console.WriteLine("No filepath provided as parameter. Executing using filepath provided in code.");
            }
            else
            {
                foreach(string param in args) 
                {
                    if (!File.Exists(param))
                    {
                        Console.WriteLine($"The file at {param} does not exist.");
                        return;
                    }
                    else
                    {
                        if (Path.GetExtension(param).ToLower() != ".csv")
                        {
                            Console.WriteLine($"The file at {param} is not a CSV file.");
                            return;
                        }
                    }
                }
            }*/
        }
    }
}

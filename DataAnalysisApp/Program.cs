using DataAnalysisApp.Controllers;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DataAnalysisApp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            MainController mc = new MainController(args);
            mc.Run();
        }
    }
}

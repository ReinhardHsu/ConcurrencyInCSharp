using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace ReportingProgress
{
    class Program
    {
        static void Main(string[] args)
        {
            var progress = new Progress<double>();
            progress.ProgressChanged += (sender, argarr) =>
            {
                Console.WriteLine(argarr.ToString());
            };

            AsyncContext.Run(() =>
                MyMethodAsync(progress)
            );
            Console.ReadKey();
        }

        static async Task MyMethodAsync(IProgress<double> progress = null)
        {
            double percentComplete = 0;
            while (percentComplete<=100)
            {
                percentComplete += 1;

                if (progress != null)
                {
                    progress.Report(percentComplete);
                }
            }
        }
    }
}

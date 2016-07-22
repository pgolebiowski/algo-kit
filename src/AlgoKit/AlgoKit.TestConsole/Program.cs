using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoKit.TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("AlgoKit Test Console");
            Console.WriteLine("Doing stuff...");

            var instance = new ToBeDeleted();
            Console.WriteLine(instance.Fixed(true));

            Console.WriteLine();
            Console.WriteLine("Finished");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}

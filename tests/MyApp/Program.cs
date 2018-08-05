using System;

namespace MyApp {
    class Program {

        static void LockNull() {
            lock (null) {
                Console.WriteLine("Bug ...");
            }
        }

        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
        }
    }
}

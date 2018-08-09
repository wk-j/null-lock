using System;

namespace MyApp {
    class Program {
        static void A() {
            lock (null) {
                Console.WriteLine("This is ...");
            }
        }
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
        }
    }
}

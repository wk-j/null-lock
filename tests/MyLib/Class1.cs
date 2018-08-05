using System;

namespace MyLib {
    public class Class1 {
        public void Lock() {
            lock (null) {
                Console.WriteLine("Bug ...");
            }
        }
    }
}

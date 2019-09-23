using System;
using System.ComponentModel;

namespace Opgave5TCP
{
    class Program
    {
        static void Main(string[] args)
        {
            Worker work = new Worker();
            work.Start();
        }
    }
}

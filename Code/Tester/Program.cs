using System;
using System.Threading;



Console.WriteLine("*****Synchronizing Threads *****\n");
Printer p = new Printer();
// Make 10 threads that are all pointing to the same
// method on the same object.
Thread[] threads = new Thread[10];
for (int i = 0; i < 10; i++)
{
    threads[i] = new Thread(p.PrintNumbers)
    {
        Name = $"Worker thread #{i}"
    };
}
// Now start each one.
foreach (Thread t in threads)
{
    t.Start();
}
Console.ReadLine();

public class Printer
{
    public void PrintNumbers()
    {
        // Display Thread info.
        Console.WriteLine("-> {0} is executing PrintNumbers()",
        Thread.CurrentThread.Name);
        // Print out numbers.
        Console.Write("Your numbers: ");
        for (int index = 0; index < 10; index++)
        {
            Console.Write("{0}, ", index);
            Thread.Sleep(50);
        }
        Console.WriteLine();
    }
}
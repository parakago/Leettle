using BenchmarkDotNet.Running;
using System;

namespace Leettle.Data.Benchmark
{
    public class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<OraMngVsLeettleVsNHiberSelectTest>();
            Console.ReadLine();
        }
    }
}

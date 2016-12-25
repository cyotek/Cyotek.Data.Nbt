using BenchmarkDotNet.Running;

namespace Benchmarks
{
  class Program
  {
    #region Static Methods

    static void Main(string[] args)
    {
      BenchmarkRunner.Run<TagWriterBenchmarks>();
    }

    #endregion
  }
}

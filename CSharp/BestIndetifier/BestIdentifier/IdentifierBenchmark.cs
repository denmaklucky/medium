using BenchmarkDotNet.Attributes;
using Visus.Cuid;

namespace BestIdentifier;

public class IdentifierBenchmark
{
    [Benchmark]
    public void GuidV4() => Guid.NewGuid();

    [Benchmark]
    public void GuidV7() => Guid.CreateVersion7();

    [Benchmark]
    public void Ulid() => System.Ulid.NewUlid();

    [Benchmark]
    public void Suid() => BestIdentifier.Suid.New();

    [Benchmark]
    public void Cuid() => new Cuid2();
}
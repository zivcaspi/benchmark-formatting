using System;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class FormattableStringBenchmark
{
    private bool m_never = Guid.NewGuid().ToString().Equals("bla");
    private bool m_always = !Guid.NewGuid().ToString().Equals("bla");
    private int m_i1 = 1;
    private int m_i2 = 2;
    private int m_i3 = 3;
    private int m_i4 = 4;

    public static void Main(string[] args)
    {
        var config = DefaultConfig.Instance;
        BenchmarkRunner.Run(typeof(FormattableStringBenchmark), config);
    }

    [Benchmark]
    public string False_Simple()
    {
        return InvokeSimple(m_never, "{0}/{1}/{2}/{3}", m_i1, m_i2, m_i3, m_i4);
    }

    [Benchmark]
    public string True_Simple()
    {
        return InvokeSimple(m_always, "{0}/{1}/{2}/{3}", m_i1, m_i2, m_i3, m_i4);
    }

    [Benchmark]
    public string False_AggressiveInlining()
    {
        return InvokeAggressiveInlining(m_never, "{0}/{1}/{2}/{3}", m_i1, m_i2, m_i3, m_i4);
    }

    [Benchmark]
    public string True_AggressiveInlining()
    {
        return InvokeAggressiveInlining(m_always, "{0}/{1}/{2}/{3}", m_i1, m_i2, m_i3, m_i4);
    }

    [Benchmark]
    public string False_Formattable()
    {
        return InvokeFormattable(m_never, $"{m_i1}/{m_i2}/{m_i3}/{m_i4}");
    }

    [Benchmark]
    public string True_Formattable()
    {
        return InvokeFormattable(m_always, $"{m_i1}/{m_i2}/{m_i3}/{m_i4}");
    }

    private string InvokeSimple(bool cond, string format, params object[] args)
    {
        if (cond)
        {
            return string.Format(format, args);
        }
        return null;
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private string InvokeAggressiveInlining(bool cond, string format, params object[] args)
    {
        if (!cond)
        {
            return null;
        }
        return InvokeSimple(cond, format, args);
    }

    private string InvokeFormattable(bool cond, FormattableString message)
    {
        if (!cond)
        {
            return null;
        }
        return message.ToString();
    }
}

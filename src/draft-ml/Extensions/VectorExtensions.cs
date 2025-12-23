namespace draft_ml.Extensions;

public static class VectorExtensions
{
    public static Vector Multiply(this Vector a, float b)
    {
        var output = a.Memory.ToArray();
        for (int i = 0; i < output.Length; i++)
        {
            output[i] *= b;
        }
        return new Vector(new ReadOnlyMemory<float>(output));
    }

    public static Vector Divide(this Vector a, float b)
    {
        var output = a.Memory.ToArray();
        for (int i = 0; i < output.Length; i++)
        {
            output[i] /= b;
        }
        return new Vector(new ReadOnlyMemory<float>(output));
    }

    public static Vector Subtract(this Vector a, Vector b)
    {
        var output = a.Memory.ToArray();
        var subtracter = b.Memory.ToArray();
        for (int i = 0; i < output.Length; i++)
        {
            output[i] -= subtracter[i];
        }
        return new Vector(new ReadOnlyMemory<float>(output));
    }

    public static Vector Add(this Vector a, Vector b)
    {
        var output = a.Memory.ToArray();
        var adder = b.Memory.ToArray();
        for (int i = 0; i < output.Length; i++)
        {
            output[i] += adder[i];
        }
        return new Vector(new ReadOnlyMemory<float>(output));
    }

    public static double L2DNorm(this Vector a)
    {
        var aVal = a.Memory.ToArray();

        // Sum squares
        double output = aVal.Aggregate((x, x2) => x += (x2 * x2));

        // Sqrt
        return Math.Sqrt(output);
    }

    public static double L2DNormSq(this Vector a)
    {
        var aVal = a.Memory.ToArray();

        return aVal.Aggregate((x, x2) => x += (x2 * x2));
    }

    public static Vector Clone(this Vector a)
    {
        return new Vector(new ReadOnlyMemory<float>(a.Memory.ToArray()));
    }
}

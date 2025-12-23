using draft_ml.Extensions;
using Pgvector;

namespace draft_ml.UnitTests
{
    public class VectorExtensionTests
    {
        [Fact]
        public void Given_Vector_When_Clone_Then_Equal()
        {
            var vec = new Vector(new ReadOnlyMemory<float>([1.5f, 1.5f, 1.5f]));
            var vecClone = vec.Clone();

            Assert.Equal(vec, vecClone);
        }

        [Fact]
        public void Given_Vector_When_Multiply_Then_Correct()
        {
            var vec = new Vector(new ReadOnlyMemory<float>([1.5f, 1.5f, 1.5f]));
            var vecMultiplied = vec.Multiply(3);

            var expectedOutput = new Vector(new ReadOnlyMemory<float>([4.5f, 4.5f, 4.5f]));

            Assert.Equal(expectedOutput, vecMultiplied);
        }

        [Fact]
        public void Given_Vector_When_Divide_Then_Correct()
        {
            var vec = new Vector(new ReadOnlyMemory<float>([4.5f, 4.5f, 4.5f]));
            var vecDivided = vec.Divide(3);

            var expectedOutput = new Vector(new ReadOnlyMemory<float>([1.5f, 1.5f, 1.5f]));

            Assert.Equal(expectedOutput, vecDivided);
        }

        [Fact]
        public void Given_Vector_When_Subtract_Then_Correct()
        {
            var vec = new Vector(new ReadOnlyMemory<float>([4.5f, 4.5f, 4.5f]));
            var vec2 = new Vector(new ReadOnlyMemory<float>([1.5f, 1.5f, 1.5f]));
            var vecSubtracted = vec.Subtract(vec2);

            var expectedOutput = new Vector(new ReadOnlyMemory<float>([3f, 3f, 3f]));

            Assert.Equal(expectedOutput, vecSubtracted);
        }

        [Fact]
        public void Given_Vector_When_Add_Then_Correct()
        {
            var vec = new Vector(new ReadOnlyMemory<float>([4.5f, 4.5f, 4.5f]));
            var vec2 = new Vector(new ReadOnlyMemory<float>([1.5f, 1.5f, 1.5f]));
            var vecAdded = vec.Add(vec2);

            var expectedOutput = new Vector(new ReadOnlyMemory<float>([6f, 6f, 6f]));

            Assert.Equal(expectedOutput, vecAdded);
        }

        [Fact]
        public void Given_Vector_When_Calculate_L2DNorm_Then_Correct()
        {
            var vec = new Vector(new ReadOnlyMemory<float>([3f, 3f, 3f]));

            Assert.Equal(4.58257569495584, vec.L2DNorm());
        }

        [Fact]
        public void Given_Vector_When_Calculate_L2DNormSq_Then_Correct()
        {
            var vec = new Vector(new ReadOnlyMemory<float>([3f, 3f, 3f]));

            Assert.Equal(21, vec.L2DNormSq());
        }
    }
}

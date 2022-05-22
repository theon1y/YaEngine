using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Silk.NET.Maths;
using Xunit;

namespace Tests
{
    public class MatrixSpanTest
    {
        [Fact]
        public unsafe void ShouldConvertMatrixToSpanAndBack()
        {
            var random = new Random((int)DateTime.UtcNow.Ticks);
            var count = 64;
            var matrix = CreateRandom(() => (int) (random.NextDouble() * 255));
            
            var span = new Span<float>(Unsafe.AsPointer(ref matrix), 16);

            var spanMatrix = new Matrix4x4();
            var i = 0;
            foreach (var value in span)
            {
                var idx = i % 16;
                SetMatrixValue(ref spanMatrix, idx, value);
                ++i;
            }

            Assert.Equal(matrix, spanMatrix);
        }
        
        [Fact]
        public void ShouldConvertToSpanAndBack()
        {
            var random = new Random((int)DateTime.UtcNow.Ticks);
            var count = 64;
            var matrices = Enumerable.Range(0, count)
                .Select(_ => CreateRandom(() => (int) (random.NextDouble() * 255)))
                .ToArray();
            
            var matrixSpan = matrices.AsSpan();
            var span = MemoryMarshal.Cast<Matrix4x4, float>(matrixSpan);

            var spanMatrices = Enumerable.Range(0, count)
                .Select(_ => new Matrix4x4())
                .ToArray();
            var i = 0;
            foreach (var value in span)
            {
                var matrix = i / 16;
                var idx = i % 16;
                SetMatrixValue(ref spanMatrices[matrix], idx, value);
                ++i;
            }

            for (i = 0; i < count; ++i)
            {
                Assert.Equal(matrices[i], spanMatrices[i]);
            }
        }

        private void SetMatrixValue(ref Matrix4x4 matrix, int idx, float value)
        {
            if (idx == 0) matrix.M11 = value;
            if (idx == 1) matrix.M12 = value;
            if (idx == 2) matrix.M13 = value;
            if (idx == 3) matrix.M14 = value;
            if (idx == 4) matrix.M21 = value;
            if (idx == 5) matrix.M22 = value;
            if (idx == 6) matrix.M23 = value;
            if (idx == 7) matrix.M24 = value;
            if (idx == 8) matrix.M31 = value;
            if (idx == 9) matrix.M32 = value;
            if (idx == 10) matrix.M33 = value;
            if (idx == 11) matrix.M34 = value;
            if (idx == 12) matrix.M41 = value;
            if (idx == 13) matrix.M42 = value;
            if (idx == 14) matrix.M43 = value;
            if (idx == 15) matrix.M44 = value;
        }

        private Matrix4x4 CreateRandom(Func<float> generator)
        {
            return new(
                generator(),
                generator(),
                generator(),
                generator(),

                generator(),
                generator(),
                generator(),
                generator(),

                generator(),
                generator(),
                generator(),
                generator(),

                generator(),
                generator(),
                generator(),
                generator()
            );
        }
    }
}

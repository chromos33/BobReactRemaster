using BobReactRemaster.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BobReactRemaster.Tests.Helper
{
    internal class TestableRandomGenerator : IRandomGenerator
    {
        public int Result = 0;
        public TestableRandomGenerator()
        {

        }
        public void SetResult(int r)
        {
            Result = r;
        }
        public int Generate(int min, int max)
        {
            return Result;
        }
    }
}

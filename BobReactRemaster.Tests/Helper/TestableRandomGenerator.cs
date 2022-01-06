using BobReactRemaster.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BobReactRemaster.Tests.Helper
{
    internal class TestableRandomGenerator : IRandomGenerator
    {
        public int Result = 0;
        public List<int> Results = new List<int>();
        public TestableRandomGenerator()
        {

        }
        public void SetResult(int r)
        {
            Result = r;
        }

        public void AddResult(int r)
        {
            Results.Add(r);
        }
        public int Generate(int min, int max)
        {
            if (Results.Count > 0)
            {
                var num = Results.First();
                Results.Remove(num);
                return num;
            }
            return Result;
        }
    }
}

using System;
using System.Linq;
using System.IO;
using MriCSharp.App;
using Xunit;

namespace MriCSharp.Tests
{
    public class DiffCalculatorTests
    {
        [Fact]
        public void ShouldGetTheDiffOfTheMethod()
        {
            var diff = DiffCalculator.GetDiffBetween("Data/FileBefore.txt", "Data/FileAfter.txt", "TestMethod");

            Assert.Equal(true, diff.HasChanged);
            Assert.Equal(5, diff.Churn);
        }

        [Fact]
        public void ShouldGetTheDiffOfTheMethodWithIdenticLines()
        {
            var diff = DiffCalculator.GetDiffBetween("Data/FileAfter.txt", "Data/FileAfter2.txt", "TestMethod");

            Assert.Equal(diff.HasChanged, true);
            Assert.Equal(diff.Churn, 5);
        }
    }
}
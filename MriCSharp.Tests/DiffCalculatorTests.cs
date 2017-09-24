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
            Assert.Equal(7, diff.Churn);
        }

        [Fact]
        public void ShouldGetTheDiffOfTheMethodWithIdenticLines()
        {
            var diff = DiffCalculator.GetDiffBetween("Data/FileAfter.txt", "Data/FileAfter2.txt", "TestMethod");

            Assert.Equal(true, diff.HasChanged);
            Assert.Equal(5, diff.Churn);
        }

        [Fact]
        public void ShouldGetTheInfoOfTheSecondFileIfTheFirstOneDoesntExists()
        {
            var diff = DiffCalculator.GetDiffBetween("Data/NoFile.txt", "Data/FileAfter.txt", "TestMethod");

            Assert.Equal(true, diff.HasChanged);
            Assert.Equal(9, diff.Churn);
        }

        [Fact]
        public void ShouldGetTheInfoOfTheFirstFileIfTheSecondOneDoesntExists()
        {
            var diff = DiffCalculator.GetDiffBetween("Data/FileAfter.txt", "Data/NoFile.txt", "TestMethod");

            Assert.Equal(true, diff.HasChanged);
            Assert.Equal(9, diff.Churn);
        }

        [Fact]
        public void ShouldGetTheInfoOfOverloadedMethod()
        {
            var diff = DiffCalculator.GetDiffBetween("Data/Before.txt", "Data/ControllerActionInvokerTest.txt", "CreateInvoker");

            Assert.Equal(true, diff.HasChanged);
            Assert.Equal(170, diff.Churn);
        }
    }
}
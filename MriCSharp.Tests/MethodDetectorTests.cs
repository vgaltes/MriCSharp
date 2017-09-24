using System;
using System.Linq;
using MriCSharp.App;
using Xunit;

namespace MriCSharp.Tests
{
    public class MethodDetectorTests
    {
        [Fact]
        public void ShouldGetTheNameOfTheMethods()
        {
            var filePath = "Data/ControllerActionInvokerTest.txt";
            var methodNames = MethodDetector.GetMethodNamesOf(filePath);

            Assert.Equal(methodNames.Count(), 68);
        }

        [Fact]
        public void ShouldReturnAnEmptyListIfTheFileDoesntExist()
        {
            var filePath = "an/invalid/file.txt";
            var methodNames = MethodDetector.GetMethodNamesOf(filePath);

            Assert.Equal(methodNames.Count(), 0);
        }
    }
}
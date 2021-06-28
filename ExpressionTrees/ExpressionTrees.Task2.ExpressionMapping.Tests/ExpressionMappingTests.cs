using System.Text;
using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var builder = new StringBuilder("Value");
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var source = new Foo() { ProstoString = "Test", Biulder = builder , Count = 10};

            var res = mapper.Map(source);

            Assert.AreEqual(res.ProstoString, source.ProstoString);
            Assert.AreEqual(res.Count, source.Count);
            Assert.AreEqual(res.Biulder.ToString(), source.Biulder.ToString());
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Leettle.Data.Test
{
    [TestClass]
    public class FileSystemSqlProviderTest
    {
        [TestMethod]
        public void TestFindSql()
        {
            string baseDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string preparedSqlDir = Path.Combine(baseDir, "preparedSqls");

            var provider = new FileSystemSqlProvider(preparedSqlDir, false);

            string a1 = provider.GetSql("sample.select.test.all");
            string a2 = provider.GetSql("test.select.test.all");
            Assert.AreEqual(a1, a2);

            string b1 = provider.GetSql("test.select.test.all.orderby1");
            string b2 = provider.GetSql("sample.select.test.all.orderby2");
            Assert.AreEqual(b1, b2);

            try
            {
                provider.GetSql("test.select.test.monitor");
                Assert.IsTrue(false);
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }
    }
}

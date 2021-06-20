using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObservableSharp.Test
{
    [TestClass]
    public class UnitTest
    {
        
        public ObservableProperty<int> A { get; set; }
        public ObservableProperty<int> B { get; set; }
        public ObservableProperty<int> Sum { get; set; }

        [TestInitialize]
        public void Init()
        {
            A = new();
            B = new();
            Sum = new();
        }

        [TestMethod]
        public void TestMethod()
        {
            Sum.Compute(this)
                .DependsOn(x => x.A)
                .DependsOn(x => x.B)
                .Apply(x => x.A + x.B);

            A.Value = 2;
            B.Value = 3;

            Assert.AreEqual(5, Sum.Value);
        }

    }
}

using TP_Boleto;
namespace TP_Boleto_Test

{
    public class TarjetaTest
    {
        Tarjeta tarj;
        [SetUp]
        public void Setup()
        {
            tarj = new Tarjeta();
        }

        [Test]
        [TestCase(2000)]
        [TestCase(3000)]
        [TestCase(4000)]
        [TestCase(5000)]
        [TestCase(6000)]
        [TestCase(7000)]
        [TestCase(8000)]
        [TestCase(9000)]
        public void CargarTest(double c)
        {
            Assert.That(tarj.Cargar(c), Is.EqualTo(true));
            
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
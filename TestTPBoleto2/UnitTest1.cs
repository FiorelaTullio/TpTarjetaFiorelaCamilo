using TpBoleto2;

namespace TestTPBoleto2
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
        [TestCase(1000)]
        [TestCase(1500)]
        public void PuedePagarTest(float cargaInicial)
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Saldo = cargaInicial;
            Colectivo colectivo = new Colectivo();
            Assert.NotNull(colectivo.pagarCon(tarjeta));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-500)]
        public void NoPuedePagar(float cargaInicial)
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Saldo = cargaInicial;
            Colectivo colectivo = new Colectivo();
            Assert.IsNull(colectivo.pagarCon(tarjeta));
        }


        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
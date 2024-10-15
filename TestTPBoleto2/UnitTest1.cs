using System.Security.Cryptography.X509Certificates;
using TpBoleto2;

namespace TestTPBoleto2
{
    public class TarjetaTest
    {
        Tarjeta tarj;
        TarjetaFranquciaCompleta boleto
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
        public void PuedePagarTest(double cargaInicial)
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Saldo = cargaInicial;
            Colectivo colectivo = new Colectivo();
            Assert.NotNull(colectivo.pagarCon(tarjeta));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-500)]
        public void NoPuedePagar(double cargaInicial)
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Saldo = cargaInicial;
            Colectivo colectivo = new Colectivo();
            Assert.IsNull(colectivo.pagarCon(tarjeta));
        }

        [Test]
        [TestCase(460)]
        [TestCase(500)]
        [TestCase(700)]
        [TestCase(1000)]
        public void SaldoAdeudado(float cargaInicial)
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.Saldo = cargaInicial;
            double saldoInicial = tarjeta.Saldo;
            Colectivo colectivo = new Colectivo();
            colectivo.pagarCon(tarjeta);
            Assert.That(tarjeta.Saldo, Is.EqualTo(saldoInicial - Boleto.Precio));

        }

        [Test]
        [TestCase(460)]
        [TestCase(500)]
        [TestCase(700)]
        [TestCase(1000)]
        public void MedioSaldoTest(float cargaInicial)
        {
            TarjetaFranquiciaMedia tarjeta = new TarjetaFranquiciaMedia();
            tarjeta.Saldo = cargaInicial;
            double saldoInicial = tarjeta.Saldo;
            Colectivo colectivo = new Colectivo();
            colectivo.pagarCon(tarjeta);
            Assert.That(tarjeta.Saldo, Is.EqualTo(saldoInicial - Boleto.Precio / 2));

        }

        [Test]
        [TestCase(0)]
        [TestCase(500)]
        [TestCase(700)]
        [TestCase(1000)]
        public void CompletoSaldoTest(float cargaInicial)
        {
            TarjetaFranquciaCompleta tarjeta = new TarjetaFranquciaCompleta();
            tarjeta.Saldo = cargaInicial;
            double saldoInicial = tarjeta.Saldo;
            Colectivo colectivo = new Colectivo();
            Assert.NotNull(colectivo.pagarCon(tarjeta));

        }

        public void CantidadViajesGratisTest()
        {

        }



        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
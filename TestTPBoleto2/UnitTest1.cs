using System.Security.Cryptography.X509Certificates;
using TpBoleto2;

namespace TestTPBoleto2
{
    public class TarjetaTest
    {
        Tarjeta tarjeta;
        TarjetaFranquciaCompleta tarjetaCompleta;
        TarjetaFranquiciaMedia tarjetaMedia;


        Colectivo colectivo;
        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta(4);
            tarjetaCompleta = new TarjetaFranquciaCompleta(2);
            tarjetaMedia = new TarjetaFranquiciaMedia(3);
            colectivo = new Colectivo("122 Roja");
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
            Assert.That(tarjeta.Cargar(c), Is.EqualTo(true));

        }

        [Test]
        [TestCase(1000)]
        [TestCase(1500)]
        public void PuedePagarTest(double cargaInicial)
        {
            tarjeta.Saldo = cargaInicial;
            Assert.NotNull(colectivo.pagarCon(tarjeta));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-500)]
        public void NoPuedePagar(double cargaInicial)
        {
            tarjeta.Saldo = cargaInicial;
            Assert.IsNull(colectivo.pagarCon(tarjeta));
        }

        [Test]
        [TestCase(460)]
        [TestCase(500)]
        [TestCase(700)]
        [TestCase(1000)]
        public void SaldoAdeudado(float cargaInicial)
        {
            tarjeta.Saldo = cargaInicial;
            double saldoInicial = tarjeta.Saldo;
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
            tarjetaMedia.Saldo = cargaInicial;
            double saldoInicial = tarjetaMedia.Saldo;
            colectivo.pagarCon(tarjetaMedia);
            Assert.That(tarjetaMedia.Saldo, Is.EqualTo(saldoInicial - Boleto.Precio / 2));

        }

        [Test]
        [TestCase(0)]
        [TestCase(500)]
        [TestCase(700)]
        [TestCase(1000)]
        public void CompletoSaldoTest(float cargaInicial)
        {
            tarjetaCompleta.Saldo = cargaInicial;
            double saldoInicial = tarjetaCompleta.Saldo;
            Assert.NotNull(colectivo.pagarCon(tarjetaCompleta));


        }

        public void IntervaloMedioBoletoTest()
        {
            tarjetaMedia.Cargar(5000);
            Boleto boleto1 = colectivo.pagarCon(tarjetaMedia);
            Boleto boleto2 = colectivo.pagarCon(tarjetaMedia);
            Assert.Null(boleto2);
        }





        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
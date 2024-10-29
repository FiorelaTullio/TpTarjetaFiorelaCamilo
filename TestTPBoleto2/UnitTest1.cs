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
            Assert.That(tarjeta.CargarAcreditar(c), Is.EqualTo(true));

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
        [TestCase(600)]
        [TestCase(700)]
        [TestCase(800)]
        [TestCase(900)]
        public void SaldoAdeudado(float cargaInicial)
        {
            tarjeta.Saldo = cargaInicial;
            double saldoInicial = tarjeta.Saldo;
            colectivo.pagarCon(tarjeta);
            Assert.That(tarjeta.Saldo, Is.EqualTo(saldoInicial - Colectivo.PrecioBoleto));

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
            Assert.That(tarjetaMedia.Saldo, Is.EqualTo(saldoInicial - Colectivo.MedioPrecioBoleto));
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

        [Test]
        public void probarBoletoNormal()
        {
            tarjeta.CargarAcreditar(3000);
            Boleto? boleto = colectivo.pagarCon(tarjeta);
            Assert.NotNull(boleto);
            Assert.That(boleto.SacadoCon, Is.EqualTo(tarjeta.GetType().Name));
        }

        [Test]
        public void probarBoletoMedio()
        {
            tarjetaMedia.CargarAcreditar(3000);
            Boleto? boleto = colectivo.pagarCon(tarjetaMedia);
            Assert.NotNull(boleto);
            Assert.That(boleto.SacadoCon, Is.EqualTo(tarjetaMedia.GetType().Name));
        }

        [Test]
        public void probarBoletoCompleto()
        {
            tarjetaCompleta.CargarAcreditar(3000);
            Boleto? boleto = colectivo.pagarCon(tarjetaCompleta);
            Assert.NotNull(boleto);
            Assert.That(boleto.SacadoCon, Is.EqualTo(tarjetaCompleta.GetType().Name));
        }


        [Test]
        public void IntervaloMedioBoletoTest()
        {
            tarjetaMedia.CargarAcreditar(5000);
            Boleto? boleto1 = colectivo.pagarCon(tarjetaMedia);
            Boleto? boleto2 = colectivo.pagarCon(tarjetaMedia);
            Assert.Null(boleto2);
        }

        [Test]
        public void MedioBoletoCantidadTest()
        {
            tarjetaMedia.CargarAcreditar(2000);
            double cargaAntes = tarjetaMedia.Saldo;
            tarjetaMedia.BoletosSacadosHoy = (DateTime.Today, TarjetaFranquiciaMedia.MaximosBoletosPorDia);
            Boleto? boleto = colectivo.pagarCon(tarjetaMedia);
            double cargaDespues = tarjetaMedia.Saldo;
            Assert.NotNull(boleto);
            Assert.That(cargaAntes - cargaDespues, Is.EqualTo(Colectivo.PrecioBoleto));
        }

        [Test]
        public void SoloDosBoletosGratuitosTest()
        {
            tarjetaCompleta.CargarAcreditar(4000);
            double saldoAntes = tarjetaCompleta.Saldo;
            Assert.That(tarjetaCompleta.cantidadBoletosSacados, Is.EqualTo(0));
            Boleto? boleto1 = colectivo.pagarCon(tarjetaCompleta);
            Assert.That(tarjetaCompleta.cantidadBoletosSacados, Is.EqualTo(1));
            Boleto? boleto2 = colectivo.pagarCon(tarjetaCompleta);
            Assert.That(tarjetaCompleta.cantidadBoletosSacados, Is.EqualTo(2));
            Assert.NotNull(boleto1);
            Assert.NotNull(boleto2);
            Assert.That(tarjetaCompleta.Saldo, Is.EqualTo(saldoAntes));
            Boleto? boleto3 = colectivo.pagarCon(tarjetaCompleta);
            Assert.That(tarjetaCompleta.cantidadBoletosSacados, Is.EqualTo(3));
            Assert.NotNull(boleto3);
            double saldoEsperado = saldoAntes - Colectivo.PrecioBoleto;
            Assert.That(tarjetaCompleta.Saldo, Is.EqualTo(saldoEsperado));
        }

        [Test]
        public void CargaLimitadaTest()
        {
            double i = tarjeta.Saldo;
            for (; i < Tarjeta.SaldoMaximo; i += 2000f)
            {
                Assert.That(tarjeta.Saldo, Is.EqualTo(i));
                tarjeta.CargarAcreditar(2000f);
            }

            Assert.That(tarjeta.Saldo, Is.EqualTo(Tarjeta.SaldoMaximo));
            Assert.That(tarjeta.pendienteDeAcreditacion, Is.EqualTo(i - Tarjeta.SaldoMaximo));
        }

        [Test]
        public void AcreditarAlSacarTest()
        {
            while(tarjeta.pendienteDeAcreditacion == 0)
            {
                tarjeta.CargarAcreditar(2000);
            }

            double pendienteAntes = tarjeta.pendienteDeAcreditacion;
            colectivo.pagarCon(tarjeta);
            double pendienteDespues = tarjeta.pendienteDeAcreditacion;
            Assert.That(pendienteDespues, Is.LessThan(pendienteAntes));
        }

        [Test]
        public void DescuentosTest()
        {
            int iteraciones = 0;
            while(0 <= iteraciones && iteraciones <= 29)
            {
                tarjeta.Saldo = Colectivo.PrecioBoleto;
                colectivo.pagarCon(tarjeta);
                Assert.That(tarjeta.Saldo, Is.EqualTo(0));
                iteraciones++;
            }

            while (30 <= iteraciones && iteraciones <= 79)
            {
                tarjeta.Saldo = Colectivo.PrecioBoleto * 0.8;
                colectivo.pagarCon(tarjeta);
                Assert.That(tarjeta.Saldo, Is.EqualTo(0));
                iteraciones++;
            }
            while (80 <= iteraciones && iteraciones <= 100)
            {
                tarjeta.Saldo = Colectivo.PrecioBoleto * 0.75;
                colectivo.pagarCon(tarjeta);
                Assert.That(tarjeta.Saldo, Is.EqualTo(0));
                iteraciones++;
            }
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
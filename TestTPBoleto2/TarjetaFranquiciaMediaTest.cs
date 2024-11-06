using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TpBoleto2;

namespace TestTPBoleto2
{
    public class TarjetaFranquiciaMediaTest
    {
        // Declaración de variables de tarjetas y colectivos.
        TarjetaFranquiciaMedia tarjetaMedia;

        Colectivo colectivo;

        // Configuración de tarjetas y colectivos.
        [SetUp]
        public void Setup()
        {
            tarjetaMedia = new TpBoleto2.TarjetaFranquiciaMedia(3);
            colectivo = new Colectivo("122 Roja");
        }

        // Chequea que se descuente el precio del medio boleto del saldo de la tarjeta media.
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
            Assert.That(tarjetaMedia.Saldo, Is.EqualTo(saldoInicial - colectivo.MedioPrecioBoleto));
        }

        // Chequea que no se deja marcar el medio boleto en un intervalo menor
        // a 5 minutos.
        [Test]
        public void IntervaloMedioBoletoTest()
        {
            tarjetaMedia.CargarAcreditar(5000);
            Boleto? boleto1 = colectivo.pagarCon(tarjetaMedia);
            Boleto? boleto2 = colectivo.pagarCon(tarjetaMedia);
            Assert.Null(boleto2);
        }

        // Chequea la cantidad válida de viajes con medio boleto por día.
        [Test]
        public void MedioBoletoCantidadTest()
        {
            tarjetaMedia.CargarAcreditar(2000);
            double cargaAntes = tarjetaMedia.Saldo;
            tarjetaMedia.BoletosSacadosHoy = (DateTime.Today, TarjetaFranquiciaMedia.MaximosBoletosPorDia);
            Boleto? boleto = colectivo.pagarCon(tarjetaMedia);
            double cargaDespues = tarjetaMedia.Saldo;
            Assert.NotNull(boleto);
            Assert.That(cargaAntes - cargaDespues, Is.EqualTo(colectivo.PrecioBoleto));
        }
    }
}

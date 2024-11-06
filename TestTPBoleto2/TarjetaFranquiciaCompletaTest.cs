using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TpBoleto2;

namespace TestTPBoleto2
{
    public class TarjetaFranquiciaCompletaTest
    {
        // Declaración de variables de tarjetas y colectivos.
        TarjetaFranquciaCompleta tarjetaCompleta;

        Colectivo colectivo;

        // Configuración de tarjetas y colectivos.
        [SetUp]
        public void Setup()
        {
            tarjetaCompleta = new TarjetaFranquciaCompleta(2);
            colectivo = new Colectivo("122 Roja");
        }

        // Chequea que se descuente el precio del boleto completo del saldo de la tarjeta completa. (franquicia completa)
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

        // Chequea que una vez superado el limite de boletos gratuitos de franquicia completa por día,
        // se cobre el monto completo.
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
            double saldoEsperado = saldoAntes - colectivo.PrecioBoleto;
            Assert.That(tarjetaCompleta.Saldo, Is.EqualTo(saldoEsperado));
        }
    }
}

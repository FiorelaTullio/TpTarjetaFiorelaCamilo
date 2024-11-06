using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TpBoleto2;

namespace TestTPBoleto2
{
    public class BoletoTest
    {

        // Declaración de variables de tarjetas y colectivos.
        Tarjeta tarjeta;
        TarjetaFranquciaCompleta tarjetaCompleta;
        TarjetaFranquiciaMedia tarjetaMedia;

        Colectivo colectivo;

        // Configuración de tarjetas y colectivos.
        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta(4);
            tarjetaCompleta = new TarjetaFranquciaCompleta(2);
            tarjetaMedia = new TpBoleto2.TarjetaFranquiciaMedia(3);
            colectivo = new Colectivo("122 Roja");
        }

        // Chequea que al pagar con una tarjeta de tipo normal (sin beneficios),
        // las características del boleto sean del mismo tipo (normal).
        [Test]
        public void probarBoletoNormal()
        {
            tarjeta.CargarAcreditar(3000);
            Boleto? boleto = colectivo.pagarCon(tarjeta);
            Assert.NotNull(boleto);
            Assert.That(boleto.SacadoCon, Is.EqualTo(tarjeta.GetType().Name));
        }

        // Chequea que al pagar con una tarjeta de tipo franquicia media,
        // las características del boleto sean del mismo tipo (medio boleto). 
        [Test]
        public void probarBoletoMedio()
        {
            tarjetaMedia.CargarAcreditar(3000);
            Boleto? boleto = colectivo.pagarCon(tarjetaMedia);
            Assert.NotNull(boleto);
            Assert.That(boleto.SacadoCon, Is.EqualTo(tarjetaMedia.GetType().Name));
        }

        // Chequea que al pagar con una tarjeta de tipo franquicia completa,
        // las características del boleto sean del mismo tipo (boleto completo).
        [Test]
        public void probarBoletoCompleto()
        {
            tarjetaCompleta.CargarAcreditar(3000);
            Boleto? boleto = colectivo.pagarCon(tarjetaCompleta);
            Assert.NotNull(boleto);
            Assert.That(boleto.SacadoCon, Is.EqualTo(tarjetaCompleta.GetType().Name));
        }
    }
}

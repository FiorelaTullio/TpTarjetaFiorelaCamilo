using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TpBoleto2;

namespace TestTPBoleto2
{
    public class ColectivoInterurbanoTest
    {
        // Declaración de variables de tarjetas y colectivos.
        Tarjeta tarjeta;
        TarjetaFranquciaCompleta tarjetaCompleta;
        TarjetaFranquiciaMedia tarjetaMedia;

        ColectivoInterurbano interurbano;

        // Configuración de tarjetas y colectivos.
        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta(4);
            tarjetaCompleta = new TarjetaFranquciaCompleta(2);
            tarjetaMedia = new TarjetaFranquiciaMedia(3);
            interurbano = new ColectivoInterurbano("146 Negro");
        }

        // Chequea que al pagar con una línea interurbana normal (sin beneficios),
        // el saldo descontado de la tarjeta sea el correcto. 
        [Test]
        [TestCase(3000)]
        [TestCase(4000)]
        [TestCase(5000)]
        [TestCase(6000)]
        public void InterurbanosTest(float saldo)
        {
            tarjeta.Saldo = saldo;
            interurbano.pagarCon(tarjeta);
            Assert.That(tarjeta.Saldo, Is.EqualTo(saldo - 2500f));
        }

        // Chequea que al pagar con una línea interurbana franquicia media,
        // el saldo descontado de la tarjeta sea el correcto. (medio boleto)
        [Test]
        [TestCase(3000)]
        [TestCase(4000)]
        [TestCase(5000)]
        [TestCase(6000)]
        public void InterurbanosMedioTest(float saldo)
        {
            tarjetaMedia.Saldo = saldo;
            interurbano.pagarCon(tarjetaMedia);
            Assert.That(tarjetaMedia.Saldo, Is.EqualTo(saldo - 1250f));
        }

        // Chequea que al pagar con una línea interurbana franquicia completa,
        // el saldo descontado de la tarjeta sea el correcto. (boleto con descuento completo)
        [Test]
        [TestCase(3000)]
        [TestCase(4000)]
        [TestCase(5000)]
        [TestCase(6000)]
        public void InterurbanosCompletoTest(float saldo)
        {
            tarjetaCompleta.Saldo = saldo;
            interurbano.pagarCon(tarjetaCompleta);
            Assert.That(tarjetaCompleta.Saldo, Is.EqualTo(saldo));
        }
    }
}

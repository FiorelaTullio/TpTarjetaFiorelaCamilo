using System.Security.Cryptography.X509Certificates;
using TpBoleto2;

namespace TestTPBoleto2
{
    public class TarjetaTest
    {
        // Declaración de variables de tarjetas y colectivos.
        Tarjeta tarjeta;
        TarjetaFranquciaCompleta tarjetaCompleta;
        TarjetaFranquiciaMedia tarjetaMedia;

        Colectivo colectivo;
        ColectivoInterurbano interurbano;

        // Configuración de tarjetas y colectivos.
        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta(4);
            tarjetaCompleta = new TarjetaFranquciaCompleta(2);
            tarjetaMedia = new TarjetaFranquiciaMedia(3);
            colectivo = new Colectivo("122 Roja");
            interurbano = new ColectivoInterurbano("146 Negro");
        }

        // Test para verificar la correcta acreditación de la carga de la tarjeta.
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
        // Pago con saldo en la tarjeta.
        [Test]
        [TestCase(1000)]
        [TestCase(1500)]
        public void PuedePagarTest(double cargaInicial)
        {
            tarjeta.Saldo = cargaInicial;
            Assert.NotNull(colectivo.pagarCon(tarjeta));
        }

        // Pago sin saldo en la tarjeta.
        [Test]
        [TestCase(0)]
        [TestCase(-500)]
        public void NoPuedePagar(double cargaInicial)
        {
            tarjeta.Saldo = cargaInicial;
            Assert.IsNull(colectivo.pagarCon(tarjeta));
        }

        // Chequea que se descuente el precio del boleto del saldo de la tarjeta normal (sin beneficios).
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
            Assert.That(tarjeta.Saldo, Is.EqualTo(saldoInicial - colectivo.PrecioBoleto));

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

        // Chequea que el saldo que exceda el monto máximo permitido, 
        // quede almacenado y pendiente de acreditación. 
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

        // Verifica si hay saldo pendiente de acreditación y lo carga
        // hasta el máximo permitido.
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

        // Chequea que se apliquen los descuentos del boleto de uso frecuente.
        [Test]
        public void DescuentosTest()
        {
            int iteraciones = 0;
            while(0 <= iteraciones && iteraciones <= 29)
            {
                tarjeta.Saldo = colectivo.PrecioBoleto;
                colectivo.pagarCon(tarjeta);
                Assert.That(tarjeta.Saldo, Is.EqualTo(0));
                iteraciones++;
            }

            while (30 <= iteraciones && iteraciones <= 79)
            {
                tarjeta.Saldo = colectivo.PrecioBoleto * 0.8;
                colectivo.pagarCon(tarjeta);
                Assert.That(tarjeta.Saldo, Is.EqualTo(0));
                iteraciones++;
            }
            while (80 <= iteraciones && iteraciones <= 100)
            {
                tarjeta.Saldo = colectivo.PrecioBoleto * 0.75;
                colectivo.pagarCon(tarjeta);
                Assert.That(tarjeta.Saldo, Is.EqualTo(0));
                iteraciones++;
            }
        }

        // Chequea que todas las franquicias medias y completas solo puedean 
        // utilizarse en la franja horaria permitida.
        [Test]
        public void LimitesFranquiciasTest()
        {
            TarjetaFranquiciaMedia.MaximosBoletosPorDia = int.MaxValue;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 24; j++)
                {
                    colectivo.reloj = new TiempoFalso(new DateTime(2024, 10, 21 + i, j, 0, 0));
                    tarjetaMedia.Saldo = colectivo.PrecioBoleto;
                    Boleto? boleto = colectivo.pagarCon(tarjetaMedia);
                    if (i < 5 && j >= 6 && j <= 22)
                    {
                        Assert.That(tarjetaMedia.Saldo, Is.EqualTo(colectivo.PrecioBoleto - colectivo.MedioPrecioBoleto));
                    } else
                    {
                        Assert.That(tarjetaMedia.Saldo, Is.EqualTo(0));
                    }
                }
            }
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

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
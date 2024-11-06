using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TpBoleto2;

namespace TestTPBoleto2
{
    public class TarjetaTest
    {
        // Declaración de variables de tarjetas y colectivos.
        Tarjeta tarjeta;

        Colectivo colectivo;

        // Configuración de tarjetas y colectivos.
        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta(4);
            colectivo = new Colectivo("122 Roja");
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
            while (tarjeta.pendienteDeAcreditacion == 0)
            {
                tarjeta.CargarAcreditar(2000);
            }

            double pendienteAntes = tarjeta.pendienteDeAcreditacion;
            colectivo.pagarCon(tarjeta);
            double pendienteDespues = tarjeta.pendienteDeAcreditacion;
            Assert.That(pendienteDespues, Is.LessThan(pendienteAntes));
        }

        [Test]
        public void Test()
        {
            Assert.Pass();
        }

    }
}

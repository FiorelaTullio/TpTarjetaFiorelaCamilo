using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TpBoleto2;

namespace TestTPBoleto2
{
    public class ColectivoTest
    {
        // Declaración de variables de tarjetas y colectivos.
        Tarjeta tarjeta;
        TarjetaFranquiciaMedia tarjetaMedia;

        Colectivo colectivo;

        // Configuración de tarjetas y colectivos.
        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta(4);
            tarjetaMedia = new TpBoleto2.TarjetaFranquiciaMedia(3);
            colectivo = new Colectivo("122 Roja");
        }

        // Chequea que se apliquen los descuentos del boleto de uso frecuente.
        [Test]
        public void DescuentosTest()
        {
            int iteraciones = 0;
            while (0 <= iteraciones && iteraciones <= 29)
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
                    }
                    else
                    {
                        Assert.That(tarjetaMedia.Saldo, Is.EqualTo(0));
                    }
                }
            }
        }
    }
}

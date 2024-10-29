

namespace TpBoleto2
{
    public class Colectivo
    {

        public static float PrecioBoleto = 1200f;
        public static float MedioPrecioBoleto
        {
            get { return Colectivo.PrecioBoleto / 2; }
        }

        public static (int, int, double)[] intervalosDescuentos = [(0, 29, 1), (30, 79, 0.8), (80, int.MaxValue, 0.75)];

        public string Linea { get; }
        
        public Colectivo(string linea) 
        {
            Linea = linea;
        }

        private bool puedeUsarFranquicia()
        {
            bool puedeDia = DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday;
            bool puedeHora = 6 <= DateTime.Now.Hour && DateTime.Now.Hour <= 22;
            return puedeDia && puedeHora;
        }

        private Boleto? cobrarTarjeta(Tarjeta tarjeta, double precio)
        {
            if (tarjeta.Cobrar(precio))
            {
                return new Boleto(tarjeta, this, precio);
            } else
            {
                return null;
            }
        }

        private Boleto? pagarConFranquiciaCompleta(TarjetaFranquciaCompleta tarjeta)
        {
            if (!puedeUsarFranquicia())
            {
                return cobrarTarjeta(tarjeta, PrecioBoleto);
            }
            if (tarjeta.ultimoBoleto == DateTime.Today)
            {
                if (tarjeta.cantidadBoletosSacados < TarjetaFranquciaCompleta.MaxBoletosPorDia)
                {
                    tarjeta.cantidadBoletosSacados++;
                    return cobrarTarjeta(tarjeta, 0);
                }
                else
                {
                    tarjeta.cantidadBoletosSacados++;
                    return cobrarTarjeta(tarjeta, PrecioBoleto);
                }
            }
            else
            {
                tarjeta.ultimoBoleto = DateTime.Today;
                tarjeta.cantidadBoletosSacados = 1;
                return cobrarTarjeta(tarjeta, 0);
            }
        }

        private Boleto? pagarConFranquiciaMedia(TarjetaFranquiciaMedia tarjeta)
        {
            if (!puedeUsarFranquicia())
            {
                return cobrarTarjeta(tarjeta, PrecioBoleto);
            }
            (DateTime dia, int veces) = tarjeta.BoletosSacadosHoy;
            if (dia.Date != DateTime.Now.Date)
            {
                tarjeta.BoletosSacadosHoy = (DateTime.Now, 1);
                return cobrarTarjeta(tarjeta, MedioPrecioBoleto);
            } else
            {
                if (veces >= TarjetaFranquiciaMedia.MaximosBoletosPorDia)
                {
                    tarjeta.BoletosSacadosHoy = (DateTime.Now, veces + 1);
                    return cobrarTarjeta(tarjeta, PrecioBoleto);
                } else
                {
                    TimeSpan span = DateTime.Now - dia;
                    if (span.Minutes > TarjetaFranquiciaMedia.MinutosEntreBoletos)
                    {
                        tarjeta.BoletosSacadosHoy = (DateTime.Now, veces + 1);
                        return cobrarTarjeta(tarjeta, MedioPrecioBoleto);
                    } else
                    {
                        return null;
                    }
                    
                }
            }
        }  
        private Boleto? pagarConTarjetaNormal(Tarjeta tarjeta)
        {
            if (tarjeta.MesActual != DateTime.Today.Month)
            {
                tarjeta.MesActual = DateTime.Today.Month;
                tarjeta.UsosEsteMes = 0;
            }

            foreach((int desde, int hasta, double descuento) in intervalosDescuentos)
            {
                if (desde <= tarjeta.UsosEsteMes && tarjeta.UsosEsteMes <= hasta)
                {
                    tarjeta.UsosEsteMes++;
                    return cobrarTarjeta(tarjeta,  descuento * PrecioBoleto);
                }
            }
            return null;
        }

        public Boleto? pagarCon (Tarjeta tarjeta)
        {
            Boleto? boleto = tarjeta.GetType().Name switch
            {
                nameof(TarjetaFranquciaCompleta) => pagarConFranquiciaCompleta((TarjetaFranquciaCompleta)tarjeta),
                nameof(TarjetaFranquiciaMedia) => pagarConFranquiciaMedia((TarjetaFranquiciaMedia)tarjeta),
                _ => pagarConTarjetaNormal(tarjeta),
            };
            tarjeta.Acreditar();
            return boleto;
        }
    }
}
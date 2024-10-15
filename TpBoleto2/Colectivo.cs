

namespace TpBoleto2
{
    public class Colectivo
    {

        public string Linea { get; }
        
        public Colectivo(string linea) 
        {
            Linea = linea;
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
            return cobrarTarjeta(tarjeta, 0);
        }

        private Boleto? pagarConFranquiciaMedia(TarjetaFranquiciaMedia tarjeta)
        {
            (DateTime dia, int veces) = tarjeta.BoletosSacadosHoy;
            if (dia.Date != DateTime.Now.Date)
            {
                tarjeta.BoletosSacadosHoy = (DateTime.Now, 1);
                return cobrarTarjeta(tarjeta, Boleto.MedioBoleto);
            } else
            {
                if (veces >= TarjetaFranquiciaMedia.MaximosBoletosPorDia)
                {
                    tarjeta.BoletosSacadosHoy = (DateTime.Now, veces + 1);
                    return cobrarTarjeta(tarjeta, Boleto.Precio);
                } else
                {
                    TimeSpan span = DateTime.Now - dia;
                    if (span.Minutes > TarjetaFranquiciaMedia.MinutosEntreBoletos)
                    {
                        tarjeta.BoletosSacadosHoy = (DateTime.Now, veces + 1);
                        return cobrarTarjeta(tarjeta, Boleto.MedioBoleto);
                    } else
                    {
                        return null;
                    }
                    
                }
            }
        }

        private Boleto? pagarConTarjetaNormal(Tarjeta tarjeta)
        {
            return cobrarTarjeta(tarjeta, Boleto.Precio);
        }

        public Boleto? pagarCon (Tarjeta tarjeta)
        {
            switch (tarjeta.GetType().Name)
            {
                case nameof(Tarjeta):
                    return pagarConTarjetaNormal(tarjeta);
                case nameof(TarjetaFranquciaCompleta):
                    return pagarConFranquiciaCompleta((TarjetaFranquciaCompleta)tarjeta);
                case nameof(TarjetaFranquiciaMedia):
                    return pagarConFranquiciaMedia((TarjetaFranquiciaMedia)tarjeta);
                default:
                    return null;
            }
        }
    }
}


namespace TpBoleto2
{
    public class Colectivo
    {

        private Boleto cobrarTarjeta(Tarjeta tarjeta, double precio)
        {
            if (tarjeta.Cobrar(precio))
            {
                return new Boleto();
            } else
            {
                return null;
            }
        }

        private Boleto pagarConFranquiciaCompleta(TarjetaFranquciaCompleta tarjeta)
        {
            return new Boleto();
        }

        private Boleto pagarConFranquiciaMedia(TarjetaFranquiciaMedia tarjeta)
        {
            (DateTime dia, int veces) = tarjeta.boletosSacadosHoy;
            if (dia.Date != DateTime.Now.Date)
            {
                tarjeta.boletosSacadosHoy = (DateTime.Now, 1);
                return cobrarTarjeta(tarjeta, Boleto.MedioBoleto);
            } else
            {
                if (veces >= TarjetaFranquiciaMedia.MaximosBoletosPorDia)
                {
                    tarjeta.boletosSacadosHoy = (DateTime.Now, veces + 1);
                    return cobrarTarjeta(tarjeta, Boleto.Precio);
                } else
                {
                    TimeSpan span = DateTime.Now - dia;
                    if (span.Minutes < TarjetaFranquiciaMedia.MinutosEntreBoletos)
                    {
                        tarjeta.boletosSacadosHoy = (DateTime.Now, veces + 1);
                        return cobrarTarjeta(tarjeta, Boleto.MedioBoleto);
                    } else
                    {
                        return null;
                    }
                    
                }
            }
        }

        private Boleto pagarConTarjetaNormal(Tarjeta tarjeta)
        {
            return cobrarTarjeta(tarjeta, Boleto.Precio);
        }

        public Boleto pagarCon (Tarjeta tarjeta)
        {
            switch (tarjeta.GetType())
            {
                case Tarjeta:
                    return pagarConTarjetaNormal(tarjeta);
                case TarjetaFranquciaCompleta:
                    return pagarConFranquiciaCompleta(tarjeta);
                case TarjetaFranquiciaMedia:
                    return pagarConFranquiciaCompleta(tarjeta);
            }
        }
    }
}
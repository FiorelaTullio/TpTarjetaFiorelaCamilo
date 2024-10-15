

namespace TpBoleto2
{
    public class Colectivo
    {

        private Boleto? cobrarTarjeta(Tarjeta tarjeta, double precio)
        {
            if (tarjeta.Cobrar(precio))
            {
                return new Boleto();
            }
            else
            {
                return null;
            }
        }

        private Boleto? pagarConFranquiciaCompleta(TarjetaFranquciaCompleta tarjeta)
        {
            if (tarjeta.ultimoBoleto == DateTime.Today)
            {
                if (tarjeta.cantidadBoletosSacados < TarjetaFranquciaCompleta.MaxBoletosPorDia)
                {
                    tarjeta.cantidadBoletosSacados++;
                    return cobrarTarjeta(tarjeta, 0);
                } else
                {
                    tarjeta.cantidadBoletosSacados++;
                    return cobrarTarjeta(tarjeta, Boleto.Precio);
                }
            } else
            {
                tarjeta.ultimoBoleto = DateTime.Today;
                tarjeta.cantidadBoletosSacados = 1;
                return cobrarTarjeta(tarjeta, 0);
            }
        }

        public Boleto? pagarConFranquiciaMedia(TarjetaFranquiciaMedia tarjeta)
        {
            return cobrarTarjeta(tarjeta, Boleto.MedioBoleto);
        }

        private Boleto? pagarConTarjetaNormal(Tarjeta tarjeta)
        {
            return cobrarTarjeta(tarjeta, Boleto.Precio);
        }

        public Boleto? pagarCon(Tarjeta tarjeta)
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
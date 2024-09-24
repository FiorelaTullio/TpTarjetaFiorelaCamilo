

namespace TpBoleto2
{
    public class Colectivo
    {

        public Boleto pagarCon (Tarjeta tarjeta)
        {
            if (tarjeta.Cobrar(Boleto.Precio))
            {
                return new Boleto();
            }
            else
            {
                return null;
            }
        }
    }
}
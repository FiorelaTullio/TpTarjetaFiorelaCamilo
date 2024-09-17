

namespace TpBoleto2
{
    public class Colectivo
    {

        public Boleto pagarCon (Tarjeta tarjeta)
        {
            if (tarjeta.Saldo < Boleto.Precio)
            {
                return null;
            }
            tarjeta.Saldo -= Boleto.Precio;
            return new Boleto();
        }
    }
}
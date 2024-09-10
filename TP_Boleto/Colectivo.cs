

namespace TP_Boleto
{
    public class Colectivo
    {

        public Boleto pagarCon (Tarjeta tarjeta)
        {
            if (tarjeta.Saldo < Boleto.Precio)
            {
                throw new Exception("Saldo Insuficiente");
            }
            tarjeta.Saldo -= Boleto.Precio;
            return new Boleto();
        }
    }
}


namespace TPColectivos
{
    public class Colectivo
    {

        public Boleto pagarCon (Tarjeta tarjeta)
        {
            if (tarjeta.saldo < Boleto.Precio)
            {
                throw new Exception("Saldo Insuficiente");
            }
            tarjeta.saldo -= Boleto.Precio;
            return new Boleto();
        }
    }
}
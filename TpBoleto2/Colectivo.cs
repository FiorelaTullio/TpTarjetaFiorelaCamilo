

namespace TpBoleto2
{
    public class Colectivo
    {

        public string Linea { get; }
        
        public Colectivo(string linea) 
        {
            Linea = linea;
        }

        public Boleto? pagarCon (Tarjeta tarjeta)
        {
            if (tarjeta.Cobrar(Boleto.Precio, out double cobrado))
            {
                return new Boleto(tarjeta, this, cobrado);
            }
            else
            {
                return null;
            }
        }
    }
}
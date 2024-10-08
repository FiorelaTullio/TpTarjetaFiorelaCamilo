using System;
using System.Runtime.CompilerServices;


namespace TpBoleto2
{
    public class Boleto
    {
        public static float Precio = 940f;
        public static float MedioBoleto
        {
            get { return Precio / 2; }
        }
    }
}

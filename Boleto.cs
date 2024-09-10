using System;


namespace TPColectivos
{
    public class Boleto
    {

        static const float Precio = 940f;

        public DateTime FechaEmision;

        public Boleto()
        {
            FechaEmision = DateTime.Now;
        }
    }
}




namespace TPColectivos
{
    public class Tarjeta
    {
        static const double[] CargasValidas = { 2000f, 3000f, 4000f, 5000f, 6000f, 7000f, 8000f, 9000f };
        static const double SaldoMaximo = 9900f;
        public double Saldo { set; get; };

        public Tarjeta()
        {
            this.Saldo = 0;
        }

        public bool Cargar(double carga)
        {
            if (CargasValidas.Contains(carga))
            {
                if (this.Saldo + carga <= SaldoMaximo)
                {
                    this.Saldo += carga;
                    return true;
                }
            }
            return false;
        }

        public bool Cobrar(double cos) { }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace TpBoleto2
{

    public class Tarjeta
    {
        public static double[] CargasValidas = [2000,3000,4000,5000,6000,7000,8000,9000];
        const double SaldoMaximo = 9900f;
        public double Saldo { set; get; }

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
    }
}
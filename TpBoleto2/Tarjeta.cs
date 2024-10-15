using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace TpBoleto2
{

    public class Tarjeta
    {
        public static double[] CargasValidas = [2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000];
        public static double MaxSaldoNegativo = -480f; 
        const double SaldoMaximo = 9900f;
        protected double saldo;

        public double Saldo
        {
            get { return saldo + MaxSaldoNegativo; }
            set { saldo = value - MaxSaldoNegativo; }
        }

        public Tarjeta()
        {
            this.saldo = 0;
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

        // Especial para viajes plus
        public bool Cobrar(double precio)
        {
            if (saldo < precio)
            {
                return false;
            }
            saldo -= precio;
            return true;
        }
    }
}
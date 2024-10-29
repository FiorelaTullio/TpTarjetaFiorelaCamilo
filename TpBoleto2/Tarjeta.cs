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
        public static double MaxSaldoNegativo = -600f;
        public const double SaldoMaximo = 36000f;
        protected double saldo;
        public double pendienteDeAcreditacion { get; private set; } = 0;
        public int ID;
        public bool CargoPorEncimaDeNegativo = false;
        public int MesActual = -1;
        public int UsosEsteMes = 0;
     

        public double Saldo
        {
            get { return saldo + MaxSaldoNegativo; }
            set { saldo = value - MaxSaldoNegativo; }

        }

        public Tarjeta(int id)
        {
            this.saldo = 0;
            this.ID = id;
        }

        public bool CargarAcreditar(double carga)
        {
            bool puedeCargar = CargasValidas.Contains(carga);
            if (puedeCargar)
            {
                this.pendienteDeAcreditacion += carga;
            }
            this.Acreditar();
            return puedeCargar;
        }

        public void Acreditar()
        {
            if (this.Saldo + this.pendienteDeAcreditacion > SaldoMaximo)
            {
                this.pendienteDeAcreditacion -= SaldoMaximo - this.Saldo;
                this.Saldo = SaldoMaximo;
            }
            else
            {
                this.Saldo += this.pendienteDeAcreditacion;
                this.pendienteDeAcreditacion = 0;
            }
        }

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
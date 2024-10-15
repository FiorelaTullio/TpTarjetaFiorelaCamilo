using System;
using System.Runtime.CompilerServices;


namespace TpBoleto2
{
    public class Boleto
    {
        public static float Precio = 940f;

        public DateTime Fecha { get; }
        public string SacadoCon {  get; }
        public string LineaColectivo { get; }
        public double Cobrado { get; }
        public int TarjetaID { get; }
        public double SaldoRestante { get; }
        public string? Nota { get; }

        public Boleto(Tarjeta sacadoCon, Colectivo sacadoEn, double cobrado)
        {
            Fecha = DateTime.Today;
            SacadoCon = sacadoCon.GetType().Name;
            LineaColectivo = sacadoEn.Linea;
            Cobrado = cobrado;
            TarjetaID = sacadoCon.ID;
            SaldoRestante = sacadoCon.Saldo;
            if (sacadoCon.CargoPorEncimaDeNegativo)
            {
                Nota = "Su saldo ya no está en negativo";
                sacadoCon.CargoPorEncimaDeNegativo = false;
            } else
            {
                Nota = "";
            }
        }


    }

}

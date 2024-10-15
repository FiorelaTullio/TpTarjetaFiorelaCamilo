using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpBoleto2
{
    public class TarjetaFranquciaCompleta : Tarjeta
    {
        public int cantidadBoletosSacados = 0;
        public DateTime ultimoBoleto = DateTime.MinValue;
        public static int MaxBoletosPorDia = 2;
        public TarjetaFranquciaCompleta(int id) : base(id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpBoleto2
{
    public class TarjetaFranquiciaMedia : Tarjeta
    {
        public (DateTime, int) BoletosSacadosHoy = (DateTime.MinValue, 0);
        public static int MaximosBoletosPorDia = 4;
        public static int MinutosEntreBoletos = 5;
        public TarjetaFranquiciaMedia(int id) : base(id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpBoleto2
{
    public class TarjetaFranquciaCompleta : Tarjeta
    {

        public TarjetaFranquciaCompleta(int id) : base(id)
        {
        }

        public override bool Cobrar(double precio, out double cobrado)
        {
            cobrado = 0.0;
            return true;
        }
    }
}

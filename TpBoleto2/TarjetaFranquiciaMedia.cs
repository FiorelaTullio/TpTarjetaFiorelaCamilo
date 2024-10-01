using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpBoleto2
{
    public class TarjetaFranquiciaMedia : Tarjeta
    {
        public TarjetaFranquiciaMedia(int id) : base(id)
        {
        }

        public override bool Cobrar(double precio, out double cobrado)
        {
            if (saldo < precio / 2)
            {
                cobrado = 0.0;
                return false;
            }
            saldo -= precio / 2;
            cobrado = precio / 2;
            return true;
        }
    }
}

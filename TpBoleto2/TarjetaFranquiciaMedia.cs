using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpBoleto2
{
    public class TarjetaFranquiciaMedia : Tarjeta
    {

        public override bool Cobrar(double precio)
        {
            if (saldo < precio / 2)
            {
                return false;
            }
            saldo -= precio / 2;
            return true;
        }
    }
}

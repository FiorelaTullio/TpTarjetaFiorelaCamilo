﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpBoleto2
{
    public class TarjetaFranquciaCompleta : Tarjeta
    {
        public override bool Cobrar(double precio)
        {
            return true;
        }
    }
}

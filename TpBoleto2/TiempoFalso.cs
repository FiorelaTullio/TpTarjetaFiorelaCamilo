using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpBoleto2
{
    public class TiempoFalso : Tiempo
    {
        private DateTime fijo;

        public TiempoFalso(DateTime fijar)
        {
            fijo = fijar;
        }

        public override DateTime Now()
        {
            return fijo;
        }

        public override DateTime Today()
        {
            return fijo.Date;
        }

        public void AgregarHoras(int i)
        {
            fijo = fijo.AddHours(i);
        }

        public void AgregarDias(int i)
        {
            fijo = fijo.AddDays(i);
        }

    }
}

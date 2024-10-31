using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpBoleto2
{
    public class ColectivoInterurbano : Colectivo
    {
        public override float PrecioBoleto {
            get { return 2500f; }
        }

        public ColectivoInterurbano(string linea) : base(linea) { }
    }
}

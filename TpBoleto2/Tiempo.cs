using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpBoleto2
{
    public class Tiempo
    {
        public Tiempo() { }

        public virtual DateTime Now()
        {
            return DateTime.Now;
        } 

        public virtual DateTime Today()
        {
            return DateTime.Today;
        }
    }
}

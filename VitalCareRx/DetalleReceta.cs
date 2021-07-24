using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitalCareRx
{
    class DetalleReceta
    {
        public int IdReceta { get; set; }

        public int IdFarmaco { get; set; }

        public string Farmaco { get; set; }

        public int Cantidad { get; set; }

        public string DuracionTratamiento { get; set; }

        public string Indicaciones { get; set; }


    }
}

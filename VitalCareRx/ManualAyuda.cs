using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitalCareRx
{
    class ManualAyuda
    {
        public void LlamarManualUsuario(string ubicacion)
        {
            ManualUsuario manualUsuario = new ManualUsuario(ubicacion);
            manualUsuario.ShowDialog();
        }
    }
}

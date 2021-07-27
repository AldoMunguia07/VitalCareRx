using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VitalCareRx
{
    public partial class ManualUsuario : Form
    {
        public ManualUsuario(string ubicacion)
        {
            InitializeComponent();

      

            string ruta = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            

            web.Navigate($@"{ruta}\Omega Soft Evolution\Vital Care Rx\{ubicacion}");
        }
    }
}

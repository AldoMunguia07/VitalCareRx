using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VitalCareRx
{
    public partial class ManualUsuario : Form
    {
        public ManualUsuario()
        {
            InitializeComponent();

            string baseDir = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.StartupPath);
            MessageBox.Show(baseDir);
            string ruta = baseDir.Substring(0, baseDir.Length - 3);
            MessageBox.Show(ruta);





            web.Navigate($@"{ruta}Manual de usuario\Pacientes\OutDocument.htm");
        }
    }
}

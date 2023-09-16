using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Validaciones
{
    public class Validacion
    {
        public void CargarImagen(PictureBox pbCatalogo, string imagen)
        {
            try
            {
                pbCatalogo.Load(imagen);
            }
            catch (Exception)
            {
                pbCatalogo.Load("https://img.freepik.com/vector-premium/vector-icono-imagen-predeterminado-pagina-imagen-faltante-diseno-sitio-web-o-aplicacion-movil-no-hay-foto-disponible_87543-11093.jpg");
            }
        }
        public bool ValidarNumero(string cad)
        {
            string patron = @"^[0-9]+(\.[0-9]+)?$";
            return Regex.IsMatch(cad, patron);
        }
    }

}

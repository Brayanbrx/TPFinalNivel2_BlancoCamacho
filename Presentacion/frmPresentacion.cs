using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;
using Dominio;

namespace Presentacion
{
    public partial class frmPresentacion : Form
    {
        private List<Articulo> listaArticulo;
        public frmPresentacion()
        {
            InitializeComponent();
        }

        private void frmPresentacion_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {
                Articulo seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);   
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                dgvCatalogo.DataSource = listaArticulo;
                cargarImagen(listaArticulo[0].ImagenUrl);
                //ocultarColumnas();
                //cargarImagen(listaPokemon[0].UrlImagen);
                dgvCatalogo.Columns["ImagenUrl"].Visible = false;
                dgvCatalogo.Columns["Id"].Visible = false;
                cargarImagen(listaArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbCatalogo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbCatalogo.Load("https://img.freepik.com/vector-premium/vector-icono-imagen-predeterminado-pagina-imagen-faltante-diseno-sitio-web-o-aplicacion-movil-no-hay-foto-disponible_87543-11093.jpg");
               //throw ex;
            }
            
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();
        }
    }
}

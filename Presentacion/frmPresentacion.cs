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
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                dgvCatalogo.DataSource = listaArticulo;
                cargarImagen(listaArticulo[0].ImagenUrl);
                //ocultarColumnas();
                //cargarImagen(listaPokemon[0].UrlImagen);
                dgvCatalogo.Columns["ImagenUrl"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {
                Articulo seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);   
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbCatalogo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbCatalogo.Load("https://www.google.com/url?sa=i&url=http%3A%2F%2Fwww.colombianosune.com%2Fejes%2Fplancomunidad%2Fasociaciones%2Fasociacion-artistica-y-cultural-thakhi-runa&psig=AOvVaw3GYWZ186q3Z_wC7nWgRX1I&ust=1694624314199000&source=images&cd=vfe&opi=89978449&ved=0CBAQjRxqFwoTCJjRwdnFpYEDFQAAAAAdAAAAABAE");
               throw ex;
            }
            
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

                throw ex;
            }
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
        }
    }
}

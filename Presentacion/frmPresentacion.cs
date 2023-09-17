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
using System.Text.RegularExpressions;
using Validaciones;

namespace Presentacion
{
    public partial class frmPresentacion : Form
    {
        private List<Articulo> listaArticulo;
        private Validacion validar = new Validacion();
        public frmPresentacion()
        {
            InitializeComponent();
        }
        private void frmPresentacion_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Codigo");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripcion");
            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Marca");
        }
        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvCatalogo.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                validar.CargarImagen(pbCatalogo, seleccionado.ImagenUrl);

                lbDetalles.Text = "Codigo: "+seleccionado.Codigo.ToString()+"\r\n" +
                    "Nombre: " +seleccionado.Nombre.ToString() +"\r\n" +
                    "Descripción: "+seleccionado.Descripcion.ToString() +"\r\n" +
                    "Precio: " + seleccionado.Precio.ToString() +"\r\n" +
                    "Categoria: " + seleccionado.Categoria.ToString() +"\r\n" +
                    "Marca: " + seleccionado.Marca.ToString() +"\r\n";
            }           
        }
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                dgvCatalogo.DataSource = listaArticulo;
                ocultarColumnas();
                validar.CargarImagen(pbCatalogo, listaArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ocultarColumnas()
        {
            dgvCatalogo.Columns["ImagenUrl"].Visible = false;
            dgvCatalogo.Columns["Id"].Visible = false;
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            cargar();
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow != null && dgvCatalogo.CurrentRow.DataBoundItem != null)
            {
                Articulo seleccionado;
                seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
                modificar.ShowDialog();
                cargar();
            } else 
            {
                MessageBox.Show("No se ha seleccionado ningún artículo para modificar.");
            }
        }
        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }
        private void bntEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }
        private void eliminar (bool logico = false)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            if (dgvCatalogo.CurrentRow == null || dgvCatalogo.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show("Por favor, selecciona un artículo para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DialogResult respuesta = MessageBox.Show("De verdad queres eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                    if (logico)
                        negocio.eliminarLogico(seleccionado.Id);
                    else
                         negocio.eliminar(seleccionado.Id);
                    cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;
            if (filtro.Length>=2)
            {
                listaFiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Codigo.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulo;
            }
            dgvCatalogo.DataSource = null;
            dgvCatalogo.DataSource = listaFiltrada;
            ocultarColumnas();
        }
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                //if (cboCampo.SelectedItem != null && cboCriterio.SelectedItem != null)
                //{
                    string campo = cboCampo.SelectedItem.ToString();
                    string criterio = cboCriterio.SelectedItem.ToString();
                    string filtro = txtFiltroAvanzado.Text;
                    dgvCatalogo.DataSource = negocio.filtrar(campo, criterio, filtro);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            } 
        }
        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            } else if (opcion == "Categoria")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Celulares");
                cboCriterio.Items.Add("Televisores");
                cboCriterio.Items.Add("Media");
                cboCriterio.Items.Add("Audio");
            } else if (opcion == "Marca")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Samsung");
                cboCriterio.Items.Add("Apple");
                cboCriterio.Items.Add("Sony");
                cboCriterio.Items.Add("Huawei");
                cboCriterio.Items.Add("Motorola");
            }
            else{
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
            btnFiltro.Enabled = false;
            if(opcion == "Categoria" || opcion == "Marca")
                txtFiltroAvanzado.Enabled = false;
            else
                txtFiltroAvanzado.Enabled = true;
        }
        private void cboCriterio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCriterio.SelectedItem != null)
                btnFiltro.Enabled = true;   
        }
    }
}

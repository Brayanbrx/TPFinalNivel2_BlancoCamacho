using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data.SqlClient;
using System.Xml.Linq;
using AccesoDatos;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();


            Datos datos = new Datos();
            try
            {
                datos.setConsulta("Select Codigo, Nombre, A.Descripcion, ImagenUrl, C.DESCRIPCION Categoria, M.Descripcion Marca from ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.idCategoria AND M.Id = A.IdMarca");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                   /* Categoria aux = new Categoria();
                    aux.Id = (int)datos.Lector["iD"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    lista.Add(aux);*/

                    Articulo aux = new Articulo();
                    //aux.Id = (int)lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    //aux.Categoria.Id = (int)lector["IdCategoria"];
                    //aux.Categoria.Descripcion = (string)lector["Categoria"];
                    //aux.Categoria.Id = (int)lector["IdCategoria"]; //se lee el Id Tipo
                    //aux.Categoria.Descripcion = (string)lector["Categoria"];

                    aux.Marca = new Marca();
                    //aux.Marca.Id = (int)lector["IdMarca"]; //Se lee el Id Debilidad
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public void agregar(Articulo nuevo)
        {

        }

        public void modificar(Articulo modificar)
        {

        }
    }
}

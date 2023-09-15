using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data.SqlClient;
using System.Xml.Linq;
using AccesoDatos;
using System.Net;

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
                datos.setConsulta("Select Codigo, Nombre, A.Descripcion, ImagenUrl, C.DESCRIPCION Categoria, M.Descripcion Marca, A.IdCategoria, A.IdMarca, A.Id  from ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.idCategoria AND M.Id = A.IdMarca");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                   /* Categoria aux = new Categoria();
                    aux.Id = (int)datos.Lector["iD"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    lista.Add(aux);*/

                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    // if(!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl"))))
                    //    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["idCategoria"]; 
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    //aux.Categoria.Id = (int)lector["IdCategoria"];
                    //aux.Categoria.Descripcion = (string)lector["Categoria"];
                    //aux.Categoria.Id = (int)lector["IdCategoria"]; //se lee el Id Tipo
                    //aux.Categoria.Descripcion = (string)lector["Categoria"];

                    aux.Marca = new Marca();
                    //aux.Marca.Id = (int)lector["IdMarca"]; //Se lee el Id Debilidad
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
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
            Datos datos = new Datos();

            try
            {
                //arreglar en set los parametros
                datos.setConsulta("Insert into articulos (Codigo, Nombre, Descripcion, idCategoria, idMarca, ImagenUrl) values ('"+ nuevo.Codigo+"', '" +nuevo.Nombre+"', '"+nuevo.Descripcion+"', @idCategoria, @idMarca, @ImagenUrl)");
                datos.setParametro("@idCategoria", nuevo.Categoria.Id);
                datos.setParametro("@idMarca", nuevo.Marca.Id);
                datos.setParametro("ImagenUrl", nuevo.ImagenUrl);

                datos.ejecutarAccion();
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

        public void modificar(Articulo art)
        {
            Datos datos = new Datos(); //hacerlo como un atributo privado para no llamarlo nuevamente
            try
            {
                datos.setConsulta("update articulos set Codigo = @cod, Nombre = @nombre, Descripcion = @desc, ImagenUrl = @imagenUrl, IdCategoria = @idCat, idMarca = @idMarca where Id = @Id");
                datos.setParametro("@cod", art.Codigo);
                datos.setParametro("@nombre", art.Nombre);
                datos.setParametro("@desc", art.Descripcion);
                datos.setParametro("@imagenUrl", art.ImagenUrl);
                datos.setParametro("@idCat", art.Categoria.Id);
                datos.setParametro("@idMarca", art.Marca.Id);
                datos.setParametro("@Id", art.Id);

                datos.ejecutarAccion();
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
    }
}

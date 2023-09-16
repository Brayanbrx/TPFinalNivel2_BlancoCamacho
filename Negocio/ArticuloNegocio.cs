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
using System.Configuration;
using System.Text.RegularExpressions;

namespace Negocio
{
    public class ArticuloNegocio
    {
        private Datos datos = new Datos();
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            try
            {
                datos.setConsulta("Select Codigo, Nombre, A.Descripcion, ImagenUrl, C.DESCRIPCION Categoria, M.Descripcion Marca,Precio, A.IdCategoria, A.IdMarca, A.Id  from ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.idCategoria AND M.Id = A.IdMarca And Precio > 0");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["idCategoria"]; 
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Precio = Convert.ToDouble(datos.Lector["Precio"]);
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
        public void agregar(Articulo art)
        {
            try
            {
                datos.setConsulta("Insert into articulos (Codigo, Nombre, Descripcion, idCategoria, idMarca, ImagenUrl, Precio) values (@Codigo, @Nombre, @Descripcion, @idCategoria, @idMarca, @ImagenUrl, @Precio)");
                datos.setParametro("@Codigo", art.Codigo);
                datos.setParametro("@Nombre", art.Nombre);
                datos.setParametro("@Descripcion", art.Descripcion);
                datos.setParametro("@idCategoria", art.Categoria.Id);
                datos.setParametro("@idMarca", art.Marca.Id);
                datos.setParametro("ImagenUrl", art.ImagenUrl);
                datos.setParametro("@Precio", art.Precio);
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
            try
            {
                datos.setConsulta("update articulos set Codigo = @cod, Nombre = @nombre, Descripcion = @desc, ImagenUrl = @imagenUrl, IdCategoria = @idCat, idMarca = @idMarca, Precio = @Precio where Id = @Id");
                datos.setParametro("@cod", art.Codigo);
                datos.setParametro("@nombre", art.Nombre);
                datos.setParametro("@desc", art.Descripcion);
                datos.setParametro("@imagenUrl", art.ImagenUrl);
                datos.setParametro("@idCat", art.Categoria.Id);
                datos.setParametro("@idMarca", art.Marca.Id);
                datos.setParametro("@Precio", art.Precio);
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
        public void eliminar(int id)
        {
            try
            {
                datos.setConsulta("delete articulos where id = @Id");
                datos.setParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void eliminarLogico(int id)
        {
            try
            {
                datos.setConsulta("update articulos set Precio = 0 where id = @Id");
                datos.setParametro("@Id", id);
                datos.ejecutarAccion() ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            try
             {
                 string consulta = "Select Codigo, Nombre, A.Descripcion, ImagenUrl, C.DESCRIPCION Categoria, M.Descripcion Marca,Precio, A.IdCategoria, A.IdMarca, A.Id  from ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.idCategoria AND M.Id = A.IdMarca And Precio > 0 And ";
                if (campo == "Precio" && ValidarNumero(filtro))
                {
                        switch (criterio)
                        {
                            case "Mayor a":
                                consulta += "Precio > " + filtro;
                                break;
                            case "Menor a":
                                consulta += "Precio < " + filtro;
                                break;
                            default:
                                consulta += "Precio = " + filtro;
                                break;
                        }
                }
                else if (campo == "Nombre")
                {
                   switch (criterio)
                   {
                       case "Comienza con":
                           consulta += "Nombre like '" + filtro + "%' ";
                           break;
                       case "Termina con":
                           consulta += "Nombre like '%" + filtro +"'";
                           break;
                       default:
                           consulta += "Nombre like '%"+filtro+"%'";
                           break;
                   }
               } else if (campo == "Codigo")
               {
                   switch (criterio)
                   {
                       case "Comienza con":
                           consulta += "Codigo like '" + filtro + "%' ";
                           break;
                       case "Termina con":
                           consulta += "Codigo like '%" + filtro + "'";
                           break;
                       default:
                           consulta += "Codigo like '%" + filtro + "%'";
                           break;
                   }
               }
               else
               {
                   switch (criterio)
                   {
                       case "Comienza con":
                           consulta += "A.Descripcion like '" + filtro + "%' ";
                           break;
                       case "Termina con":
                           consulta += "A.Descripcion like '%" + filtro + "'";
                           break;
                       default:
                           consulta += "A.Descripcion like '%" + filtro + "%'";
                           break;
                   }
               }
                datos.setConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["idCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Precio = Convert.ToDouble(datos.Lector["Precio"]);
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex ;
            }
        }

        private bool ValidarNumero(string cad)
        {
            string patron = @"^[0-9]+(\.[0-9]+)?$";
            return Regex.IsMatch(cad, patron);
        }
    }
}

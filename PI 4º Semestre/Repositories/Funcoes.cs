using AdminLar.Models;
using PI_4º_Semestre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace AdminLar.Repositories
{
    public class Funcoes
    {

        public static Usuario AutenciaUsuario(string login, string senha)
        {

            Context _db = new Context();

            // query validar autenticidade no login

            var query = (from u in _db.Usuarios
                         where u.Email == login && u.Senha == senha
                         select u).SingleOrDefault();


            if (query == null)
            {

                return null;
            }


            FormsAuthentication.SetAuthCookie(query.Email, false);

            HttpContext.Current.Session["Usuario"] = query.Email;

            return query;

        }

        //conta condominos notificações 

        public static void ContCondomino(string valor)
        {
            HttpContext.Current.Session["ContCondomino"] = valor;
        }


        public static string GetContCondomino()
        {

            if (HttpContext.Current.Session["ContCondomino"] != null)
                return HttpContext.Current.Session["ContCondomino"].ToString();
            else
                return ""; 
        }


        //Conta encomenda notificações

        public static void ContEncomenda(string valor)
        {
            HttpContext.Current.Session["ContEncomenda"] = valor;
        }


        public static string GetContEncomenda()
        {

            if (HttpContext.Current.Session["ContEncomenda"] != null)
                return HttpContext.Current.Session["ContEncomenda"].ToString();
            else
                return "";
        }

        //Conta as inadimplências


        public static void ContInadimplecia(string valor)
        {
          HttpContext.Current.Session["ContInadimplencia"] = valor;
    
        }


        public static string GetContInadimplecia()
        {

            if (HttpContext.Current.Session["ContInadimplencia"] != null)
                return HttpContext.Current.Session["ContInadimplencia"].ToString();
            else
                return "";
        }

        //Pega os usuarios tipo sindico

        public static Sindico GetUsuario()
        {

            string _login = HttpContext.Current.User.Identity.Name;

            if (HttpContext.Current.Session.Count > 0 || HttpContext.Current.Session["Usuario"] != null)
            {

                _login = HttpContext.Current.Session["Usuario"].ToString();


                //se os logins estiverem com sessões vazias

                if (_login == "")
                {

                    return null;
                }

                else
                {

                    Context _db = new Context();
                    Sindico usuarios = (from u in _db.Sindicos
                                        where u.Email == _login
                                        select u).SingleOrDefault();
                    return usuarios;

                }
            }
            else
            {

                return null;
            }

        }

        //verifica se os sindico está logado no sistema 

        public static Sindico GetUsuario(string _login)
        {
            if (_login == "")
            {
                return null;
            }
            else
            {
                Context _db = new Context();
                Sindico usuario = (from u in _db.Sindicos
                                   where u.Email == _login
                                   select u).SingleOrDefault();
                return usuario;
            }
        }






        //pega os usuarios tipo condomino

        public static Condomino GetCondomino()
        {

            string _login = HttpContext.Current.User.Identity.Name;

            if (HttpContext.Current.Session.Count > 0 || HttpContext.Current.Session["Usuario"] != null)
            {

                _login = HttpContext.Current.Session["Usuario"].ToString();

                //se os logins estiverem com sessões vazias

                if (_login == "")
                {

                    return null;
                }

                else
                {

                    Context _db = new Context();
                    Condomino usuarios = (from u in _db.Condominos
                                          where u.Email == _login
                                          select u).SingleOrDefault();
                    return usuarios;

                }
            }
            else
            {

                return null;
            }

        }

        //verifica se os condominos estão logado no sistema 

        public static Condomino GetCondomino(string _login)
        {
            if (_login == "")
            {
                return null;
            }
            else
            {
                Context _db = new Context();
                Condomino usuarios = (from u in _db.Condominos
                                      where u.Email == _login
                                      select u).SingleOrDefault();
                return usuarios;
            }
        }






        //Pega os usuarios tipo porteiro


        public static Porteiro GetPorteiro()
        {

            string _login = HttpContext.Current.User.Identity.Name;

            if (HttpContext.Current.Session.Count > 0 || HttpContext.Current.Session["Usuario"] != null)
            {

                _login = HttpContext.Current.Session["Usuario"].ToString();


                //se os logins estiverem com sessões vazias

                if (_login == "")
                {

                    return null;
                }

                else
                {

                    Context _db = new Context();
                    Porteiro usuarios = (from u in _db.Porteiros
                                         where u.Email == _login
                                         select u).SingleOrDefault();
                    return usuarios;

                }
            }
            else
            {

                return null;
            }

        }


        //verifica se os porteiros estão logados no sistema 

        public static Porteiro GetPorteiro(string _login)
        {
            if (_login == "")
            {
                return null;
            }
            else
            {
                Context _db = new Context();
                Porteiro usuarios = (from u in _db.Porteiros
                                     where u.Email == _login
                                     select u).SingleOrDefault();
                return usuarios;
            }
        }


        //Função pega administrador


        public static Administrador GetAdministrador()
        {

            string _login = HttpContext.Current.User.Identity.Name;

            if (HttpContext.Current.Session.Count > 0 || HttpContext.Current.Session["Usuario"] != null)
            {

                _login = HttpContext.Current.Session["Usuario"].ToString();


                //se os logins estiverem com sessões vazias

                if (_login == "")
                {

                    return null;
                }

                else
                {

                    Context _db = new Context();
                    Administrador usuarios = (from u in _db.Administradores
                                          where u.Email == _login
                                          select u).SingleOrDefault();
                    return usuarios;

                }
            }
            else
            {

                return null;
            }

        }


        //funcao que verifica se o administrador está logado

        public static Administrador GetAdministrador(string _login)
        {
            if (_login == "")
            {
                return null;
            }
            else
            {
                Context _db = new Context();
                Administrador usuarios = (from u in _db.Administradores
                                      where u.Email == _login
                                      select u).SingleOrDefault();
                return usuarios;
            }
        }

        //função para deslogar do sistema

        public static void Deslogar()
        {
            HttpContext.Current.Session["Usuario"] = "";
            FormsAuthentication.SignOut();
        }

       
        //HASH CODIGÃO

        public static string HashTexto(string texto, string nomeHash)
        {
            HashAlgorithm algoritmo = HashAlgorithm.Create(nomeHash);
            if (algoritmo == null)
            {
                throw new ArgumentException("Nome de hash incorreto", "nomeHash");
            }
            byte[] hash = algoritmo.ComputeHash(Encoding.UTF8.GetBytes(texto));
            return Convert.ToBase64String(hash).Replace("+", "");

        }

        public static string Base64Codifica(string texto)
        {
            byte[] stringBase64 = new byte[texto.Length];
            stringBase64 = Encoding.UTF8.GetBytes(texto);
            string codifica = Convert.ToBase64String(stringBase64);
            return codifica;
        }

        public static string Base64Decodifica(string texto)
        {
            var encode = new UTF8Encoding();
            var utf8Decode = encode.GetDecoder();

            byte[] stringValor = Convert.FromBase64String(texto);
            int contador = utf8Decode.GetCharCount(stringValor, 0, stringValor.Length);
            char[] decodeChar = new char[contador];
            utf8Decode.GetChars(stringValor, 0, stringValor.Length, decodeChar, 0);
            string resultado = new String(decodeChar);
            return resultado;
        }
    }
}
using AdminLar.Models;
using AdminLar.Repositories;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PI_4º_Semestre.Repositories;
using Newtonsoft.Json;
using System.Net;

namespace AdminLar.Controllers
{
    public class LandingController : Controller
    {

        // GET: Landing
        public ActionResult Logar()
        {
            return View();
        }


        public ActionResult RecuperarSenha()
        {

            return View();

        }

        //Sindico link ativacao

        public ActionResult CadastrarSenha(string mandaemail)
        {
            string email = Funcoes.Base64Decodifica(mandaemail);

            var id = db.Sindicos.Where(x => x.Email == email).Select(x => x.Codigo).FirstOrDefault();

            Sindico usuario = (Sindico)db.Usuarios.Find(id);

            return View(usuario);

        }

        //Porteiro link ativacao

        public ActionResult CadastrarSenhaP(string mandaemail)
        {
            string email = Funcoes.Base64Decodifica(mandaemail);

            var id = db.Porteiros.Where(x => x.Email == email).Select(x => x.Codigo).FirstOrDefault();

            Porteiro usuario = (Porteiro)db.Usuarios.Find(id);

            return View(usuario);

        }

        //Condomino link ativacao

        public ActionResult CadastrarSenhaC(string mandaemail)
        {

            string email = Funcoes.Base64Decodifica(mandaemail);

            var id = db.Condominos.Where(x => x.Email == email).Select(x => x.Codigo).FirstOrDefault();

            Condomino usuario = (Condomino)db.Usuarios.Find(id);


            return View(usuario);

        }


        //Action para recuperar senha

        public ActionResult Recupera(string email)
        {

            if (db.Usuarios.Any(x => x.Email == email))
            {

                var response = Request["g-recaptcha-response"];

                //chave secreta que foi gerada no site

                const string secret = "6LfK1j4UAAAAAHtP7zXZbgSE8XT8Zm25pFm0sSUZ";

                var client = new WebClient();
                var reply =
                client.DownloadString(

               string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
               secret, response));
                var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                //Response false – devemos ver qual a mensagem de erro

                if (!captchaResponse.Success)
                {
                    if (captchaResponse.ErrorCodes.Count <= 0) return View();

                    var error = captchaResponse.ErrorCodes[0].ToLower();

                    switch (error)
                    {
                        case ("missing-input-secret"):
                            ViewBag.Message = "The secret parameter is missing.";
                            break;

                        case ("invalid-input-secret"):
                            ViewBag.Message = "The secret parameter is invalid or malformed.";
                     break;

                        case ("missing-input-response"):
                            ViewBag.Message = "The response parameter is missing.";
                            break;

                        case ("invalid-input-response"):
                            ViewBag.Message = "The response parameter is invalid or malformed.";
                     break;

                        default:
                            ViewBag.Message = "Error occured. Please try again";
                            break;
                    }
                    TempData["Error"] = "Certifique-se que você não é um robô";
                }

                else
                {

                    var callbackUrl = Url.Action("Recuperar", "Landing", new { recupera = Funcoes.Base64Codifica(email) }, protocol: Request.Url.Scheme);


                    GmailEmailService gmail = new GmailEmailService();
                    EmailMessage msg = new EmailMessage();
                    msg.Body = "Link para redefinir a senha \n\n clicando aqui: " + callbackUrl;
                    msg.IsHtml = false;
                    msg.Subject = "Redefinir senha";
                    msg.ToEmail = email;
                    gmail.SendEmailMessage(msg);

                    TempData["Sucesso"] = "Um link para redefinir sua senha foi enviado para o seu email";

                }

            }
            else
            {
                TempData["Error"] = "Email não encontrado no sistema";

            }

            return RedirectToAction("RecuperarSenha", "Landing");
        }


        //Action para redefinir a senha

        public ActionResult Recuperar(string recupera)
        {

            string email = Funcoes.Base64Decodifica(recupera);

            var id = db.Usuarios.Where(x => x.Email == email).Select(x => x.Codigo).FirstOrDefault();

            Usuario usuario = db.Usuarios.Find(id);


            return View(usuario);


        }

        //action para redefinir mesmo

        [HttpPost]
        public ActionResult Senhas(int codigo, string senha, string confirma)
        {

                Usuario sin = db.Usuarios.Find(codigo);
                sin.Senha = senha;
                sin.Confirmasenha = confirma;

                db.Entry(sin).State = EntityState.Modified;

                if (db.SaveChanges() > 0)
                {

                    return Json(true);

                }
       
            return Json(false);
        }


        [HttpPost]
        public ActionResult Redefinir(int codigo, string senha, string confirma)
        {

            Usuario usu = db.Usuarios.Find(codigo);
            usu.Senha = senha;
            usu.Confirmasenha = confirma;


            db.Entry(usu).State = EntityState.Modified;

            if (db.SaveChanges() > 0)
            {

                return Json(true);

            }

            return Json(false);
        }



        [HttpPost]
        public ActionResult EnviaEmail(string nome, string email, string assunto, string mensagem)
        {

            GmailEmailService gmail = new GmailEmailService();
            EmailMessage msg = new EmailMessage();
            msg.Body = "Nome do Usuário: " + nome + "\n\n Email do Usuário: " + email + "\n\n Assunto: " + mensagem;
            msg.IsHtml = false;
            msg.Subject = assunto;
            msg.ToEmail = "admlteam2018@gmail.com";
            gmail.SendEmailMessage(msg);



            return Json(true);

        }


        Context db = new Context();

        //Action para verificar se o usuario existe 

        [HttpPost]
        public ActionResult Logar(string email, string senha)
        {
            Usuario usu = Funcoes.AutenciaUsuario(email, senha);

            if (usu == null)
            {
                ViewBag.Error = "Nome de usuário e/ou senha inválidos";

                return View();

            }
            else if (usu.Tipo == "S")
            {

                return RedirectToAction("Inicio", "Sindicos");


            }
            else if (usu.Tipo == "C")
            {

                return RedirectToAction("Inicio", "Condomino");


            }
            else if (usu.Tipo == "P")
            {

                return RedirectToAction("Index", "Porteiros");

            }
            else if (usu.Tipo == "A")
            {

                return RedirectToAction("Index", "Administradors");

            }

            return View();

        }

        public ActionResult AcessoNegado()
        {
            using (Context c = new Context())
            {
                return View();
            }
        }


        public ActionResult Logoff()
        {

            AdminLar.Repositories.Funcoes.Deslogar();
            return RedirectToAction("Logar", "Landing");

        }
    }
}
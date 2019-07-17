using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc.Async;
using System.Web;
using System.Web.Mvc;
using AdminLar.Models;
using PI_4º_Semestre.Models;
using PI_4º_Semestre.Repositories;
using AdminLar.Repositories;

namespace PI_4º_Semestre.Controllers
{
    public class AdministradorsController : Controller
    {
        private Context db = new Context();

        // GET: Administradors
        public ActionResult Index()
        {
            Administrador usuario = AdminLar.Repositories.Funcoes.GetAdministrador();

            if (usuario != null)
            {

                ViewBag.Sindico = db.Usuarios.OfType<Sindico>().Count(x => x.Codigo != null);

                ViewBag.Condominio = db.Apartamento.Count(x => x.ApeId != null);

                return View(db.Usuarios.OfType<Administrador>().ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }
        }


        public ActionResult AdicionarApartamento()
        {

            return View(db.Apartamento.ToList());

        }


        public ActionResult AdicionarSindico()
        {


            Apartamento ape = new Apartamento();

            ViewBag.Apartamento = db.Apartamento.ToList();


            var sindicos = db.Sindicos.Include(v => v.Apartamento);

            return View(sindicos.ToList());

        }

        //Action para salvar condomínios

        [HttpPost]
        public ActionResult SalvaApe(string nome, string endereco)
        {

            Apartamento ape = new Apartamento();

            ape.NomeApe = nome;
            ape.Endereco = endereco;

            db.Apartamento.Add(ape);


            if (db.SaveChanges() > 0)
            {

                return Json(true);

            }

            return Json(false);

        }


        [HttpPost]
        public ActionResult Sindicos(string email, string nome, string cpf, string data, string numeroape, string telefone, string celular, string codigo)
        {

            Sindico sin = new Sindico();

            sin.Email = email;
            sin.Sin_Nome = nome;
            sin.Sin_Cpf = cpf;
            sin.Tipo = "S";
            sin.Sin_DataNascimento = data;
            sin.Sin_NumeroApartamento = Convert.ToInt32(numeroape);
            sin.Sin_Telefone = telefone;
            sin.Sin_Celular = celular;
            sin.ApeId = Convert.ToInt32(codigo);

            var callbackUrl = Url.Action("CadastrarSenha", "Landing", new { mandaemail = Funcoes.Base64Codifica(email) }, protocol: Request.Url.Scheme);


            GmailEmailService gmail = new GmailEmailService();
            EmailMessage msg = new EmailMessage();
            msg.Body = "Bem vindo Confirme a sua senha clicando aqui: " + callbackUrl;
            msg.IsHtml = false;
            msg.Subject = "Bem vindo a AdminLar (Confirmação de Cadastro)";
            msg.ToEmail = email;
            gmail.SendEmailMessage(msg);

            db.Sindicos.Add(sin);

            if (db.SaveChanges() > 0)
            {

                return Json(true);

            }

            return Json(false);

        }


        //public ActionResult EnviarLink(int ide, string email)
        //{

        //    var callbackUrl = Url.Action("CadastrarSenha", "Landing", new { id = Funcoes.Base64Codifica(ide.ToString()) }, protocol: Request.Url.Scheme);


        //    GmailEmailService gmail = new GmailEmailService();
        //    EmailMessage msg = new EmailMessage();
        //    msg.Body = "Bem vindo Confirme a sua senha clicando aqui: " + callbackUrl;
        //    msg.IsHtml = false;
        //    msg.Subject = "Bem vindo a AdminLar (Confirmação de Cadastro)";
        //    msg.ToEmail = email;
        //    gmail.SendEmailMessage(msg);

        //    return RedirectToAction("AdicionarSindico");

        //}



        // GET: Administradors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = (Administrador)db.Usuarios.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            return View(administrador);
        }

        // GET: Administradors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administradors/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo,Email,Senha,Confirmasenha,Tipo,AdmNome")] Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(administrador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(administrador);
        }

        // GET: Administradors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = (Administrador)db.Usuarios.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            return View(administrador);
        }

        // POST: Administradors/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo,Email,Senha,Confirmasenha,Tipo,AdmNome")] Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administrador).State = EntityState.Modified;

                if(db.SaveChanges() > 0)
                {

                    ViewBag.Success = "Dados alterados com sucesso";

                }

              //  return RedirectToAction("Index");
            }
            return View(administrador);
        }

        // GET: Administradors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = (Administrador)db.Usuarios.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            return View(administrador);
        }

        // POST: Administradors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Administrador administrador = (Administrador)db.Usuarios.Find(id);
            db.Usuarios.Remove(administrador);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

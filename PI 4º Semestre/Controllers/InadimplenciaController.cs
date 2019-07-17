using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLar.Models;
using PI_4º_Semestre.Repositories;

namespace AdminLar.Controllers
{
    public class InadimplenciaController : Controller
    {
        private Context db = new Context();

        // GET: Inadimplencia
        public ActionResult Index()
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                ViewBag.Condominos = db.Condominos.Include(p => p.Apartamento).Where(p => p.ApeId == usuario.ApeId);

                return View(db.Inadimplecias.Include(e => e.Apartamento).Include(e => e.Condomino).Where(e => e.ApeId == usuario.ApeId).ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }

        [HttpPost]
        public ActionResult Inadimplencias([Bind(Include = "InaId,valor,Status,Codigo,ApeId")] Inadimplencia inadimplencia, string codigo)
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                inadimplencia.Codigo = Convert.ToInt32(codigo);
                inadimplencia.Status = "Devendo";
                inadimplencia.ApeId = usuario.ApeId;
                db.Inadimplecias.Add(inadimplencia);


                var converte = Convert.ToInt32(codigo);
                
                //pega o email do condomino inadimplete

                var email = db.Usuarios.Where(x => x.Codigo == converte).Select(x => x.Email).FirstOrDefault();

                var nome = db.Usuarios.OfType<Condomino>().Where(x => x.Codigo == converte).Select(x => x.Con_Nome).FirstOrDefault();

                var ape = db.Sindicos.Where(x => x.Codigo == usuario.Codigo).Select(x => x.Apartamento.NomeApe).First();

                GmailEmailService gmail = new GmailEmailService();
                EmailMessage msg = new EmailMessage();
                msg.Body = "Prezado(a) " + nome + "\n\n encontramos no nosso sistema pendências em seu nome. \n\n Entre em contato com o Síndico ou caso você já tenha efetuado o pagamento desconsidere essa mensagem \n\n Atenciosamente " + ape;
                msg.IsHtml = false;
                msg.Subject = "Pendência";
                msg.ToEmail = Convert.ToString(email);
                gmail.SendEmailMessage(msg);

                if (db.SaveChanges() > 0)
                {

                    return RedirectToAction("Index", "Inadimplencia");

                }

            }

            return Json(false);

        }


            // GET: Inadimplencia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inadimplencia inadimplencia = db.Inadimplecias.Find(id);
            if (inadimplencia == null)
            {
                return HttpNotFound();
            }
            return View(inadimplencia);
        }

        // GET: Inadimplencia/Create
        public ActionResult Create()
        {
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe");
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email");
            return View();
        }

        // POST: Inadimplencia/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InaId,valor,Status,Codigo,ApeId")] Inadimplencia inadimplencia)
        {
            if (ModelState.IsValid)
            {
                db.Inadimplecias.Add(inadimplencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", inadimplencia.ApeId);
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email", inadimplencia.Codigo);
            return View(inadimplencia);
        }

        // GET: Inadimplencia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inadimplencia inadimplencia = db.Inadimplecias.Find(id);
            if (inadimplencia == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", inadimplencia.ApeId);
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email", inadimplencia.Codigo);
            return View(inadimplencia);
        }

        // POST: Inadimplencia/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InaId,valor,Status,Codigo,ApeId")] Inadimplencia inadimplencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inadimplencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", inadimplencia.ApeId);
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email", inadimplencia.Codigo);
            return View(inadimplencia);
        }

        // GET: Inadimplencia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inadimplencia inadimplencia = db.Inadimplecias.Find(id);
            if (inadimplencia == null)
            {
                return HttpNotFound();
            }
            return View(inadimplencia);
        }

        // POST: Inadimplencia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inadimplencia inadimplencia = db.Inadimplecias.Find(id);
            db.Inadimplecias.Remove(inadimplencia);
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

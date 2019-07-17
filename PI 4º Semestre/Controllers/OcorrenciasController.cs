using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLar.Models;

namespace AdminLar.Controllers
{
    public class OcorrenciasController : Controller
    {
        private Context db = new Context();

        // GET: Ocorrencias
        public ActionResult Index()
        {
            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {

                var ocorrencias = db.Ocorrencias.Include(o => o.Apartamento).Include(o => o.Condomino).Where(o => o.Codigo == usuario.Codigo && o.ApeId == usuario.ApeId);
                return View(ocorrencias.ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");


            }
        }

        // GET: Ocorrencias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ocorrencias ocorrencias = db.Ocorrencias.Find(id);
            if (ocorrencias == null)
            {
                return HttpNotFound();
            }
            return View(ocorrencias);
        }

        // GET: Ocorrencias/Create
        public ActionResult Create()
        {
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe");
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email");
            return View();
        }


        [HttpPost]
        public ActionResult Create([Bind(Include = "ocoid,datahoraocorrencia,Descricao,CondoNome,Status,ApeId,Codigo")] Ocorrencias ocorrencias)
        {

            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {

                ocorrencias.CondoNome = usuario.Con_Nome;
                ocorrencias.ApeId = usuario.ApeId;
                ocorrencias.datahoraocorrencia = DateTime.Now;
                ocorrencias.Status = "Ativo";
                ocorrencias.Codigo = usuario.Codigo;
                db.Ocorrencias.Add(ocorrencias);

                db.SaveChanges();

                return RedirectToAction("Index", "Ocorrencias");

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");


            }

        }

        // GET: Ocorrencias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ocorrencias ocorrencias = db.Ocorrencias.Find(id);
            if (ocorrencias == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", ocorrencias.ApeId);
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email", ocorrencias.Codigo);
            return View(ocorrencias);
        }

        // POST: Ocorrencias/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ocoid,datahoraocorrencia,Descricao,CondoNome,Status,ApeId,Codigo")] Ocorrencias ocorrencias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ocorrencias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", ocorrencias.ApeId);
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email", ocorrencias.Codigo);
            return View(ocorrencias);
        }

        // GET: Ocorrencias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ocorrencias ocorrencias = db.Ocorrencias.Find(id);
            if (ocorrencias == null)
            {
                return HttpNotFound();
            }
            return View(ocorrencias);
        }

        // POST: Ocorrencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ocorrencias ocorrencias = db.Ocorrencias.Find(id);
            db.Ocorrencias.Remove(ocorrencias);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLar.Models;
using PI_4º_Semestre.Models;

namespace PI_4º_Semestre.Controllers
{
    public class ApartamentosController : Controller
    {
        private Context db = new Context();

        // GET: Apartamentos
        public ActionResult Index()
        {

            return View(db.Apartamento.ToList());
        }

        // GET: Apartamentos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartamento apartamento = db.Apartamento.Find(id);
            if (apartamento == null)
            {
                return HttpNotFound();
            }
            return View(apartamento);
        }

        // GET: Apartamentos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Apartamentos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApeId,NomeApe,Endereco")] Apartamento apartamento)
        {
            if (ModelState.IsValid)
            {
                db.Apartamento.Add(apartamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(apartamento);
        }

        // GET: Apartamentos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartamento apartamento = db.Apartamento.Find(id);
            if (apartamento == null)
            {
                return HttpNotFound();
            }
            return View(apartamento);
        }

        // POST: Apartamentos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApeId,NomeApe,Endereco")] Apartamento apartamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apartamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(apartamento);
        }

        // GET: Apartamentos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartamento apartamento = db.Apartamento.Find(id);
            if (apartamento == null)
            {
                return HttpNotFound();
            }
            return View(apartamento);
        }

        // POST: Apartamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Apartamento apartamento = db.Apartamento.Find(id);
            db.Apartamento.Remove(apartamento);
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

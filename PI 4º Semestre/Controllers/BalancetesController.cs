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
    public class BalancetesController : Controller
    {
        private Context db = new Context();

        // GET: Balancetes
        public ActionResult Index()
        {
            var balancetes = db.Balancetes.Include(b => b.Apartamento);
            return View(balancetes.ToList());
        }

        // GET: Balancetes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Balancetes balancetes = db.Balancetes.Find(id);
            if (balancetes == null)
            {
                return HttpNotFound();
            }
            return View(balancetes);
        }

        // GET: Balancetes/Create
        public ActionResult Create()
        {
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe");
            return View();
        }


        [HttpPost]
        public ActionResult SalvaBalancete([Bind(Include = "Balid,DataBal,Descricao,Arquivo,ApeId")] Balancetes bal, HttpPostedFileBase arquivo, string data)
        {
            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                string fileName = "";
                string contentType = "";
                string path = "";

                if (arquivo != null && arquivo.ContentLength > 0)
                {
                    fileName = System.IO.Path.GetFileName(arquivo.FileName);
                    contentType = arquivo.ContentType;

                    path = System.Configuration.ConfigurationManager.AppSettings["PathFiles"] + "\\Atas\\" + fileName;

                    arquivo.SaveAs(path);

                    bal.Arquivo = fileName;

                }

                bal.ApeId = usuario.ApeId;
                bal.DataBal = Convert.ToDateTime(data);

                db.Balancetes.Add(bal);

                if (db.SaveChanges() > 0)
                {

                    return RedirectToAction("Index", "Balancetes");

                }
            }

            return Json(false);
        }

        // GET: Balancetes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Balancetes balancetes = db.Balancetes.Find(id);
            if (balancetes == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", balancetes.ApeId);
            return View(balancetes);
        }

        // POST: Balancetes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Balid,DataBal,Descricao,Arquivo,ApeId")] Balancetes balancetes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(balancetes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", balancetes.ApeId);
            return View(balancetes);
        }

        // GET: Balancetes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Balancetes balancetes = db.Balancetes.Find(id);
            if (balancetes == null)
            {
                return HttpNotFound();
            }
            return View(balancetes);
        }

        // POST: Balancetes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Balancetes balancetes = db.Balancetes.Find(id);
            db.Balancetes.Remove(balancetes);
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

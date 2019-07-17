using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLar.Models;

namespace PI_4º_Semestre.Controllers
{
    public class AtasController : Controller
    {
        private Context db = new Context();

        // GET: Atas
        public ActionResult Index()
        {
            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                return View(db.Ata.Include(a => a.Apartamento).Where(a => a.ApeId == usuario.ApeId).ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }


        [HttpPost]
        public ActionResult SalvaAta([Bind(Include = "Ataid,DataAta,Descricao,Arquivo,ApeId")] Atas atas, HttpPostedFileBase arquivo, string data)
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

                    atas.Arquivo = fileName;

                }

                atas.ApeId = usuario.ApeId;
                atas.DataAta = Convert.ToDateTime(data);

                db.Ata.Add(atas);

                if (db.SaveChanges() > 0)
                {

                    return RedirectToAction("Index", "Atas");

                }

            }

            return Json(false);
        }


        // GET: Atas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atas atas = db.Ata.Find(id);
            if (atas == null)
            {
                return HttpNotFound();
            }
            return View(atas);
        }

        // GET: Atas/Create
        public ActionResult Create()
        {
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe");
            return View();
        }

        // POST: Atas/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ataid,DataAta,Descricao,Arquivo,ApeId")] Atas atas)
        {
            if (ModelState.IsValid)
            {
                db.Ata.Add(atas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", atas.ApeId);
            return View(atas);
        }

        // GET: Atas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atas atas = db.Ata.Find(id);
            if (atas == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", atas.ApeId);
            return View(atas);
        }

        // POST: Atas/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ataid,DataAta,Descricao,Arquivo,ApeId")] Atas atas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(atas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", atas.ApeId);
            return View(atas);
        }

        // GET: Atas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atas atas = db.Ata.Find(id);
            if (atas == null)
            {
                return HttpNotFound();
            }
            return View(atas);
        }

        // POST: Atas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Atas atas = db.Ata.Find(id);
            db.Ata.Remove(atas);
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

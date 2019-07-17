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
    public class AvisosController : Controller
    {
        private Context db = new Context();

        // GET: Avisos
        public ActionResult Index()
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                ViewBag.Data = DateTime.Now;

                DateTime expiro = DateTime.Now.AddDays(-1);

                return View(db.Avisos.Include(p => p.Apartamento).Where(p => p.ApeId == usuario.ApeId && expiro < p.DataAviso).OrderBy(t => t.DataAviso));

            } else
            {

                return RedirectToAction("AcessoNegado", "Landing");
            }

        }

        // GET: Avisos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aviso aviso = db.Avisos.Find(id);
            if (aviso == null)
            {
                return HttpNotFound();
            }
            return View(aviso);
        }

        // GET: Avisos/Create
        public ActionResult Create()
        {
            return View();
        }


        //cadastra Avisos via Json

        [HttpPost]
        public ActionResult SalvaAviso(string dataaviso, string descricao)
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                Aviso aviso = new Aviso();
        
                aviso.DataAviso = DateTime.Parse(dataaviso);
                aviso.Descricao = descricao;
                aviso.ApeId = usuario.ApeId;

                db.Avisos.Add(aviso);

                if (db.SaveChanges() > 0)
                {

                    return Json(true);

                }

            }

            return Json(false);
        }


        // POST: Avisos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Idaviso,DataAviso,Descricao")] Aviso aviso)
        {
            if (ModelState.IsValid)
            {
                db.Avisos.Add(aviso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aviso);
        }

        // GET: Avisos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aviso aviso = db.Avisos.Find(id);
            if (aviso == null)
            {
                return HttpNotFound();
            }
            return View(aviso);
        }

        // POST: Avisos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Idaviso,DataAviso,Descricao,AvisoExpira,ApeId")] Aviso aviso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aviso).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Success = "Aviso alterado com sucesso";
                //return RedirectToAction("Index");
            }
            return View(aviso);
        }

        // GET: Avisos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aviso aviso = db.Avisos.Find(id);
            if (aviso == null)
            {
                return HttpNotFound();
            }
            return View(aviso);
        }

        // POST: Avisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aviso aviso = db.Avisos.Find(id);
            db.Avisos.Remove(aviso);
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

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
    public class AreaLazerController : Controller
    {
        private Context db = new Context();

        // GET: AreaLazer
        public ActionResult Index()
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                return View(db.AreaLazer.Include(a => a.Apartamento).Where(a => a.ApeId == usuario.ApeId).ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }
        }


        //Action recebe dados enviados da área de lazer via Jquery Json

        [HttpPost]
        public ActionResult SalvaArea(string nomearea, string descricao, string regras)
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                AreaLazer area = new AreaLazer();

                area.NomeArea = nomearea;
                area.descricao = descricao;
                area.Regras = regras;
                area.ApeId = usuario.ApeId;

                db.AreaLazer.Add(area);

                if (db.SaveChanges() > 0)
                {

                    return Json(true);

                }
            }


            return Json(false);

        }

        // GET: AreaLazer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaLazer areaLazer = db.AreaLazer.Find(id);
            if (areaLazer == null)
            {
                return HttpNotFound();
            }
            return View(areaLazer);
        }

        // GET: AreaLazer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AreaLazer/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Areaid,NomeArea,descricao,Regras,ApeId")] AreaLazer areaLazer)
        {
            if (ModelState.IsValid)
            {
                db.AreaLazer.Add(areaLazer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(areaLazer);
        }

        // GET: AreaLazer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaLazer areaLazer = db.AreaLazer.Find(id);
            if (areaLazer == null)
            {
                return HttpNotFound();
            }
            return View(areaLazer);
        }

        // POST: AreaLazer/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Areaid,NomeArea,descricao,,Regras,ApeId")] AreaLazer areaLazer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(areaLazer).State = EntityState.Modified;

                if(db.SaveChanges() > 0)
                {

                    ViewBag.Success = "Area Alterada com sucesso";

                }
               // return RedirectToAction("Index");
            }
            return View(areaLazer);
        }

        // GET: AreaLazer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaLazer areaLazer = db.AreaLazer.Find(id);
            if (areaLazer == null)
            {
                return HttpNotFound();
            }
            return View(areaLazer);
        }

        // POST: AreaLazer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AreaLazer areaLazer = db.AreaLazer.Find(id);
            db.AreaLazer.Remove(areaLazer);
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

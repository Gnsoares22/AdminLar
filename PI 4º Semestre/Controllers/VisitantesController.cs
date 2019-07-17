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
    public class VisitantesController : Controller
    {
        private Context db = new Context();

        // GET: Visitantes
        public ActionResult Index()
        {
            Porteiro usuario = AdminLar.Repositories.Funcoes.GetPorteiro();

            if (usuario != null)
            {

                Visitante visitante = new Visitante();

                ViewBag.Condominos = db.Condominos.Include(p => p.Apartamento).Where(p => p.ApeId == usuario.ApeId);


                 return View(db.Visitantes.Include(v => v.Condomino).Where(v => v.ApeId == usuario.ApeId).ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }

        //Cadastra Visitantes

        [HttpPost]
        public ActionResult SalvaVisitante(string rg, string visitante, string date, string codigo)
        {

            Porteiro usuario = AdminLar.Repositories.Funcoes.GetPorteiro();

            if (usuario != null)
            {

                Visitante visita = new Visitante();

                visita.NumeroRg = rg;
                visita.NumeroApartamentoVisita = Convert.ToInt32(codigo);
                visita.NomeVisitante = visitante;
                visita.DataVisita = date;
                visita.Codigo = Convert.ToInt32(codigo);
                visita.ApeId = usuario.ApeId;

                db.Visitantes.Add(visita);


                if (db.SaveChanges() > 0)
                {
                    return Json(true);
                }

            }

            return Json(false);

        }



        // GET: Visitantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visitante visitante = db.Visitantes.Find(id);
            if (visitante == null)
            {
                return HttpNotFound();
            }
            return View(visitante);
        }

        // GET: Visitantes/Create
        public ActionResult Create()
        {
            ViewBag.Codigo = new SelectList(db.Condominos, "Codigo", "Con_Nome");
            return View();
        }

        // POST: Visitantes/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoVisita,NumeroRg,NomeVisitante,NumeroApartamentoVisita,StatusVisita,Codigo")] Visitante visitante)
        {
            if (ModelState.IsValid)
            {
                db.Visitantes.Add(visitante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Codigo = new SelectList(db.Condominos, "Codigo", "Con_Nome", visitante.Codigo);


            return View(visitante);
        }

        // GET: Visitantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visitante visitante = db.Visitantes.Find(id);
            if (visitante == null)
            {
                return HttpNotFound();
            }
            ViewBag.Codigo = new SelectList(db.Condominos, "Codigo", "Con_Nome", visitante.Codigo);
            return View(visitante);
        }

        // POST: Visitantes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoVisita,NumeroRg,NomeVisitante,NumeroApartamentoVisita,StatusVisita,Codigo,ApeId")] Visitante visitante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visitante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Codigo = new SelectList(db.Condominos, "Codigo", "Email", visitante.Codigo);
            return View(visitante);
        }

        // GET: Visitantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visitante visitante = db.Visitantes.Find(id);
            if (visitante == null)
            {
                return HttpNotFound();
            }
            return View(visitante);
        }

        // POST: Visitantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Visitante visitante = db.Visitantes.Find(id);
            db.Visitantes.Remove(visitante);
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

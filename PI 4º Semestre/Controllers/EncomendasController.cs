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
    public class EncomendasController : Controller
    {
        private Context db = new Context();

        // GET: Encomendas
        public ActionResult Index()
        {

            Porteiro usuario = AdminLar.Repositories.Funcoes.GetPorteiro();

            if (usuario != null)
            {

                ViewBag.Condominos = db.Condominos.Include(p => p.Apartamento).Where(p => p.ApeId == usuario.ApeId);


                return View(db.Encomenda.Include(e => e.Apartamento).Include(e => e.Condomino).Where(e => e.ApeId == usuario.ApeId && e.Status == "Recebido").ToList());

            } else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }

        // GET: Encomendas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encomendas encomendas = db.Encomenda.Find(id);
            if (encomendas == null)
            {
                return HttpNotFound();
            }
            return View(encomendas);
        }

        // GET: Encomendas/Create
        public ActionResult Create()
        {
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe");
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email");
            return View();
        }

        // POST: Encomendas/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult SalvaEncomenda([Bind(Include = "EncoId,Descricao,Status,Codigo,ApeId,DataEntrega")] Encomendas encomendas, string data)
        {

            Porteiro usuario = AdminLar.Repositories.Funcoes.GetPorteiro();

            if (usuario != null)
            {

                encomendas.Status = "Recebido";
                encomendas.ApeId = usuario.ApeId;
                encomendas.DataEntrega = Convert.ToDateTime(data);
                db.Encomenda.Add(encomendas);
                db.SaveChanges();

            }

            return RedirectToAction("Index","Encomendas");
        }


        [HttpPost]
        public ActionResult Cancela(string codigo)
        {

            Encomendas enc = db.Encomenda.Find(Convert.ToInt32(codigo));

            enc.Status = "Cancelado";

            db.Entry(enc).State = EntityState.Modified;

            
            if(db.SaveChanges() > 0)
            {

                return Json(true);

            }

            return Json(false);
        }


        // GET: Encomendas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encomendas encomendas = db.Encomenda.Find(id);
            if (encomendas == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", encomendas.ApeId);
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email", encomendas.Codigo);
            return View(encomendas);
        }

        // POST: Encomendas/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EncoId,Descricao,Status,Codigo,ApeId")] Encomendas encomendas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(encomendas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", encomendas.ApeId);
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email", encomendas.Codigo);
            return View(encomendas);
        }

        // GET: Encomendas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encomendas encomendas = db.Encomenda.Find(id);
            if (encomendas == null)
            {
                return HttpNotFound();
            }
            return View(encomendas);
        }

        // POST: Encomendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Encomendas encomendas = db.Encomenda.Find(id);
            db.Encomenda.Remove(encomendas);
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

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
    public class ReservasController : Controller
    {
        private Context db = new Context();

        // GET: Reservas
        public ActionResult Index()
        {

            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {

                ViewBag.Condominos = db.Condominos.Include(p => p.Apartamento).Where(p => p.ApeId == usuario.ApeId);

                ViewBag.Area = db.AreaLazer.Include(p => p.Apartamento).Where(p => p.ApeId == usuario.ApeId);

                DateTime expiro = DateTime.Now.AddDays(-1);

                return View(db.Reservas.Include(e => e.Apartamento).Include(e => e.Condomino).Include(e => e.AreaLazer).Where(e => e.ApeId == usuario.ApeId && expiro < e.DataReserva).OrderBy(t => t.DataReserva).ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }

        //Get somente minhas reservas

        public ActionResult MinhasReservas()
        {

            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {

                DateTime expiro = DateTime.Now.AddDays(-1);

                return View(db.Reservas.Include(e => e.Apartamento).Include(e => e.Condomino).Include(e => e.AreaLazer).Where(e => e.ApeId == usuario.ApeId && e.Codigo == usuario.Codigo && expiro < e.DataReserva).OrderBy(t => t.DataReserva).ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }


        //Realiza reservas

        [HttpPost]
        public ActionResult Reservar([Bind(Include = "ReservaId,DataReserva,Codigo,Areaid,ApeId")] Reserva reserva, string data, string codigo)
        {

            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {


                reserva.ApeId = usuario.ApeId;
                reserva.Areaid = Convert.ToInt32(codigo);
                reserva.Codigo = usuario.Codigo;
                reserva.DataReserva = Convert.ToDateTime(data);
                reserva.Status = "Reservado";

                if(db.Reservas.Any(x => x.DataReserva == reserva.DataReserva))
                {

                    return RedirectToAction("Index", "Reservas");

                }

                db.Reservas.Add(reserva);
                db.SaveChanges();
                return RedirectToAction("Index", "Reservas");

            }

            return Json(false);


        }


        //Cancela Reserva

        [HttpPost]
        public ActionResult Cancela(string codigo)
        {

            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {

                Reserva reserva = db.Reservas.Find(Convert.ToInt32(codigo));

                reserva.Status = "Cancelado";


                reserva.DataReserva = null;


                db.Entry(reserva).State = EntityState.Modified;


                if (db.SaveChanges() > 0)
                {

                    return RedirectToAction("MinhasReservas", "Reservas");

                }
            }
            return Json(false);

        }


        // GET: Reservas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // GET: Reservas/Create
        public ActionResult Create()
        {
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe");
            ViewBag.Areaid = new SelectList(db.AreaLazer, "Areaid", "NomeArea");
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email");
            return View();
        }

        // POST: Reservas/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservaId,DataReserva,Codigo,Areaid,ApeId")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                db.Reservas.Add(reserva);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", reserva.ApeId);
            ViewBag.Areaid = new SelectList(db.AreaLazer, "Areaid", "NomeArea", reserva.Areaid);
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email", reserva.Codigo);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", reserva.ApeId);
            ViewBag.Areaid = new SelectList(db.AreaLazer, "Areaid", "NomeArea", reserva.Areaid);
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email", reserva.Codigo);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservaId,DataReserva,Codigo,Areaid,ApeId")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reserva).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApeId = new SelectList(db.Apartamento, "ApeId", "NomeApe", reserva.ApeId);
            ViewBag.Areaid = new SelectList(db.AreaLazer, "Areaid", "NomeArea", reserva.Areaid);
            ViewBag.Codigo = new SelectList(db.Usuarios, "Codigo", "Email", reserva.Codigo);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reserva reserva = db.Reservas.Find(id);
            db.Reservas.Remove(reserva);
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

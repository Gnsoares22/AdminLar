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
    public class PorteirosController : Controller
    {
        private Context db = new Context();

        // GET: Porteiros
        public ActionResult Index()
        {

            Porteiro usuario = AdminLar.Repositories.Funcoes.GetPorteiro();


            if (usuario != null)
            {

                ViewBag.Visitantes = db.Visitantes.Count(x => x.ApeId == usuario.ApeId && x.CodigoVisita != null);
                ViewBag.Encomendas = db.Encomenda.Count(x => x.ApeId == usuario.ApeId && x.Status == "Recebido" && x.EncoId != null);

                DateTime expiro = DateTime.Now.AddDays(-1);

                return View(db.Avisos.Include(p => p.Apartamento).Where(p => p.ApeId == usuario.ApeId && expiro < p.DataAviso).OrderBy(t => t.DataAviso));


            }
            else
            {
                return RedirectToAction("AcessoNegado", "Landing");
            }

        }


        // GET: Porteiros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Porteiro porteiro = (Porteiro)db.Usuarios.Find(id);
            if (porteiro == null)
            {
                return HttpNotFound();
            }
            return View(porteiro);
        }

        // GET: Porteiros/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Porteiros/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo,Email,Senha,Tipo,Por_Nome,Por_Cpf,Por_Sexo,Por_DataNascimento,Por_Endereco,Por_Telefone,Por_Celular")] Porteiro porteiro)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(porteiro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(porteiro);
        }

        // GET: Porteiros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Porteiro porteiro = (Porteiro)db.Usuarios.Find(id);
            if (porteiro == null)
            {
                return HttpNotFound();
            }
            return View(porteiro);
        }

        // POST: Porteiros/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo,Email,Senha,Confirmasenha,Tipo,Por_Nome,Por_Cpf,Por_DataNascimento,Por_Endereco,Por_Telefone,Por_Celular,ApeId")] Porteiro porteiro)
        {
            if (db.Sindicos.Any(x => x.Email == porteiro.Email))
            {

                ViewBag.Error = "Email já cadastrado no sistema";

            }
            else if (db.Condominos.Any(x => x.Email == porteiro.Email))
            {

                ViewBag.Error = "Email já cadastrado no sistema";

            }

            else if (db.Condominos.Any(x => x.Con_Cpf == porteiro.Por_Cpf))
            {

                ViewBag.Error = "CPF já existente";

            }

            else if (db.Sindicos.Any(x => x.Sin_Cpf == porteiro.Por_Cpf))
            {

                ViewBag.Error = "CPF já existente";

            }
            else
            {

                if (ModelState.IsValid)
                {

                    db.Entry(porteiro).State = EntityState.Modified;

                    db.SaveChanges();

                    ViewBag.Success = "Dados salvos com sucesso";

                }

                //return RedirectToAction("Index");
            }
            return View(porteiro);
        }

        // GET: Porteiros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Porteiro porteiro = (Porteiro)db.Usuarios.Find(id);
            if (porteiro == null)
            {
                return HttpNotFound();
            }
            return View(porteiro);
        }

        // POST: Porteiros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Porteiro porteiro = (Porteiro)db.Usuarios.Find(id);
            db.Usuarios.Remove(porteiro);
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

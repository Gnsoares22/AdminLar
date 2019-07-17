using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLar.Models;
using AdminLar.Repositories;
using PI_4º_Semestre.Models;

namespace PI_4º_Semestre.Controllers
{
    public class CondominoController : Controller
    {
        private Context db = new Context();


        //pega o usuario logado e conta a quantidade de visitantes no Id (serve para fazer a notificacao)

        public ActionResult Index()
        {
            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {
                Condomino con = db.Usuarios.OfType<Condomino>().Include(t => t.Visitantes).Where(t => t.Codigo == usuario.Codigo && t.ApeId == usuario.ApeId).FirstOrDefault();
            //    Funcoes.ContCondomino(con.Visitantes.Count.ToString());
                return View(con);
            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }


        public ActionResult Balancetes()
        {

            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {
                return View(db.Balancetes.Include(t => t.Apartamento).Where(t => t.ApeId == usuario.ApeId).ToList());
            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }


        }


        //pega o usuario logado e conta a quantidade de encomendas no Id (serve para fazer a notificacao)

        public ActionResult Encomendas()
        {
            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {

                Condomino con = db.Usuarios.OfType<Condomino>().Include(t => t.Encomendas).Where(t => t.Codigo == usuario.Codigo && t.ApeId == usuario.ApeId).FirstOrDefault();
            //    Funcoes.ContEncomenda(con.Encomendas.Count(x => x.Status == "Recebido").ToString());
                return View(con);
            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }

        //Visualiza as inadimplências do condomino logado

        public ActionResult Inadimplencias()
        {
            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {
                Condomino con = db.Usuarios.OfType<Condomino>().Include(t => t.Inadimplecia).Include(t => t.Apartamento).Where(t => t.Codigo == usuario.Codigo && t.ApeId == usuario.ApeId).FirstOrDefault();
              //  Funcoes.ContInadimplecia(con.Inadimplecia.Count.ToString());
                return View(con);
            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }

        //Lista as área de lazer para o condomino fazer as reservas

        public ActionResult Areas()
        {

            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {

                return View(db.AreaLazer.Include(a => a.Apartamento).Where(a => a.ApeId == usuario.ApeId).ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }


        //Lista as atas

        public ActionResult Atas()
        {
            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();

            if (usuario != null && usuario.Status == "Ativo")
            {

                return View(db.Ata.Include(a => a.Apartamento).Where(a => a.ApeId == usuario.ApeId).ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }


        // GET: Condomino
        public ActionResult Inicio()
        {

            Condomino usuario = AdminLar.Repositories.Funcoes.GetCondomino();


            if (usuario != null && usuario.Status == "Ativo")
            {

                ViewBag.Encomendas = db.Encomenda.Count(x => x.Codigo == usuario.Codigo && x.Status == "Recebido");

                ViewBag.Inadimplencia = db.Inadimplecias.Count(x => x.Codigo == usuario.Codigo);

                ViewBag.Visitantes = db.Visitantes.Count(x => x.Codigo == usuario.Codigo);

                ViewBag.Atas = db.Ata.Count(x => x.Ataid != null && x.ApeId == usuario.ApeId);

                DateTime expiro = DateTime.Now.AddDays(-1);

                return View(db.Avisos.Include(p => p.Apartamento).Where(p => p.ApeId == usuario.ApeId && expiro < p.DataAviso).OrderBy(t => t.DataAviso));

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");
            }

        }



        // GET: Condomino/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Condomino condomino = (Condomino)db.Usuarios.Find(id);
            if (condomino == null)
            {
                return HttpNotFound();
            }
            return View(condomino);
        }

        // GET: Condomino/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Condomino/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo,Email,Senha,Confirmasenha,Tipo,Con_Nome,Con_Cpf,Con_Sexo,Con_DataNascimento,Con_Telefone,Con_Celular,Con_NumeroApartamento,Status")] Condomino condomino)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(condomino);
                db.SaveChanges();
                return RedirectToAction("Inicio");
            }

            return View(condomino);
        }



        // GET: Condomino/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Condomino condomino = (Condomino)db.Usuarios.Find(id);
            if (condomino == null)
            {
                return HttpNotFound();
            }
            return View(condomino);
        }

        // POST: Condomino/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo,Email,Senha,Confirmasenha,Tipo,Con_Nome,Con_Cpf,Con_DataNascimento,Con_Telefone,Con_Celular,Con_NumeroApartamento,Status,ApeId")] Condomino condomino)
        {

             if (db.Sindicos.Any(x => x.Email == condomino.Email))
            {

                ViewBag.Error = "Email já cadastrado no sistema";

            }
            else if (db.Porteiros.Any(x => x.Email == condomino.Email))
            {

                ViewBag.Error = "Email já cadastrado no sistema";

            }

            else if (db.Porteiros.Any(x => x.Por_Cpf == condomino.Con_Cpf))
            {

                ViewBag.Error = "CPF já existente";

            }

            else if (db.Sindicos.Any(x => x.Sin_Cpf == condomino.Con_Cpf))
            {

                ViewBag.Error = "CPF já existente";

            }

            else if (db.Sindicos.Any(x => x.Sin_NumeroApartamento == condomino.Con_NumeroApartamento))
            {

                ViewBag.Error = "Numero do apartamento já existente";


            }

            else
            {

                if (ModelState.IsValid)
                {

                    db.Entry(condomino).State = EntityState.Modified;

                    db.SaveChanges();

                    ViewBag.Success = "Dados editados com sucesso";

                }

                //return RedirectToAction("Inicio");
            }

            return View(condomino);
        }

        // GET: Condomino/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Condomino condomino = (Condomino)db.Usuarios.Find(id);
            if (condomino == null)
            {
                return HttpNotFound();
            }
            return View(condomino);
        }

        // POST: Condomino/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Condomino condomino = (Condomino)db.Usuarios.Find(id);
            db.Usuarios.Remove(condomino);
            db.SaveChanges();
            return RedirectToAction("Inicio");
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

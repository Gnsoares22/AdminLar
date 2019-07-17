using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLar.Models;
using AdminLar.Repositories;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using PI_4º_Semestre.Repositories;

namespace PI_4º_Semestre.Controllers
{
    public class SindicosController : Controller
    {
        private Context db = new Context();


        public ActionResult Inicio()
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                var condomino = db.Usuarios.OfType<Condomino>().Count(x => x.Codigo != null && x.ApeId == usuario.ApeId);
                var sindico = db.Usuarios.OfType<Sindico>().Count(x => x.Codigo != null && x.ApeId == usuario.ApeId);
                var porteiro = db.Usuarios.OfType<Porteiro>().Count(x => x.Codigo != null && x.ApeId == usuario.ApeId);

                ViewBag.Ocorrencia = db.Ocorrencias.Count(x => x.ocoid != null && x.ApeId == usuario.ApeId);
                ViewBag.Atas = db.Ata.Count(x => x.Ataid != null && x.ApeId == usuario.ApeId);
                ViewBag.Inadimplencia = db.Inadimplecias.Count(x => x.InaId != null && x.ApeId == usuario.ApeId);

                ViewBag.Soma = condomino + sindico + porteiro;

                //itens para o grafico

                //    var monthNames = from d in
                //(
                //    from row in db.Ocorrencias.ToList() ).ToList();



                //var lista = db.Ocorrencias.OrderBy(t => t.datahoraocorrencia).GroupBy(t => t.datahoraocorrencia.Month).ToList();
                var lista = from ocor in db.Ocorrencias where ocor.ApeId == usuario.ApeId
                            group ocor by new
                            {
                                ocor.datahoraocorrencia.Year,
                                ocor.datahoraocorrencia.Month
                            }
                            into g
                            select new
                            {
                                Month = g.Select(n => n.datahoraocorrencia.Month.ToString()).FirstOrDefault(),
                                Quantity = g.Count()
                            };

                //grafico

                Highcharts columnChart = new Highcharts("columnchart");
                columnChart.InitChart(new Chart()
                {
                    Type = DotNet.Highcharts.Enums.ChartTypes.Column,
                    BackgroundColor = new BackColorOrGradient(System.Drawing.Color.White),
                    Style = "fontWeight: 'bold', fontSize: '13px'",
                    BorderColor = System.Drawing.Color.White,
                    BorderRadius = 0,
                    BorderWidth = 2
                });
                columnChart.SetTitle(new Title()
                {
                    Text = ""
                });
                columnChart.SetSubtitle(new Subtitle()
                {
                    Text = "Ocorrências"
                });

                string[] categorias = new string[lista.Count()];
                object[] dados = new object[lista.Count()];

                string[] meses = new string[] { "jan", "fev", "mar", "abr", "mai", "jun", "jul","ago","set","out","nov","dez" };

                int contador = 0;
                foreach (var item in lista)
                {
                    categorias[contador] = meses[Convert.ToInt32(item.Month.ToString()) - 1];
                    dados[contador] = item.Quantity;
                    //categorias[contador] = Convert.ToDateTime("2018-"+ item.Key.ToString()+"-01").ToString("MMMM");
                    //dados[contador] = item.Count();
                    contador = contador + 1;

                }

                columnChart.SetXAxis(new XAxis()
                {

                    Type = AxisTypes.Category,
                    Title = new XAxisTitle() { Text = "Meses", Style = "fontWeight: 'bold', fontSize: '17px'" },
                    Categories = categorias
                });
                columnChart.SetYAxis(new YAxis()
                {
                    Title = new YAxisTitle()
                    {
                        Text = "Quantidade de condôminos",
                        Style = "fontWeight: 'bold', fontSize: '17px'"
                    },
                    ShowFirstLabel = true,
                    ShowLastLabel = true,
                    Min = 0
                });
                columnChart.SetLegend(new Legend
                {
                    Enabled = true,
                    BorderColor = System.Drawing.Color.CornflowerBlue,
                    BorderRadius = 6,
                    BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFADD8E6"))
                });

                columnChart.SetSeries(new Series[]
                {
                new Series()
    {
        Name = "Número de Ocorrências",
                    Data = new Data(dados) //aqui vai os dados;
                }
}


                );

                return View(columnChart);

            }

            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }


        // GET: Sindicos
        public ActionResult Index()
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                //lista sindicos do condominio logado

                return View(db.Usuarios.OfType<Sindico>().Include(t => t.Apartamento).Where(t => t.ApeId == usuario.ApeId).ToList());

            }
            else

                return RedirectToAction("AcessoNegado", "Landing");
        }


        public ActionResult Ocorrencias()
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                //lista sindicos do condominio logado

                return View(db.Ocorrencias.Include(t => t.Apartamento).Where(t => t.ApeId == usuario.ApeId).ToList());

            }
            else

                return RedirectToAction("AcessoNegado", "Landing");
        }


        //Lista os porteiros na pagina do sindico

        public ActionResult ListaPorteiros()
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                return View(db.Usuarios.OfType<Porteiro>().Include(t => t.Apartamento).Where(t => t.ApeId == usuario.ApeId).ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }


        //Lista todos condominos 

        public ActionResult ListaCondominos()
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                return View(db.Usuarios.OfType<Condomino>().Include(t => t.Apartamento).Where(t => t.ApeId == usuario.ApeId).ToList());

            }
            else
            {

                return RedirectToAction("AcessoNegado", "Landing");

            }

        }



        // Cadastra Porteiros no sistema via Json

        [HttpPost]
        public ActionResult SalvarPorteiros(string email, string nome, string cpf, string endereco, string data, string telefone, string celular)
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {

                if (db.Usuarios.Any(x => x.Email == email))
                {

                    ViewBag.Error = "Email já cadastrado no sistema";

                }
                else if (db.Porteiros.Any(x => x.Por_Cpf == cpf))
                {

                    ViewBag.Error = "CPF já existente";

                }
                else if (db.Condominos.Any(x => x.Con_Cpf == cpf))
                {

                    ViewBag.Error = "CPF já existente";

                }
                else if (db.Sindicos.Any(x => x.Sin_Cpf == cpf))
                {

                    ViewBag.Error = "CPF já existente";

                }

                else
                {

                    Porteiro porteiro = new Porteiro();


                    porteiro.Email = email;
                    porteiro.Por_Nome = nome;
                    porteiro.Tipo = "P";
                    porteiro.Por_Cpf = cpf;
                    porteiro.Por_Endereco = endereco;
                    porteiro.Por_DataNascimento = data;
                    porteiro.Por_Telefone = telefone;
                    porteiro.Por_Celular = celular;
                    porteiro.ApeId = usuario.ApeId;


                    db.Porteiros.Add(porteiro);

                    var callbackUrl = Url.Action("CadastrarSenhaP", "Landing", new { mandaemail = Funcoes.Base64Codifica(email) }, protocol: Request.Url.Scheme);


                    GmailEmailService gmail = new GmailEmailService();
                    EmailMessage msg = new EmailMessage();
                    msg.Body = "Bem vindo Confirme a sua senha clicando aqui: " + callbackUrl;
                    msg.IsHtml = false;
                    msg.Subject = "Bem vindo a AdminLar (Confirmação de Cadastro)";
                    msg.ToEmail = email;
                    gmail.SendEmailMessage(msg);


                    if (db.SaveChanges() > 0)
                    {

                        return Json(true);

                    }

                }
            }

            return Json(false);
        }


        //envia link de confirmação de conta

        //public ActionResult EnviaLink(int ide, string email)
        //{

        //    var callbackUrl = Url.Action("CadastrarSenhaP", "Landing", new { identifica = Funcoes.Base64Codifica(ide.ToString()) }, protocol: Request.Url.Scheme);


        //    GmailEmailService gmail = new GmailEmailService();
        //    EmailMessage msg = new EmailMessage();
        //    msg.Body = "Bem vindo Confirme a sua senha clicando aqui: " + callbackUrl;
        //    msg.IsHtml = false;
        //    msg.Subject = "Bem vindo a AdminLar (Confirmação de Cadastro)";
        //    msg.ToEmail = email;
        //    gmail.SendEmailMessage(msg);

        //    return RedirectToAction("ListaPorteiros");

        //}


        //public ActionResult EnviaLinkC(int ide, string email)
        //{

        //    var callbackUrl = Url.Action("CadastrarSenhaC", "Landing", new { iden = Funcoes.Base64Codifica(ide.ToString()) }, protocol: Request.Url.Scheme);


        //    GmailEmailService gmail = new GmailEmailService();
        //    EmailMessage msg = new EmailMessage();
        //    msg.Body = "Bem vindo Confirme a sua senha clicando aqui: " + callbackUrl;
        //    msg.IsHtml = false;
        //    msg.Subject = "Bem vindo a AdminLar (Confirmação de Cadastro)";
        //    msg.ToEmail = email;
        //    gmail.SendEmailMessage(msg);

        //    return RedirectToAction("ListaCondominos");

        //}


        //Cadastra condominos no sistema via Json

        [HttpPost]
        public ActionResult Condominos(string email, string nome, string cpf, int numeroape, string data, string telefone, string celular)
        {

            Sindico usuario = AdminLar.Repositories.Funcoes.GetUsuario();

            if (usuario != null)
            {


                if (db.Usuarios.Any(x => x.Email == email))
                {

                    ViewBag.Error = "Email já cadastrado no sistema";

                }

                else if (db.Porteiros.Any(x => x.Por_Cpf == cpf))
                {

                    ViewBag.Error = "CPF já existente";

                }
                else if (db.Condominos.Any(x => x.Con_Cpf == cpf))
                {

                    ViewBag.Error = "CPF já existente";

                }
                else if (db.Sindicos.Any(x => x.Sin_Cpf == cpf))
                {

                    ViewBag.Error = "CPF já existente";

                }
                else if (db.Condominos.Any(x => x.Con_NumeroApartamento == numeroape))
                {

                    ViewBag.Error = "Numero do apartamento já existente";


                }
                else if (db.Sindicos.Any(x => x.Sin_NumeroApartamento == numeroape))
                {

                    ViewBag.Error = "Numero do apartamento já existente";


                }

                else
                {

                    Condomino condomino = new Condomino();


                    condomino.Email = email;
                    condomino.Con_Nome = nome;
                    condomino.Tipo = "C";
                    condomino.Con_Cpf = cpf;
                    condomino.Con_NumeroApartamento = numeroape;
                    condomino.Con_DataNascimento = data;
                    condomino.Con_Telefone = telefone;
                    condomino.Con_Celular = celular;
                    condomino.Status = "Ativo";
                    condomino.ApeId = usuario.ApeId;


                    var callbackUrl = Url.Action("CadastrarSenhaC", "Landing", new { mandaemail = Funcoes.Base64Codifica(email) }, protocol: Request.Url.Scheme);


                    GmailEmailService gmail = new GmailEmailService();
                    EmailMessage msg = new EmailMessage();
                    msg.Body = "Bem vindo Confirme a sua senha clicando aqui: " + callbackUrl;
                    msg.IsHtml = false;
                    msg.Subject = "Bem vindo a AdminLar (Confirmação de Cadastro)";
                    msg.ToEmail = email;
                    gmail.SendEmailMessage(msg);

                    db.Condominos.Add(condomino);

                    if (db.SaveChanges() > 0)
                    {

                        return Json(true);

                    }

                }
            }

            return Json(false);

        }


        // Inativar condominos

        [HttpPost]
        public ActionResult status(string codigo, string status)
        {


            Condomino com = db.Condominos.Find(Convert.ToInt32(codigo));

            //  com.Codigo =  id;

            com.Status = status;

            db.Entry(com).State = EntityState.Modified;

            if (db.SaveChanges() > 0)
            {

                return Json(true);
            }

            return Json(false);

        }


        // GET: Sindicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sindico sindico = (Sindico)db.Usuarios.Find(id);
            if (sindico == null)
            {
                return HttpNotFound();
            }
            return View(sindico);
        }

        // GET: Sindicos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sindicos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo,Email,Senha,Tipo,Confirmasenha,Sin_Nome,Sin_Cpf,Sin_DataNascimento,Sin_Telefone,Sin_Celular,Sin_NumeroApartamento")] Sindico sindico)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(sindico);
                db.SaveChanges();
                ViewBag.Success = "Dados salvos com sucesso";
                //return RedirectToAction("Index");
            }

            return View(sindico);
        }

        // GET: Sindicos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sindico sindico = (Sindico)db.Usuarios.Find(id);
            if (sindico == null)
            {
                return HttpNotFound();
            }
            return View(sindico);
        }

        // POST: Sindicos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo,Email,Senha,Confirmasenha,Tipo,Sin_Nome,Sin_Cpf,Sin_DataNascimento,Sin_Telefone,Sin_Celular,Sin_NumeroApartamento,ApeId")] Sindico sindico)
        {

            if (db.Porteiros.Any(x => x.Email == sindico.Email))
            {

                ViewBag.Error = "Email já cadastrado no sistema";

            }
            else if (db.Condominos.Any(x => x.Email == sindico.Email))
            {

                ViewBag.Error = "Email já cadastrado no sistema";

            }

            else if (db.Condominos.Any(x => x.Con_Cpf == sindico.Sin_Cpf))
            {

                ViewBag.Error = "CPF já existente";

            }

            else if (db.Porteiros.Any(x => x.Por_Cpf == sindico.Sin_Cpf))
            {

                ViewBag.Error = "CPF já existente";

            }
            else
            {

                if (ModelState.IsValid)
                {
                    db.Entry(sindico).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Success = "Dados editados com sucesso";
                    // return RedirectToAction("Inicio");
                }

            }
            return View(sindico);
        }



        // GET: Sindicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sindico sindico = (Sindico)db.Usuarios.Find(id);
            if (sindico == null)
            {
                return HttpNotFound();
            }
            return View(sindico);
        }

        // POST: Sindicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sindico sindico = (Sindico)db.Usuarios.Find(id);
            db.Usuarios.Remove(sindico);
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

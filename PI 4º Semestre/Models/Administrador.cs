using AdminLar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PI_4º_Semestre.Models
{
    public class Administrador:Usuario
    {
        //administrador irá herdar os mesmos atributos do usuario portanto ele não terá atributos relevantes exceto

        public string AdmNome { get; set; }
    }
}
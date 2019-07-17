using PI_4º_Semestre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLar.Models
{
    public class Inadimplencia
    {
        [Key]
        public int InaId { get; set; }

        [DisplayName("Valor da pendência ")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public string valor { get; set; }

        public string Status { get; set; }

        public int Codigo { get; set; }

        public virtual Condomino Condomino { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }

    }
}
using PI_4º_Semestre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLar.Models
{
    public class Balancetes
    {

        [Key]
        public int Balid { get; set; }

        [DisplayName("Data")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataBal { get; set; }

        [DisplayName("Informações do Balancete")]
        public string Descricao { get; set; }

        [DisplayName("Arquivo do Balancete")]
        public string Arquivo { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }

    }
}
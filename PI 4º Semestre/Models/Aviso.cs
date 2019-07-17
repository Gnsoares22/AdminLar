using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PI_4º_Semestre.Models
{
    public class Aviso
    {

        [Key]
        public int Idaviso { get; set; }

        [DisplayName("Data do aviso")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataAviso { get; set; }

        [DisplayName("Descrição do Aviso")]
        [Required(ErrorMessage = "Preencha o campo descrição do aviso")]
        public string Descricao { get; set; }

        [DisplayName("Data de Expiração")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AvisoExpiro { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }

    }
}
using AdminLar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PI_4º_Semestre.Models
{
    public class Visitante
    {

        [Key]
        public int CodigoVisita { get; set; }

        [DisplayName("Número do Rg do visitante ")]
        [Required]
        public string NumeroRg { get; set; }

        [DisplayName("Nome do visitante ")]
        [Required]
        public string NomeVisitante { get; set; }

        [DisplayName("Número do Apartamento que irá visitar ")]
        [Required]
        public int NumeroApartamentoVisita { get; set; }

        [DisplayName("Data da Visita")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string DataVisita { get; set; }

        [DisplayName("Visitando o Condômino ")]
        [Required]
        public int Codigo { get; set; }

        [DisplayName("Visitando o Condômino ")]
        public virtual Condomino Condomino { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }

    }
}
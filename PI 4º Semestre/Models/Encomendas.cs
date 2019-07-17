using PI_4º_Semestre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLar.Models
{
    public class Encomendas
    {
        [Key]
        public int EncoId { get; set; }

        [DisplayName("Descrição da Encomenda")]
        public string Descricao { get; set; }

        [DisplayName("Status da Encomenda")]
        public string Status { get; set; }

        [DisplayName("Data de recebimento da encomenda")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataEntrega { get; set; }

        [DisplayName("Encomenda para o apartamento do condômino número")]
        public int Codigo { get; set; }

        public virtual Condomino Condomino { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }


    }
}
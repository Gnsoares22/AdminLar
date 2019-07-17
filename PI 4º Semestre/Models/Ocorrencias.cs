using PI_4º_Semestre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLar.Models
{
    public class Ocorrencias
    {
        [Key]
        public int ocoid { get; set; }

        [DisplayName("Data e hora da ocorrência")]
        public DateTime datahoraocorrencia { get; set; }

        [DisplayName("Descrição da ocorrência")]
        public string Descricao { get; set; }

        [DisplayName("Nome do condômino")]
        public string CondoNome { get; set; }

        [DisplayName("Status ocorrência")]
        public string Status { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }

        public int Codigo { get; set; }

        public virtual Condomino Condomino { get; set; }

    }
}
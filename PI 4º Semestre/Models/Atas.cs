using PI_4º_Semestre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLar.Models
{
    public class Atas
    {

        [Key]
        public int Ataid { get; set; }

        [DisplayName("Data")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        public DateTime DataAta { get; set; }

        [DisplayName("Descrição da ata")]
        public string Descricao { get; set; }

        [DisplayName("Arquivo da Ata")]
        public string Arquivo { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }

    }
}
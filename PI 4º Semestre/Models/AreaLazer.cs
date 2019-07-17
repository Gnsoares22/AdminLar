using AdminLar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PI_4º_Semestre.Models
{
    public class AreaLazer
    {

        [Key]
        public int Areaid { get; set; }

        [DisplayName("Nome da área")]
        [Required(ErrorMessage = "Preencha o campo Nome da Área")]
        public string NomeArea { get; set; } 
            
        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Preencha o campo descricao")]
        public string descricao { get; set; }

        [DisplayName("Regras")]
        [Required(ErrorMessage = "Preencha o campo regras da sala")]
        public string Regras { get; set; }

        public virtual ICollection<Reserva> HistoricoReserva { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }
    }
}
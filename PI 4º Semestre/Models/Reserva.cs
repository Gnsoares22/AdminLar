using PI_4º_Semestre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLar.Models
{
    public class Reserva
    {

        [Key]
        public int ReservaId { get; set; }

        [DisplayName("Data da Reserva ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataReserva { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        public int Codigo { get; set; }

        public int Areaid { get; set; }

        public virtual AreaLazer AreaLazer { get; set; }

        public virtual Condomino Condomino { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }

    }
}
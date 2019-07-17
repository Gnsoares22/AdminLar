using AdminLar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PI_4º_Semestre.Models
{
    public class Apartamento
    {

        [Key]
        public int ApeId { get; set; }

        [DisplayName("Condomínio")]
        public string NomeApe { get; set; }

        [DisplayName("Endereço")]
        public string Endereco { get; set; }


        //no apartamento tem uma coleção de : 

        public virtual ICollection<Sindico> Sindicos { get; set; }

        public virtual ICollection<Porteiro> Porteiros { get; set; }

        public virtual ICollection<Condomino> Condominos { get; set; }

        public virtual ICollection<Aviso> Avisos { get; set; }

        public virtual ICollection<AreaLazer> AreaLazer { get; set; }

        public virtual ICollection<Atas> Atas { get; set; }

        public virtual ICollection<Visitante> Visita { get; set; }

        public virtual ICollection<Encomendas> Encomenda { get; set; }

        public virtual ICollection<Inadimplencia> Inadimplencia { get; set; }

        public virtual ICollection<Reserva> HistoricoReserva { get; set; }

        public virtual ICollection<Ocorrencias> Ocorrencias { get; set; }

    }
}
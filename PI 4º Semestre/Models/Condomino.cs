using PI_4º_Semestre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLar.Models
{
  
    public class Condomino : Usuario
    {

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Preencha o campo Nome")]
        public string Con_Nome { get; set; }

        [DisplayName("CPF")]
        [Required(ErrorMessage = "Preencha o campo CPF")]
        [RegularExpression(@"[0-9.-]{14}", ErrorMessage = "CPF Inválido")]
        public string Con_Cpf { get; set; }

        [DisplayName("Data de Nascimento")]
        [Required(ErrorMessage = "Preencha o campo data")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string Con_DataNascimento { get; set; }

        [DisplayName("Telefone")]
        [Required(ErrorMessage = "Preencha o campo telefone")]
        public string Con_Telefone { get; set; }

        [DisplayName("Celular")]
        [Required(ErrorMessage = "Preencha o campo celular")]
        public string Con_Celular { get; set; }

        [DisplayName("Número do Apartamento")]
        [Range(1, 999, ErrorMessage = "Digite um numero válido de apartamento")]
        public int Con_NumeroApartamento { get; set; }

        [DisplayName("Status no Condômino")]
        public string Status { get; set; }


        public virtual ICollection<Visitante> Visitantes { get; set; }

        public virtual ICollection<Encomendas> Encomendas { get; set; }

        public virtual ICollection<Inadimplencia> Inadimplecia { get; set; }

        public virtual ICollection<Ocorrencias> Ocorrencias { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }

    }
}
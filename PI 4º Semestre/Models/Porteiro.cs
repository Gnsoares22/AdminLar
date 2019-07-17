using PI_4º_Semestre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLar.Models
{
    public class Porteiro : Usuario
    {

        [DisplayName("Nome")]
        public string Por_Nome { get; set; }

        [DisplayName("CPF")]
        [Required]
        [RegularExpression(@"[0-9.-]{14}", ErrorMessage = "CPF Inválido")]
        public string Por_Cpf { get; set; }

        [DisplayName("Data de Nascimento")]
        [Required(ErrorMessage = "Preencha o campo data")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string Por_DataNascimento { get; set; }

        [DisplayName("Endereço")]
        [Required(ErrorMessage = "Preencha o campo endereço")]
        public string Por_Endereco { get; set; }

        [DisplayName("Telefone")]
        [Required(ErrorMessage = "Preencha o campo telefone")]
        public string Por_Telefone { get; set; }

        [DisplayName("Celular")]
        [Required(ErrorMessage = "Preencha o campo celular")]
        public string Por_Celular { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }
    }
}
using PI_4º_Semestre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace AdminLar.Models
{
    public class Sindico : Usuario
    {
        [DisplayName("Nome")]
        [Required(ErrorMessage = "Preencha o campo nome")]
        public string Sin_Nome { get; set; }

        [DisplayName("CPF")]
        [Required(ErrorMessage = "Preencha o campo CPF")]
        [RegularExpression(@"[0-9.-]{14}", ErrorMessage = "CPF Inválido")]
        public string Sin_Cpf { get; set; }

        [DisplayName("Data de Nascimento")]
        [Required(ErrorMessage = "Preencha o campo data")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string Sin_DataNascimento { get; set; }

        [DisplayName("Telefone")]
        public string Sin_Telefone { get; set; }

        [DisplayName("Celular")]
        public string Sin_Celular { get; set; }

        [DisplayName("Número do Apartamento")]
        [Range(1,999, ErrorMessage = "Digite um número válido de apartamento")]
        public int Sin_NumeroApartamento { get; set; }

        public int ApeId { get; set; }

        public virtual Apartamento Apartamento { get; set; }

    }
}
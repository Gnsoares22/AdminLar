using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace AdminLar.Models
{
    public class Usuario
    {
        [Key]
        public int Codigo { get; set; }

        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Email no formato incorreto")]
        [Required(ErrorMessage = "Preencha o campo email")]
        public string Email { get; set; }

        [DisplayName("Senha")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DisplayName("Confirme a senha")]
        [DataType(DataType.Password)]
        public string Confirmasenha { get; set; }


        public string Tipo { get; set; }


    }
}
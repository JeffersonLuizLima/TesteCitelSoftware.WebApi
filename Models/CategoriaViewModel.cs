using System.ComponentModel.DataAnnotations;

namespace TesteCitelSoftware.WebApi.Models
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(80, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Descricao { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace TesteCitelSoftware.WebApi.Models
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(60, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Preço")]
        public decimal Valor { get; set; }

        public virtual CategoriaViewModel Categoria { get; set; }
        public virtual int CategoriaId { get; set; }
    }
}
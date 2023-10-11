using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SustentaMais.Model
{
    public class Produto
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        // nome
        [Column(TypeName = "Varchar")]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Column(TypeName = "Varchar")]
        [StringLength(1000)]
        public string Descricao { get; set; } = string.Empty;

        [Column(TypeName = " Decimal (7,2) ")]
        public decimal Preco { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(5000)]
        public string Foto { get; set; } = string.Empty;


        public virtual Categoria? Categoria { get; set; }

        public virtual User? User { get; set; }



    }
}









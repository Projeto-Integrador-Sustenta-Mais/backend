using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SustentaMais.Model
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }


        [Column(TypeName = "Varchar")]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;


        [Column(TypeName = "Varchar")]
        [StringLength(50)]
        public string Usuario { get; set; } = string.Empty;


        [Column(TypeName = "Varchar")]
        [StringLength(255)]
        public string Senha { get; set; } = string.Empty;


        [Column(TypeName = "Varchar")]
        [StringLength(5000)]
        public string? Foto { get; set; } = string.Empty;


        [InverseProperty("User")]
        public virtual ICollection<Produto>? Produto { get; set; }


    }
}





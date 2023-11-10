using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SustentaMais.Model
{
    public class Categoria
    {
        [Key] // PRIMARY kEY (Id)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // IDENTITY(1,1)
        public long Id { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(255)]
        public string Tipo{ get; set; } = string.Empty;

        [Column(TypeName = "boolean")]
        public Boolean Disponivel{ get; set; }

        [InverseProperty("Categoria")]
        public virtual ICollection<Produto>? Produto { get; set; }

    }
}

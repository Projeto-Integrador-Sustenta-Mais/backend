using SustentaMais.Model;

namespace SustentaMais.Service
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> GetAll(); 

        Task<Produto?> GetById(long id); 
        Task<IEnumerable<Produto>> GetByNome(string nome);
       
        Task<Produto?> Create(Produto produto); 

        Task<Produto?> Update(Produto produtos);

        Task Delete(Produto produto);


    }
}
       





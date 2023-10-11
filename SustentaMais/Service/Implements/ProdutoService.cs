using Microsoft.EntityFrameworkCore;
using SustentaMais.Data;
using SustentaMais.Model;

namespace SustentaMais.Service.Implements
{
    public class ProdutoService : IProdutoService
    {
        private readonly AppDbContext _context;

        public ProdutoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> GetAll()
        {
            return await _context.Produtos
              .Include(p => p.User)
              .Include(p => p.Categoria)
              .ToListAsync();
        }
       
        public async Task<Produto?> GetById(long id)
        {
            try 
            {
                var Produto = await _context.Produtos
                    .Include(p => p.User)
                    .Include(p => p.Categoria)
                    .FirstAsync(p => p.Id == id); 
                return Produto;

            }
            catch
            {
                return null;
            }
        }
       
        public async Task<IEnumerable<Produto>> GetByNome(string nome)
        {
            var Produtos = await _context.Produtos
                             .Include(p => p.Categoria)
                             .Include(p => p.User)
                             .Where(n => n.Nome.Contains(nome)) 
                             .ToListAsync(); 

            return Produtos;
        }

        public async Task<Produto?> Create(Produto produto)
        {
            if (produto.Categoria is not null)
            {
                var BuscaCategoria = await _context.Categorias.FindAsync(produto.Categoria.Id);

                if (BuscaCategoria is null)
                    return null;
            }

            produto.Categoria = produto.Categoria is not null ? _context.Categorias.FirstOrDefault(p => p.Id == produto.Categoria.Id) : null;

            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();

            return produto;


        }
        

        public async Task<Produto?> Update(Produto produtos)
        {
            var ProdutoUpdate = await _context.Produtos.FindAsync(produtos.Id);

            if (ProdutoUpdate is null) 
                return null;

            if (produtos.Categoria is not null)
            {
                var Buscaproduto = await _context.Produtos.FindAsync(produtos.Categoria.Id);
                if (Buscaproduto is null)
                    return null;

            }

            produtos.Categoria = produtos.Categoria is not null ? _context.Categorias.FirstOrDefault(t => t.Id == produtos.Categoria.Id) : null; // verificação

            _context.Entry(ProdutoUpdate).State = EntityState.Detached; 
            _context.Entry(produtos).State = EntityState.Modified; 
            await _context.SaveChangesAsync(); 
            return produtos;
        }
        

        public async Task Delete(Produto produto)
        {
            _context.Remove(produto);
            await _context.SaveChangesAsync();
        }

       
    }
}

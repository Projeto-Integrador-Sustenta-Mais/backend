using Microsoft.EntityFrameworkCore;
using SustentaMais.Data;
using SustentaMais.Model;

namespace SustentaMais.Service.Implements
{
    public class CategoriaService : ICategoriaService
    {
        private readonly AppDbContext _context;

        public CategoriaService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Categoria?>> GetAll()
        {
           return await _context.Categorias.ToListAsync();
        }

           public async Task<Categoria?> GetById(long id)
        {
          try
          {
            var BuscarId = await _context.Categorias.FirstAsync(c => c.Id == id);
            return BuscarId;
          }
          catch 
          {
            return null;
          }
            
        }

         public async Task<IEnumerable<Categoria?>> GetByTipo(string tipo)
        {
            try
          {
            var BuscarTipo = await _context.Categorias
                                    .Where(c => c.Tipo.Contains(tipo))
                                    .ToListAsync();
            return BuscarTipo;
          }
          catch 
          {
            return null!;
          }
        }

        public async Task<Categoria?> Create(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }
        public async Task<Categoria?> Update(Categoria categoria)
        {
            var atualizarCategoria = await _context.Categorias.FindAsync(categoria.Id);
            
            if (atualizarCategoria is null)
            {
                return null;
            }

            _context.Entry(atualizarCategoria).State = EntityState.Detached;
            _context.Entry(categoria).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task Delete(Categoria categoria)
        {
            _context.Remove(categoria);
            await _context.SaveChangesAsync();
        }

    }
}
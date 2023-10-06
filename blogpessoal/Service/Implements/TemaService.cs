using blogpessoal.Data;
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Service.Implements
{
    public class TemaService : ITemaService
    {
        public readonly AppDbContext _context;

        public TemaService(AppDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Tema>> GetAll()
        {
            return await _context.Temas
                 .Include(t => t.Postagem)
                 .ToListAsync();
        }

        public async Task<Tema?> GetById(long id)
        {
            try
            {
                var Tema = await _context.Temas
                     .Include(t => t.Postagem)
                     .FirstAsync(t => t.Id == id);

                return Tema;
            }
            catch
            {
                return null;
            }

        }

        public async Task<IEnumerable<Tema>> GetByDescricao(string descricao)
        {
            var Tema = await _context.Temas
                .Include(t => t.Postagem)
                .Where(t => t.Descricao.Contains(descricao))
                .ToListAsync();

            return Tema;
        }

        public async Task<Tema> Create(Tema tema)
        {
            _context.Temas.Add(tema);
            await _context.SaveChangesAsync();

            return tema;
        }

        public async Task<Tema?> Update(Tema tema)
        {

            var TemaUpdate = await _context.Temas.FindAsync(tema.Id);

            if (TemaUpdate == null)
                return null;

            _context.Entry(TemaUpdate).State = EntityState.Detached;
            _context.Entry(tema).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return tema;
        }

        public async Task Delete(Tema tema)
        {

            _context.Temas.Remove(tema);
            await _context.SaveChangesAsync();

        }

    }
}

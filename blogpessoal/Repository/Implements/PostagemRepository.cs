using blogpessoal.Data;
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Repository.Implements
{
    public class PostagemRepository : IPostagemRepository
    {

        public readonly AppDbContext _context;

        public PostagemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Postagem>> GetAll()
        {
            return await _context.Postagens.ToListAsync();
        }

        public Task<Postagem?> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Postagem>> GetByTitulo(string titulo)
        {
            throw new NotImplementedException();
        }

        public Task<Postagem?> Create(Postagem postagem)
        {
            throw new NotImplementedException();
        }

        public Task<Postagem?> Update(Postagem postagem)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Postagem postagem)
        {
            throw new NotImplementedException();
        }

    }
}


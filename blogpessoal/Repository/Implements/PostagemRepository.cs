using blogpessoal.Model;

namespace blogpessoal.Repository.Implements
{
    public class PostagemRepository : IPostagemRepository
    {


        public Task<IEnumerable<Postagem>> GetAll()
        {
            throw new NotImplementedException();
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


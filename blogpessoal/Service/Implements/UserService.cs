using blogpessoal.Data;
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Service.Implements
{
    public class UserService : IUserService
    {
        public readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .Include(u => u.Postagem)
                .ToListAsync();
        }

        public async Task<User?> GetById(long id)
        {
            try
            {
                var Usuario = await _context.Users
                    .Include(u => u.Postagem)
                    .FirstAsync(u => u.Id == id);

                Usuario.Senha = "";

                return Usuario;
            }
            catch
            {
                return null;
            }

        }

        public async Task<User?> GetByUsuario(string usuario)
        {
            try
            {
                var BuscaUsuario = await _context.Users
                    .Include(u => u.Postagem)
                    .Where(u => u.Usuario == usuario)
                    .FirstOrDefaultAsync();

                return BuscaUsuario;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> Create(User usuario)
        {
            var BuscaUsuario = await GetByUsuario(usuario.Usuario);

            if (BuscaUsuario is not null)
                return null;

            if (usuario.Foto is null || usuario.Foto == "")
                usuario.Foto = "https://i.imgur.com/I8MfmC8.png";

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, workFactor: 10);

            _context.Users.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<User?> Update(User usuario)
        {

            var UsuarioUpdate = await _context.Users.FindAsync(usuario.Id);

            if (UsuarioUpdate is null)
                return null;

            if (usuario.Foto is null || usuario.Foto == "")
                usuario.Foto = "https://i.imgur.com/I8MfmC8.png";

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, workFactor: 10);

            _context.Entry(UsuarioUpdate).State = EntityState.Detached;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return usuario;
        }

    }
}

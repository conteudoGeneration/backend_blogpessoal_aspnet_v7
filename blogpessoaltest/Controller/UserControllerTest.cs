using blogpessoal.Model;
using blogpessoal.Security;
using blogpessoaltest.Helper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Xunit.Extensions.Ordering;

namespace blogpessoaltest.Controllers
{

    [Order(1)]
    public class UserControllerTest : IClassFixture<WebAppFactory<Program>>
    {

        private readonly HttpClient _client;

        private string Id { get; set; } = string.Empty;

        public UserControllerTest(WebAppFactory<Program> factory)
        {
            
            _client = factory.CreateClient();

        }

        public static string GerarFakeToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor

            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "root@root.com.br")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), 
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return "Bearer " + tokenHandler.WriteToken(token).ToString();

        }

        [Fact, Order(1)]
        public async Task DeveCriarNovoUsuario()
        {

            var novoUsuario = new Dictionary<string, string>
            {
                { "nome", "Root" },
                { "usuario", "root@root.com.br" },
                { "senha", "rootroot" },
                { "foto", "-" }
            };

            var corpoRequisicao = JsonConvert.SerializeObject(novoUsuario);

            var requisicaoPost = new HttpRequestMessage(HttpMethod.Post, "/usuarios/cadastrar")
            {
                Content = new StringContent(
                corpoRequisicao,
                Encoding.UTF8,
                "application/json"
                )
            };

            var respostaPost = await _client.SendAsync(requisicaoPost);

            Assert.Equal(201, (int)respostaPost.StatusCode);

        }

        [Fact, Order(2)]
        public async Task NaoDeveCriarUsuarioDuplicado()
        {
            var novoUsuario = new Dictionary<string, string>
            {
                { "nome", "Juliana Andrews" },
                { "usuario", "juliana@email.com.br" },
                { "senha", "12345678" },
                { "foto", "-" }
            };

            var corpoRequisicao = JsonConvert.SerializeObject(novoUsuario);

            //Enviar a primeira vez

            var requisicaoPost = new HttpRequestMessage(HttpMethod.Post, "/usuarios/cadastrar")
            {
                Content = new StringContent(
                corpoRequisicao,
                Encoding.UTF8,
                "application/json"
                )
            };

            await _client.SendAsync(requisicaoPost);

            //Enviar a segunda vez

            var requisicaoPostDuplicada = new HttpRequestMessage(HttpMethod.Post, "/usuarios/cadastrar")
            {
                Content = new StringContent(
                corpoRequisicao,
                Encoding.UTF8,
                "application/json"
                )
            };

            var respostaPost = await _client.SendAsync(requisicaoPostDuplicada);

            Assert.Equal(400, (int)respostaPost.StatusCode);

        }

        [Fact, Order(3)]
        public async Task DeveAtualizarUsuario()
        {
            var criarUsuario = new Dictionary<string, string>
            {
                { "nome", "Paulo Antunes" },
                { "usuario", "paulo@email.com.br" },
                { "senha", "12345678" },
                { "foto", "-" }
            };

            var corpoRequisicaoCriar = JsonConvert.SerializeObject(criarUsuario);

            //Criar usuário

            var requisicaoPost = new HttpRequestMessage(HttpMethod.Post, "/usuarios/cadastrar")
            {
                Content = new StringContent(
                corpoRequisicaoCriar,
                Encoding.UTF8,
                "application/json"
                )
            };

            var respostaPost = await _client.SendAsync(requisicaoPost);

            var corpoRespostaPost = await respostaPost.Content.ReadFromJsonAsync<User>();

            if (corpoRespostaPost is not null)
            {
                Id = corpoRespostaPost.Id.ToString();
            }

            //Atualizar Usuário

            var atualizarUsuario = new Dictionary<string, string>
            {
                { "id", Id },
                { "nome", "Paulo Cesar Antunes" },
                { "usuario", "paulo_cesar@email.com.br" },
                { "senha", "12345678" },
                { "foto", "-" }
            };

            var corpoRequisicaoAtualizar = JsonConvert.SerializeObject(atualizarUsuario);

            var requisicaoPut = new HttpRequestMessage(HttpMethod.Put, "/usuarios/atualizar")
            {
                Content = new StringContent(
                corpoRequisicaoAtualizar,
                Encoding.UTF8,
                "application/json"
                )
            };

            _client.DefaultRequestHeaders.Add("Authorization", GerarFakeToken());
            var respostaPut = await _client.SendAsync(requisicaoPut);

            Assert.Equal(200, (int)respostaPut.StatusCode);

        }

        [Fact, Order(4)]
        public async Task DeveListarTodosOsUsuarios()
        {

            _client.DefaultRequestHeaders.Add("Authorization", GerarFakeToken());
            var respostaGet = await _client.GetAsync("/usuarios/all");

            Assert.Equal(200, (int)respostaGet.StatusCode);

        }

    }

}
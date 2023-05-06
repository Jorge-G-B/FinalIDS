using FINALIDS_1220019.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FINALIDS_1220019.Controllers
{


    [ApiController]
    [Route("Votacion")]
    public class VotacionController : Controller
    {
        private readonly IConfiguration configuration;

        public VotacionController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        static int Fase = 1;


        [Route("Login")]
        [HttpPost]
        public Token Login(Usuario user)
        {
            FinalIdsContext _VotacionContext = new FinalIdsContext();
            Usuario usuario = (from users in _VotacionContext.Usuarios
                                          where users.Usuario1 == user.Usuario1
                                          select users).FirstOrDefault();
            Token tokenresult = new Token();
            if (usuario.Contraseña == user.Contraseña)
            {
                string applicationName = "FINALIDS_1220019";
                tokenresult.expirationTime = DateTime.Now.AddMinutes(30);
                tokenresult.token = CustomTokenJWT(applicationName, tokenresult.expirationTime);
            }
            return tokenresult;
        }

        private string CustomTokenJWT(string ApplicationName, DateTime token_expiration)

        {

            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );

            var _Header = new JwtHeader(_signingCredentials);
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, ApplicationName),
                new Claim("Name", "nombrepesrsona")
            };

            var _Payload = new JwtPayload(

                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.Now,
                    expires: token_expiration
                );

            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);

        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("CreateCandidato")]
        public async Task<ActionResult<Candidato>> Create(Candidato candidato)
        {

            FinalIdsContext _VotacionContext = new FinalIdsContext();
            if (Fase == 1)
            {
                List<Candidato> candidatos = (from cand in _VotacionContext.Candidatos
                                              where cand.Nombre == candidato.Nombre && cand.Dpi == candidato.Dpi
                                              select cand).ToList();
                if (candidatos.Count() > 0)
                {
                    return BadRequest("El candidato ya existe");
                }
                _VotacionContext.Candidatos.Add(candidato);
                await _VotacionContext.SaveChangesAsync();
                return Ok(candidato);
            }
            else
            {
                return BadRequest("El sistema ya no acepta candidatos nuevos");
            }

        }

        [HttpPost("Votar")]
        public async Task<ActionResult<Voto>> Create(Voto voto)
        {
            if (Fase == 2)
            {
                FinalIdsContext _VotacionContext = new FinalIdsContext();
                List<Candidato> candidatos = (from cand in _VotacionContext.Candidatos
                                              where cand.Nombre == voto.CandidatoVotado
                                              select cand).ToList();
                if (candidatos.Count() == 0)
                {
                    return BadRequest("El candidato a votar no existe");
                }
                string hostName = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
                IPAddress[] addresses = hostEntry.AddressList;
                voto.Iporigen = addresses.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                voto.FechaVoto = DateTime.Now;
                List<Voto> votos = (from vote in _VotacionContext.Votos
                                           where vote.Dpi == voto.Dpi
                                           select vote).ToList();

                if (votos.Count() > 0)
                {
                    voto.Estado = 0;
                    _VotacionContext.Votos.Add(voto);


                    List<Estadistica> stats = (from statis in _VotacionContext.Estadisticas
                                                  where statis.Descripcion == "Fraude"
                                                  select statis).ToList();
                    if (stats.Count() == 0)
                    {
                        Estadistica stat = new Estadistica()
                        {
                            Descripcion = "Fraude",
                            CantVotos = 1
                        };
                        _VotacionContext.Estadisticas.Add(stat);
                        await _VotacionContext.SaveChangesAsync();
                    }
                    else
                    {
                        Estadistica stat = (from statis in _VotacionContext.Estadisticas
                                            where statis.Descripcion == "Fraude"
                                            select statis).FirstOrDefault();
                        stat.CantVotos++;
                        _VotacionContext.Estadisticas.Update(stat);
                        await _VotacionContext.SaveChangesAsync();
                    }
                    return BadRequest("El voto es fraude");

                }
                else
                {
                    voto.Estado = 1;
                    _VotacionContext.Votos.Add(voto);
                    await _VotacionContext.SaveChangesAsync();
                    List<Estadistica> stats = (from statis in _VotacionContext.Estadisticas
                                               where statis.Descripcion == voto.CandidatoVotado
                                               select statis).ToList();
                    if (stats.Count() == 0)
                    {
                        Estadistica stat = new Estadistica()
                        {
                            Descripcion = voto.CandidatoVotado,
                            CantVotos = 1
                        };
                        _VotacionContext.Estadisticas.Add(stat);
                        await _VotacionContext.SaveChangesAsync();
                    }
                    else
                    {
                        Estadistica stat = (from statis in _VotacionContext.Estadisticas
                                            where statis.Descripcion == voto.CandidatoVotado
                                            select statis).FirstOrDefault();
                        stat.CantVotos++;
                        _VotacionContext.Estadisticas.Update(stat);
                        await _VotacionContext.SaveChangesAsync();
                    }
                    return Ok();
                }
            }
            else
            {
                return BadRequest("No se aceptan votaciones en este momento");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("CambioFase")]
        public IActionResult ChangePhase()
        {
            Fase++;
            return Ok();
        }

        [HttpGet("GetStats")]
        public async Task<ActionResult<IEnumerable<Estadistica>>> GetList()
        {
            if (Fase < 1)
            {
                return BadRequest("Las estadisticas aún no son accesibles");
            }
            else
            {
                FinalIdsContext _VotacionContext = new FinalIdsContext();
                return Ok(_VotacionContext.Estadisticas);
            }
        }
    }
}

using Fiap.Catalog.Api.Models.Biblioteca;
using Fiap.Catalog.Api.Models.Jogo;
using Fiap.Catalog.Api.Services.Extensions;
using Fiap.Catalog.Domain.Entity;
using Fiap.Catalog.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Fiap.Catalog.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize(Roles = "Administrador")]
  public class JogoController : ControllerBase
  {
    readonly JogoService _jogoService;

    public JogoController(JogoService jogoService)
    {
      _jogoService = jogoService;
    }

    /// <summary>
    /// Retorna lista de Jogos
    /// </summary>
    /// <returns>Lista de Jogos</returns>
    /// <remarks></remarks>
    /// <response code="200">Lista de Todos as Jogos Encontrados</response>
    [HttpGet("")]
    [ProducesResponseType(typeof(GetJogosResponseDto[]), 200)]
    public IActionResult Get(
      [FromQuery] bool? ativado,
      [FromQuery] int offSet = 0,
      [FromQuery] int limit = 50)
    {
      IEnumerable<Jogo> dados = _jogoService.Buscar(ativado, offSet, limit);

      var dto = dados.Select(x => new GetJogosResponseDto
      {
        Id = x.Id,
        Nome = x.Nome,
        Preco = x.Preco,
        Ativo = x.Ativo,
        DataCadastro = x.DataCadastro
      }).ToArray();

      return Ok(dto);
    }

    /// <summary>
    /// Retorna Um Jogo
    /// </summary>
    /// <returns>Jogos</returns>
    /// <remarks></remarks>
    /// <response code="200">Jogo Encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetJogoResponseDto), 200)]
    public IActionResult GetById([FromRoute] int id)
    {
      var jogo = _jogoService.BuscarPorId(id);

      var dto = new GetJogoResponseDto
      {
        Id = jogo.Id,
        Nome = jogo.Nome,
        Preco = jogo.Preco,
        Ativo = jogo.Ativo,
        DataCadastro = jogo.DataCadastro
      };

      return Ok(dto);
    }


    /// <summary>
    /// Inclui um Jogo
    /// </summary>
    /// <param name="dto">Corpo da requisição</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpPost("")]
    [ProducesResponseType(typeof(string), 200)]
    public IActionResult Post([FromBody] PostJogoRequestDto dto)
    {
      var jogo = new Jogo(dto.Nome, dto.Preco);  

      _jogoService.Incluir(jogo);

      return Ok("Jogo Cadastrado Com Sucesso.");
    }


    /// <summary>
    /// Inclui um Jogo
    /// </summary>
    /// <param name="dto">Corpo da requisição</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpPost("Comprar")]
    [ProducesResponseType(typeof(string), 200)]
    public IActionResult PostComprar([FromBody] PostBibliotecaRequestDto dto)
    {
      var idUsuario = User.ConsultarUsuario();  

      var comprar = new JogoBiblioteca(dto.Id_Jogo, idUsuario, dto. Preco);

      _jogoService.Comprar(comprar);

      return Ok("Compra Realizada Com Sucesso.");
    }

    /// <summary>
    /// Altera um Jogo
    /// </summary>
    /// <param name="id">Identificador do Jogo</param>
    /// <param name="put">Corpo com dados a serem modificados</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(string), 200)]
    public IActionResult Put([FromRoute] int id, PutJogoRequestDto put)
    {
      var jogoExistente = _jogoService.BuscarPorId(id);

      _jogoService.Alterar(
        jogoExistente,
        put.Nome,
        put.Preco,
        put.Ativo
      );

      return Ok("Jogo Atualizado Com Sucesso.");
    }
  }
}
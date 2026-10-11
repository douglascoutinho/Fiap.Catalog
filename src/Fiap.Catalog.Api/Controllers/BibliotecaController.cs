using Fiap.Catalog.Api.Models.Biblioteca;
using Fiap.Catalog.Api.Services.Extensions;
using Fiap.Catalog.Domain.Entity;
using Fiap.Catalog.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Catalog.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize(Roles = "Administrador,Usuario")]
  public class BibliotecaController : ControllerBase
  {
    readonly BibliotecaService _bibliotecaService;

    public BibliotecaController(BibliotecaService bibliotecaService)
    {
      _bibliotecaService = bibliotecaService;
    }

    /// <summary>
    /// Retorna lista de Bibliotecas
    /// </summary>
    /// <returns>Lista de Bibliotecas</returns>
    /// <remarks></remarks>
    /// <response code="200">Lista de Todos as Bibliotecas Encontrados</response>
    [HttpGet("")]
    [ProducesResponseType(typeof(GetBibliotecasResponseDto[]), 200)]
    public IActionResult Get(
      [FromQuery] bool? ativado,
      [FromQuery] int offSet = 0,
      [FromQuery] int limit = 50)
    {
      IEnumerable<JogoBiblioteca> dados = _bibliotecaService.Buscar(ativado, offSet, limit);

      var dto = dados.Select(x => new GetBibliotecasResponseDto
      {
        Id = x.Id,
        Nome = x.Jogo.Nome,
        Preco = x.Preco,
        Id_Usuario = x.Id_Usuario,
      }).ToArray();

      return Ok(dto);
    }

    /// <summary>
    /// Realiza um Pedido
    /// </summary>
    /// <param name="dto">Corpo da requisição</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpPost("")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> Post([FromBody] PostBibliotecaRequestDto dto)
    {
      var idUsuario = User.ConsultarUsuario();  

      var comprar = new JogoBiblioteca(idUsuario: idUsuario, idJogo: dto.Id_Jogo,  preco: dto.Preco);

      await _bibliotecaService.RealizarPedido(comprar);

      return Ok("Pedido Realizado Com Sucesso.");
    }
  }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Application.Services.Interfaces;

namespace TicketsRavelli.Controllers.SubCategorias;

[ApiController]
[Route("v1/[controller]")]
public class CategoriaController : ControllerBase {
    private readonly ISubCategoriaService _subCategoriaService;

    public CategoriaController(ISubCategoriaService subCategoriaService) {
        _subCategoriaService = subCategoriaService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> ConsultarCategorias() {
        var categorias = await _subCategoriaService.ConsultarCategorias();

        if (categorias == null)
            return NotFound();

        return Ok(categorias);
    }

    [HttpGet("evento/{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> ConsultarCategoriasEvento(int idEvento) {
        var categorias = await _subCategoriaService.ConsultarCategoriasEvento(idEvento);

        if (categorias == null)
            return NotFound();

        return Ok(categorias);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> ConsultarCategoriaPeloId(int id) {
        var categoria = await _subCategoriaService.ConsultarCategoriaPeloId(id);

        if (categoria == null)
            return NotFound();

        return Ok(categoria);
    }

    [HttpGet("{idEvento}/{categoria}/{idade}/{sexo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> ConsultarCategoriasDoEvento(int idEvento, int categoria, int idade, int sexo) {
        var categorias = await _subCategoriaService.ConsultarCategoriasFiltrado(idEvento, categoria, idade, sexo);

        if (categorias == null)
            return NotFound();

        return Ok(categorias);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> CadastrarCategoria(SubCategoriaInputModel subCategoriaInputModel) {
        var idSubcategoria = await _subCategoriaService.CadastrarCategoria(subCategoriaInputModel);

        return Created($"/categoria/{idSubcategoria}", idSubcategoria);
    }

    [HttpPut("atualizar/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> AtualizarCategoria(int id, SubCategoriaInputModel subCategoriaInputModel) {
        var categoria = await _subCategoriaService.ConsultarCategoriaPeloId(id);

        if (categoria == null)
            return NotFound();

        await _subCategoriaService.AtualizarCategoria(categoria, subCategoriaInputModel);

        return Ok(categoria);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> DeletarCategoria([FromRoute] int id) {
        var categoria = await _subCategoriaService.ConsultarCategoriaPeloId(id);

        if (categoria == null)
            return NotFound(new { message = "Categoria não encontrada" });

        await _subCategoriaService.DeletarCategoria(categoria);

        return Ok();
    }
}


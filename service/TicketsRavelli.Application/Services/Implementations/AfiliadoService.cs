using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Application.InputModels;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Afiliados;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infra.Data;

namespace TicketsRavelli.Application.Services.Implementations;

public class AfiliadoService : IAfiliadoService {
    private readonly ApplicationDbContext _context;

    public AfiliadoService(ApplicationDbContext context) {
        _context = context;
    }

    public async Task AtualizarAfiliado(Afiliado afiliado, AfiliadoInputModel afiliadoInput) {
        afiliado.AtualizarAfiliado(afiliadoInput.cpf, afiliadoInput.nome, afiliadoInput.porcentagem);

        await _context.SaveChangesAsync();
    }

    public async Task CadastrarAfiliado(AfiliadoInputModel afiliado) {
        var novoAfiliado = new Afiliado(afiliado.cpf, afiliado.nome, afiliado.porcentagem);

        await _context.Afiliados.AddAsync(novoAfiliado);
        await _context.SaveChangesAsync();
    }

    public async Task<Afiliado> ConsultarAfiliadoCpf(string cpf)
    {
        return await _context.Afiliados.SingleOrDefaultAsync(a => a.Cpf == cpf);
    }

    public async Task<Afiliado> ConsultarAfiliadoId(string id)
    {
        return await _context.Afiliados.SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Afiliado>> ConsultarAfiliados() {
        return await _context.Afiliados.ToListAsync();
    }

    public async Task<List<Afiliado>> ConsultarAfiliadosEvento(int idEvento) {
        return await _context.Afiliados
            .AsNoTracking()
            .Where(a => a.Inscricoes.Any(i => i.IdEvento == idEvento))
            .Include(a => a.Inscricoes)
                .ThenInclude(i => i.Atleta)
            .ToListAsync();
    }

    public async Task DeletarAfiliado(Afiliado afiliado)
    {
        _context.Remove(afiliado);
        await _context.SaveChangesAsync();
    }
}


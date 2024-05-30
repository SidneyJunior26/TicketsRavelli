using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Core.Entities.Athletes;
using TicketsRavelli.Infra.Data;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Implementations;

public class MedicalDataRepository : IMedicalDataRepository
{
    private readonly ApplicationDbContext _context;

    public MedicalDataRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(RegistroMedico medicalData)
    {
        await _context.RegistrosMedicos.AddAsync(medicalData);
    }

    public async Task<RegistroMedico> QueryByAthleteAsync(string idAthlete)
    {
        return await _context.RegistrosMedicos
            .SingleOrDefaultAsync(m => m.IdAtleta == idAthlete);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using TicketsRavelli.Core.Entities.Athletes;
using TicketsRavelli.Infra.Data;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Implementations;

public class AthleteRepository : IAthleteRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;

    public AthleteRepository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;

        _connectionString = configuration.GetConnectionString("MySql");
    }

    public async Task CreateAsync(Athlete atleta)
    {
        await _context.Atletas.AddAsync(atleta);
    }

    public async Task<Athlete> QueryByCPF(string cpf)
    {
        return await _context.Atletas
            .Include(a => a.RegistroMedico)
            .SingleOrDefaultAsync(a => a.Cpf == cpf);
    }

    public async Task<Athlete> QueryAthleteByEmailOrCPF(string cpfEmail)
    {
        return await _context.Atletas
            .SingleOrDefaultAsync(a => a.Cpf == cpfEmail || a.Email == cpfEmail);
    }

    public async Task<Athlete> QueryByEmail(string email)
    {
        return await _context.Atletas
            .Include(a => a.RegistroMedico)
            .SingleOrDefaultAsync(a => a.Email == email);
    }

    public async Task<Athlete> QueryById(string id)
    {
        return await _context.Atletas
            .Include(a => a.RegistroMedico)
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Athlete>> QueryAllAsync()
    {
        return await _context.Atletas.AsNoTracking().OrderBy(a => a.Nome.Trim()).ToListAsync();
    }

    public void Delete(Athlete atleta)
    {
        _context.Atletas.Remove(atleta);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AthleteExistsByCPF(string cpf)
    {
        return await _context.Atletas
            .AsNoTracking()
            .AnyAsync(a => a.Cpf == cpf);
    }

    public async Task<bool> AthleteExistsByEmail(string email)
    {
        return await _context.Atletas
            .AsNoTracking()
            .AnyAsync(a => a.Email == email);
    }

    public async Task<bool> AthleteExistsById(string id)
    {
        return await _context.Atletas
            .AsNoTracking()
            .AnyAsync(a => a.Id == id);
    }

    public async Task<Athlete> QueryAthleteByRG(string rg)
    {
        return await _context.Atletas
            .AsNoTracking()
            .SingleOrDefaultAsync(a => a.Rg == rg);
    }

    public async Task UpdateCpfAsync(Athlete atlhete, string cpf)
    {
        await using (var sqlConnection = new MySqlConnection(_connectionString))
        {
            sqlConnection.Open();

            var script = $"UPDATE ATLETA SET CPF = {cpf} WHERE CPF = {atlhete.Cpf}";

            await sqlConnection.ExecuteAsync(script);
        }
    }
}


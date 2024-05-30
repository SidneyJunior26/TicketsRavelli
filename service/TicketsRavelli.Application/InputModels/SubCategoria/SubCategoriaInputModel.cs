using System;
namespace TicketsRavelli.Controllers.SubCategorias;

public record SubCategoriaInputModel(int idEvento, int categoria, string descSubcategoria,
    int filtroSexo, int filtroDupla, int idadeDe, int idadeAte, string aviso, bool ativo);
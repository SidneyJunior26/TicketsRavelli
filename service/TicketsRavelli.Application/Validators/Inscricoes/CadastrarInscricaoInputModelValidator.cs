using FluentValidation;
using TicketsRavelli.Controllers.Inscricao;

namespace TicketsRavelli.Application.Validators.Inscricoes
{
    public class CadastrarInscricaoInputModelValidator : AbstractValidator<SubscriptionInputModel> {
        public CadastrarInscricaoInputModelValidator() {
            RuleFor(x => x.idEvento)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Evento não encontrado");

            RuleFor(x => x.cpfAtleta)
                .NotNull()
                .NotEmpty()
                .WithMessage("Por favor, informe o CPF");

            RuleFor(x => x.idSubcategoria)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Por favor, escolha a Categoria");
        }
    }
}
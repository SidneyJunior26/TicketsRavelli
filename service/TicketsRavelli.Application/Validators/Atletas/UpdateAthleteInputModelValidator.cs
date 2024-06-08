using FluentValidation;
using TicketsRavelli.Application.InputModels.Atletas;

namespace TicketsRavelli.Application.Validators.Atletas;

public class UpdateAthleteInputModelValidator : AbstractValidator<UpdateAthleteInputModel>
{
    public UpdateAthleteInputModelValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .NotNull()
            .WithMessage("Por favor, informe seu Nome");

        RuleFor(x => x.Nascimento)
            .NotEmpty()
            .NotNull()
            .WithMessage("Por favor, informe sua Data de Nascimento");

        RuleFor(x => x.Sexo)
            .NotEmpty()
            .NotNull()
            .WithMessage("Por favor, informe seu Sexo");

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .NotNull()
            .MinimumLength(11)
            .WithMessage("Por favor, informe seu CPF");

        RuleFor(x => x.Rg)
            .NotEmpty()
            .NotNull()
            .WithMessage("Por favor, informe seu RG");

        RuleFor(x => x.Endereco)
            .NotNull()
            .NotEmpty()
            .WithMessage("Por favor, informe seu Endereço");

        RuleFor(x => x.Numero)
            .NotNull()
            .NotEmpty()
            .WithMessage("Por favor, informe o número do seu Endereço");

        RuleFor(x => x.Cep)
            .NotNull()
            .NotEmpty()
            .WithMessage("Por favor, informe seu CEP");

        RuleFor(x => x.Cidade)
            .NotNull()
            .NotEmpty()
            .WithMessage("Por favor, informe sua Cidade");

        RuleFor(x => x.Uf)
            .NotNull()
            .NotEmpty()
            .WithMessage("Por favor, informe seu Estado");

        RuleFor(x => x.Uf)
            .NotNull()
            .NotEmpty()
            .WithMessage("Por favor, informe seu País");

        RuleFor(x => x.Celular)
            .NotNull()
            .NotEmpty()
            .WithMessage("Por favor, informe seu Celular");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Por favor, informe seu Email válido");

        RuleFor(x => x.EmergenciaContato)
            .NotEmpty()
            .NotEmpty()
            .WithMessage("Por favor, informe um Contato de Emergência");

        RuleFor(x => x.EmergenciaCelular)
            .NotEmpty()
            .NotEmpty()
            .WithMessage("Por favor, informe o Celular do seu Contato de Emergência");
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Manager.Domain.Entities;

namespace Manager.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {

        public UserValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("A entidade não pode ser vazia.")

                .NotNull()
                .WithMessage("A entidade não pode ser nula.");

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("O Nome não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O Nome não pode ser vazio.")
                
                .MinimumLength(3)
                .WithMessage("O nome deve ter no mínimo 3 caracteres.")

                .MaximumLength(120)
                .WithMessage("O nome deve ter no máximo 120 caracteres.");

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("O email não pode ser nulo")

                .NotEmpty()
                .WithMessage("O email não pode ser vazio")

                .MinimumLength(8)
                .WithMessage("O email deve ter no mínimo 8 caracteres.")

                .MaximumLength(180)
                .WithMessage("O email deve ter no máximo 180 caracteres.")

                .Matches(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
                .WithMessage("O email informado não é válido.");

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("A senha não pode ser nulo.")

                .NotEmpty()
                .WithMessage("A senha não pode ser vazio.")
                
                .MinimumLength(6)
                .WithMessage("A Senha deve ter no mínimo 6 caracteres.")

                .MaximumLength(30)
                .WithMessage("A Senha deve ter no máximo 30 caracteres.");
        }
       
    }
}

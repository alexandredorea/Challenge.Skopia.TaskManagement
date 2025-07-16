namespace Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Validators;

using Challenge.Skopia.TaskManagement.Application.Features.TaskItems.Commands;
using FluentValidation;

public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
{
    public CreateTaskItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título da tarefa é obrigatório")
            .MaximumLength(200).WithMessage("O título deve ter no máximo 200 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("A descrição deve ter no máximo 2000 caracteres");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("A data de vencimento deve ser futura");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Prioridade inválida");

        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("O ID do projeto deve ser maior que zero");
    }
}

public class UpdateTaskItemCommandValidator : AbstractValidator<UpdateTaskItemCommand>
{
    public UpdateTaskItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("O título deve ter no máximo 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("A descrição deve ter no máximo 2000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("A data de vencimento deve ser futura")
            .When(x => x.DueDate.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status inválido")
            .When(x => x.Status.HasValue);

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("O ID do usuário deve ser maior que zero");
    }
}

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .GreaterThan(0).WithMessage("O ID da tarefa deve ser maior que zero");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("O conteúdo do comentário é obrigatório")
            .MaximumLength(2000).WithMessage("O comentário deve ter no máximo 2000 caracteres");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("O ID do usuário deve ser maior que zero");
    }
}
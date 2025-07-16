namespace Challenge.Skopia.TaskManagement.Application.Features.Projects.Validators;

using Challenge.Skopia.TaskManagement.Application.Features.Projects.Commands;
using Challenge.Skopia.TaskManagement.Application.Interfaces.Persistences;
using FluentValidation;

public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator(ITaskManagementContext context)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotNull().WithMessage("O nome do projeto é obrigatório")
            .NotEmpty().WithMessage("O nome do projeto é obrigatório")
            .MaximumLength(200).WithMessage("O nome do projeto deve ter no máximo 200 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("O ID do usuário deve ser maior que zero");

        //RuleFor(x => x.UserId)
        //    .MustAsync(async (userId, cancellation) =>
        //    {
        //        var user = await context.Users
        //        .FirstOrDefaultAsync(p => p.Id == userId, cancellation);

        //        return user is not null;
        //    })
        //    .WithMessage("O usuário especificado não encontrado ou não existe")
        //    .WithErrorCode("USER_NOT_FOUND");
    }
}

public sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator(ITaskManagementContext context)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O ID do projeto deve ser maior que zero");

        //RuleFor(x => x.Id)
        //    .MustAsync(async (id, cancellation) =>
        //    {
        //        var project = await context.Projects
        //            .Include(p => p.Tasks)
        //            .FirstOrDefaultAsync(p => p.Id == id, cancellation);

        //        return project is not null;
        //    })
        //    .WithMessage("Projeto não encontrado.")
        //    .WithErrorCode("PROJECT_NOT_FOUND");

        RuleFor(x => x.Name)
            .NotNull().WithMessage("O nome do projeto é obrigatório")
            .NotEmpty().WithMessage("O nome do projeto é obrigatório")
            .MaximumLength(200).WithMessage("O nome do projeto deve ter no máximo 200 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres");
    }
}

public sealed class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator(ITaskManagementContext context)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O ID do projeto deve ser maior que zero");

        //RuleFor(x => x.Id)
        //    .MustAsync(async (id, cancellation) =>
        //    {
        //        var project = await context.Projects
        //            .Include(p => p.Tasks)
        //            .FirstOrDefaultAsync(p => p.Id == id, cancellation);

        //        return project is not null;
        //    })
        //    .WithMessage("Projeto não encontrado.")
        //    .WithErrorCode("PROJECT_NOT_FOUND");

        //RuleFor(x => x.Id)
        //    .MustAsync(async (id, cancellation) =>
        //    {
        //        var project = await context.Projects
        //            .Include(p => p.Tasks)
        //            .FirstOrDefaultAsync(p => p.Id == id, cancellation);

        //        if (project is null)
        //            return true; // evita erro duplicado — a outra regra acima já trata esse caso

        //        return project.CanBeRemoved;
        //    })
        //    .WithMessage("O projeto possui tarefas pendentes. Conclua ou remova as tarefas antes de excluí-lo.")
        //    .WithErrorCode("HAS_PENDING_TASKS");
    }
}
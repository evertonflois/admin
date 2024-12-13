using MediatR;

using Admin.Domain.Interfaces.Repositories.Autorizacao;

namespace Admin.Application.Areas.Autorizacao.E_710_User.Commands.AutenticarUsuario;

public record AutenticarUsuarioCommand : IRequest<UsuarioAutenticadoDto>
{
    public string? Login { get; set; }

    public string? Senha { get; set; }
}

public class AutenticarUsuarioCommandHandler : IRequestHandler<AutenticarUsuarioCommand, UsuarioAutenticadoDto>
{
    private readonly I_710_UserRepository _repository;

    public AutenticarUsuarioCommandHandler(I_710_UserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UsuarioAutenticadoDto> Handle(AutenticarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _repository.GetAllAsync(new { cd_user = request.Login });

        //var entity = new TodoItem
        //{
        //    ListId = request.ListId,
        //    Title = request.Title,
        //    Done = false
        //};

        //entity.AddDomainEvent(new TodoItemCreatedEvent(entity));
        //_context.TodoItems.Add(entity);

        //await _context.SaveChangesAsync(cancellationToken);

        return new UsuarioAutenticadoDto(request, "", "");
    }
}

using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class ModCommandRepositoryLogic : IModCommandRepositoryLogic {
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;

    public ModCommandRepositoryLogic(
      IQueryCommandService<IUnitOfWork> unitOfWork
    ) {
      _unitOfWork = unitOfWork;
    }

    public IReadOnlyList<ISendable<ITransmittable>> AddCommand(string command, string response) {
      var existingCommand = _unitOfWork.Query(r => r.CustomCommand.Get(command));
      string confirmation;
      if (existingCommand == null) {
        _unitOfWork.Command(r => r.CustomCommand.Add(command, response));
        confirmation = $"!{command} added";
      } else {
        _unitOfWork.Command(r => r.CustomCommand.Update(command, response));
        confirmation = $"!{command} updated";
      }
      return new SendablePublicMessage(confirmation).Wrap().ToList();
    }

    public IReadOnlyList<ISendable<ITransmittable>> DelCommand(string command) {
      var existingCommand = _unitOfWork.Query(r => r.CustomCommand.Get(command));
      string confirmation;
      if (existingCommand == null) {
        confirmation = $"!{command} is not an existing custom command";
      } else {
        _unitOfWork.Command(r => r.CustomCommand.Delete(command));
        confirmation = $"!{command} removed";
      }
      return new SendablePublicMessage(confirmation).Wrap().ToList();
    }

  }
}

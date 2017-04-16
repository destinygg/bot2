using System.Collections.Generic;
using Bot.Models;

namespace Bot.Repository.Interfaces {
  public interface IAutoPunishmentRepository {
    void Add(AutoPunishment autoPunishment);
    void Update(AutoPunishment autoPunishment);
    IList<AutoPunishment> GetAllWithUser { get; }
  }
}

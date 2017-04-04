using System.Collections.Generic;
using Bot.Models;

namespace Bot.Repository.Interfaces {
  public interface IAutoPunishmentRepository {
    void Add(AutoPunishment autoPunishment);
    IEnumerable<AutoPunishment> GetAllMutedString();
    IEnumerable<AutoPunishment> GetAllBannedRegex();
    IEnumerable<AutoPunishment> GetAllBannedString();
    IEnumerable<AutoPunishment> GetAllMutedRegex();
    void Update(AutoPunishment autoPunishment);
  }
}

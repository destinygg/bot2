using System.Collections.Generic;
using Bot.Database.Entities;
using Bot.Models;

namespace Bot.Repository.Interfaces {
  public interface IAutoPunishmentRepository {
    void Add(AutoPunishment autoPunishment);
    void Delete(AutoPunishment autoPunishment);
    void Update(AutoPunishment autoPunishment);
    IList<AutoPunishment> GetAllWithUser { get; }
    AutoPunishment Get(string term, AutoPunishmentType type);
  }
}

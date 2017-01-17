﻿using System;
using Bot.Logic.Contracts;
using Bot.Tools;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public class ReceivedStringNuke : ReceivedNuke {
    private readonly string _nukedString;
    public ReceivedStringNuke(ReceivedMessage message, IModCommandParser parser, ITimeService timeService) : base(message, timeService) {
      Duration = parser.Nuke(message.Text).Item2;
      _nukedString = parser.Nuke(message.Text).Item1;
    }

    public override TimeSpan Duration { get; }

    public override bool IsMatch(string possibleVictimText)
      => possibleVictimText.IgnoreCaseContains(_nukedString) || possibleVictimText.SimilarTo(_nukedString) >= Settings.NukeMinimumStringSimilarity;
  }
}
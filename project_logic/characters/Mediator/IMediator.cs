﻿using System.ComponentModel;

namespace project_logic.characters.Mediator
{
    public interface IMediator
    {
        void Notify(Character sender, EventType @event);
        int GetPX();
        int GetPY();
        int GetBlinkyX();
        int GetBlinkyY();
        Direction GetPDir();
    }
}

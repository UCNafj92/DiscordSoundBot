using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace WowCombatLog.Events
{
    public record WowDeathEvent(
        string PlayerName,
        string RawLine,
        DateTimeOffset Timestamp
    ) : IEvent;

}

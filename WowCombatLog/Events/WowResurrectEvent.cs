using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace WowCombatLog.Events
{
    public record WowResurrectEvent(
        string SourcePlayer,
        string TargetPlayer,
        string SpellName,
        string RawLine,
        DateTimeOffset Timestamp
    ) : IEvent;
}

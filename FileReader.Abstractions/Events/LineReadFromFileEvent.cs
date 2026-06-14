using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileReader.Abstractions.Events
{
    public record LineReadFromFileEvent(
    string FilePath,
    string Line,
    DateTimeOffset Timestamp
) : IEvent;
}

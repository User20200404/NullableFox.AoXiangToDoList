using NullableFox.AoXiangToDoList.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Exceptions
{
    internal class ApplicationShowableException : Exception
    {
        public ApplicationShowableException()
        {
        }
        public ApplicationShowableException(ApplicationShowableException source):this()
        {
            Title = source.Title;
            Description = source.Description;
            Severity = source.Severity;
        }
        public override string Message => $"[{Severity}]{Title}\n{Description}";
        public string Title { get; init; } = string.Empty;
        public required string Description { get; init; } = string.Empty;
        public ExceptionSeverity Severity { get; init; } = ExceptionSeverity.Error;
    }

    internal enum ExceptionSeverity
    {
        NotIndicated,
        Information,
        Warning,
        Error,
        Fatal
    }
}

using System;
using System.Runtime.Serialization;

namespace ProjName.Application.Shared;

[Serializable]
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base() { }
    public ForbiddenAccessException(string message) : base(message) { }

    protected ForbiddenAccessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

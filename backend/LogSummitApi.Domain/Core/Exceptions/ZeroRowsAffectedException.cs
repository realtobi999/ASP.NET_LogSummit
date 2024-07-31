﻿namespace LogSummitApi.Domain.Core.Exceptions;

public class ZeroRowsAffectedException : Exception
{
    public ZeroRowsAffectedException() : base("Database modification attempt resulted in zero rows being affected.")
    {
    }
}

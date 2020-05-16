using System;

namespace Markit.Api.Exceptions
{
    public class ListAnalysisException : Exception
    {
        public ListAnalysisException(string message)
            : base(message)
        {
        }
    }
}
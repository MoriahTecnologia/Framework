using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArezzoCo.Framework.DTO
{
    public interface IResultAction
    {
        bool Success { get; }
        int AffectedLines { get; }
        string Message { get; }
        string Log { get; }
        long Id { get; }        
        Exception Exception { get; }
    }

    public interface IResultAction<T> : IResultAction
    {
        T DataResult { get; }
    }

    public interface IResultAction<T, X> : IResultAction<T>
    {
        X CallError { get; }
    }
}

using System;

namespace ArezzoCo.Framework.DTO
{
    /// <summary>
    /// Class para transporte de dados oriundos de um resultado.
    /// </summary>
    public class ResultAction : IResultAction
    {
        private int _affectedLines;
        private DateTime timeStart = DateTime.MinValue;
        private DateTime timeEnd = DateTime.MinValue;
        private bool timeEndTriggered = false;

        public long Id { get; set; }
        public int AffectedLines
        {
            get
            {
                return _affectedLines;
            }
            set
            {
                _affectedLines = value;
                SetEndTime();
            }
        }

        public string Log { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; } = true;
        public Exception Exception { get; set; } = null;

        public bool IsError
        {
            get
            {
                return Exception != null;
            }
        }

        protected void SetEndTime()
        {
            timeEndTriggered = true;
            timeEnd = DateTime.Now;
        }
        public ResultAction()
        {
            TimeStart = DateTime.Now;
        }

        public TimeSpan ExecutionTime
        {
            get
            {
                if (!timeEndTriggered)
                    SetEndTime();

                return timeEnd - TimeStart;
            }
        }

        protected DateTime TimeStart { get => timeStart; set => timeStart = value; }

        public void SetFailMessage(string message)
        {
            Success = false;
            Message = message;
            SetEndTime();
        }

        public void SetFailMessage(Exception exception)
        {
            Success = false;
            Message = exception.Message;
            while (exception.InnerException != null)
            {
                Message += $"\r\n{exception.InnerException.Message}";
                exception = exception.InnerException;
            }
            SetEndTime();
        }

        public void SetException(Exception ex)
        {
            Exception = ex;
            SetFailMessage(ex.Message);
        }

    }

    public class ResultAction<T> : ResultAction, IResultAction<T>
    {
        private T _dataResult;
        public T DataResult
        {
            get
            {
                return _dataResult;
            }
            set
            {
                _dataResult = value;
                SetEndTime();
            }
        }


    }

    public class ResultAction<T, X> : ResultAction<T>, IResultAction<T, X>
    {
        DateTime timeEnd2 = DateTime.MinValue;
        bool timeEndTriggered2 = false;
        private X _callError;

        protected void SetEndTime2()
        {
            timeEndTriggered2 = true;
            timeEnd2 = DateTime.Now;
        }

        public TimeSpan ExecutionTimeCallError
        {
            get
            {
                if (!timeEndTriggered2)
                    SetEndTime2();

                return timeEnd2 - TimeStart;
            }
        }
        public X CallError
        {
            get
            {
                return _callError;
            }
            set
            {
                _callError = value;
                SetFailMessage("");
            }
        }
    }


}

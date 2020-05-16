using System;

namespace Moriah.Framework.DTO
{
    /// <summary>
    /// Use this class to control the result of an action and its responses, if there are any exceptions, control by inserting into the class.
    /// </summary>
    public class ResultAction : IResultAction
    {
        private int _affectedLines;
        private long _id;
        private DateTime timeStart = DateTime.MinValue;
        private DateTime timeEnd = DateTime.MinValue;
        private bool timeEndTriggered = false;

        /// <summary>
        /// The Id returned by a query or insertion.
        /// </summary>
        public long Id 
        { 
            get 
            { 
                return _id; 
            } 
            set 
            {
                _id = value;
                SetEndTime();
            } 
        }

        /// <summary>
        /// The number of lines affected.
        /// </summary>
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

        /// <summary>
        /// Text message to store in LOG.
        /// </summary>
        public string Log { get; set; }

        /// <summary>
        /// Friendly text message, displayed on screen.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// If the request was successfully returned, the default is always True, except when there is an error or error message.
        /// </summary>
        public bool Success { get; private set; } = true;

        /// <summary>
        /// In case of any exception, this exception returns.
        /// </summary>
        public Exception Exception { get; set; } = null;

        /// <summary>
        /// If any Exceptions have been configured.
        /// </summary>
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

        /// <summary>
        /// Use this class to control the result of an action and its responses, if there are any exceptions, control by inserting into the class.
        /// </summary>
        public ResultAction()
        {
            TimeStart = DateTime.Now;
        }

        /// <summary>
        /// Execution time of the instantiation until the configuration of the Id, or of the affected lines, or call of the failure methods.
        /// </summary>
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

        /// <summary>
        /// Configures a failure message, stops the execution time count and configures the Success property as False.
        /// </summary>
        /// <param name="message">Erro message.</param>
        public void SetFailMessage(string message)
        {
            Success = false;
            Message = message;
            SetEndTime();
        }

        /// <summary>
        ///  Configures failure message based on Exceptions error messages, this concatenates all Exceptions and InnerExceptions, stops the execution time count and configures the Success property as False.
        /// </summary>
        /// <param name="exception">Exception.</param>
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

        /// <summary>
        /// Set the Exception and its message to be the return message.
        /// </summary>
        /// <param name="exception">Exception.</param>
        public void SetException(Exception exception)
        {
            Exception = exception;
            SetFailMessage(exception.Message);
        }

    }

    /// <summary>
    /// Use this class to control the result of an action and its responses, if there are any exceptions, control by inserting in the class, as well as returning the necessary data through the DataResult property.
    /// </summary>
    /// <typeparam name="T">Type of the DataResult property.</typeparam>
    public class ResultAction<T> : ResultAction, IResultAction<T>
    {
        private T _dataResult;

        /// <summary>
        /// The data to be returned.
        /// </summary>
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

    /// <summary>
    /// Use this class to control the result of an action and its responses, if there are any exceptions, control by inserting into the class, as well as returning the necessary data through the DataResult property or another type of data in the DataResultAlternative.
    /// </summary>
    /// <typeparam name="T">Type of the DataResult property.</typeparam>
    /// <typeparam name="X">Type of the DataResultAlternative property.</typeparam>
    public class ResultAction<T, X> : ResultAction<T>, IResultAction<T, X>
    {
        DateTime timeEnd2 = DateTime.MinValue;
        bool timeEndTriggered2 = false;
        private X _dataResultAlternative;

        protected void SetEndTime2()
        {
            timeEndTriggered2 = true;
            timeEnd2 = DateTime.Now;
        }

        /// <summary>
        /// Execution time of the instantiation until the DataResultAlternative is configured or call of the failure methods.
        /// </summary>
        public TimeSpan ExecutionTimeDataResultAlternative
        {
            get
            {
                if (!timeEndTriggered2)
                    SetEndTime2();

                return timeEnd2 - TimeStart;
            }
        }

        /// <summary>
        /// Data alternative.
        /// </summary>
        public X DataResultAlternative
        {
            get
            {
                return _dataResultAlternative;
            }
            set
            {
                _dataResultAlternative = value;
                SetFailMessage("");
            }
        }
    }


}

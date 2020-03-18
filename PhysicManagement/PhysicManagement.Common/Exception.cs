using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace PhysicManagement.Common
{

    [Serializable]
    public class OutOfRangeException : System.Exception
    {
        public OutOfRangeException() : base("Pagger got a out of range value")
        {
        }
    }
    [Serializable]
    public class InvalidDeleteOperationException : System.Exception
    {
        public InvalidDeleteOperationException() : base("Invalid Delete Operation Exception")
        {
        }
    }


    [Serializable]
    public class InvalidUpdateOperationException : System.Exception
    {
        public InvalidUpdateOperationException() : base("Invalid Update Operation Exception")
        {
        }
        public InvalidUpdateOperationException(string message)
        : base(message) { }

        public InvalidUpdateOperationException(string message, System.Exception inner)
            : base(message, inner) { }
    }
    [Serializable]
    public class InvalidAddOperationException : System.Exception
    {
        public InvalidAddOperationException() : base("Invalid add Operation Exception")
        {
        }
        public InvalidAddOperationException(string message)
        : base(message) { }

        public InvalidAddOperationException(string message, System.Exception inner)
            : base(message, inner) { }
    }

    public class MegaException 
    {
        public static ValidationException ThrowException(string error) {
            return ThrowException(new List<string> { error });
        }
        public static ValidationException ThrowException(List<string> errors)
        {
            var Errors = new List<FluentValidation.Results.ValidationFailure>();
            errors.ForEach(error =>
            {
                Errors.Add(new FluentValidation.Results.ValidationFailure("", error));
            });

            return new FluentValidation.ValidationException(Errors);
        }
    }
}

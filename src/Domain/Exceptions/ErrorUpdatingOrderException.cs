using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions;

public class ErrorUpdatingOrderException : Exception
{
    public ErrorUpdatingOrderException() : base("There was an error updating your order. Please try again, or contact support if the issue persists.") { }
}

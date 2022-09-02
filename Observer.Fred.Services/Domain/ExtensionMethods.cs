using System.Runtime.CompilerServices;

namespace LeaderAnalytics.Observer.Fred.Services.Domain;
public static class ExtensionMethods
{

    // replaced by ArgumentException.ThrowIfNullorEmpty(...) in .net 7


    public static void ThrowIfNullOrEmpty(string argument, [CallerArgumentExpression("argument")] string? paramName = null)
    { 
        if(string.IsNullOrEmpty(argument))
            throw new ArgumentNullException(paramName);
    }



}

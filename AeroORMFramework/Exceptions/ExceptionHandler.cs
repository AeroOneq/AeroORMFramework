using System;

namespace AeroORMFramework
{
    internal static class ExceptionHandler
    {
        public static void Handle(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

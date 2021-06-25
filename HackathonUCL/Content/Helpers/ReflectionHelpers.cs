using System;
using System.Linq;

namespace HackathonUCL
{
    public static partial class Utils
    {
        public static Type[] GetInheritedClasses(Type MyType)
        {
            return MyType.Assembly.GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && MyType.IsAssignableFrom(TheType)).ToArray();
        }
    }
}

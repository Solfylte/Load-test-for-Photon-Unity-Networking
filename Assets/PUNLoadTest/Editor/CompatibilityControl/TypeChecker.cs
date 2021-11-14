using System;
using System.Linq;

namespace PunLoadTest.CompatibilityControl
{
    public static class TypeChecker
    {
        public static bool IsTypeExist(string fullTypeName)
        {
            var type = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from myType in assembly.GetTypes()
                        where myType.FullName == fullTypeName
                        select myType).FirstOrDefault();

            return type != null;
        }
    }
}

using System;
using System.Threading.Tasks;

namespace Lithium
{
    public static class TaskFromResult
    {
        public static object Create(Type resultType, object result)
        {
            var mthd = typeof(Task).GetMethod("FromResult");
            var genericMthd = mthd.MakeGenericMethod(resultType);
            return genericMthd.Invoke(null, new object[] { resultType });
        }
    }
}

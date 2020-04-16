using System;
using System.Threading.Tasks;

namespace Lithium
{
    public class TaskFromResultFactory
    {
        public object Create(Type resultType, object result)
        {
            var mthd = typeof(Task).GetMethod("FromResult");
            var genericMthd = mthd.MakeGenericMethod(resultType);
            return genericMthd.Invoke(null, new object[] { resultType });
        }
    }
}

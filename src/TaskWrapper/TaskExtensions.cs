using System;
using System.Threading.Tasks;

namespace Lithium
{
    public static class TaskExtensions
    {
        public static T GetResult<T>(this Task task)
        {
            var taskType = task.GetType();
            if (!taskType.IsGenericType)
            {
                throw new InvalidOperationException("Cannot get result out of void task");
            }
            return (T)taskType.GetProperty("Result").GetValue(task);
        }
    }
}
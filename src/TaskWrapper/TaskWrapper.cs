using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Lithium
{
    public class TaskWrapper
    {
        private readonly ConcurrentDictionary<Type, Func<Task, IMethodInvocation, Action<Task>, Action<Task, Exception>, Task>> wrapperCreators =
            new ConcurrentDictionary<Type, Func<Task, IMethodInvocation, Action<Task>, Action<Task, Exception>, Task>>();

        public Func<Task, IMethodInvocation, Action<Task>, Action<Task, Exception>, Task> GetWrapperCreator(Type taskType)
        {
            return this.wrapperCreators.GetOrAdd(
                taskType,
                (Type t) =>
                {
                    if (t == typeof(Task))
                    {
                        return this.CreateWrapperTask;
                    }
                    else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        return (Func<Task, IMethodInvocation, Action<Task>, Action<Task, Exception>, Task>)this.GetType()
                            .GetMethod("CreateGenericWrapperTask", BindingFlags.Instance | BindingFlags.NonPublic)
                            .MakeGenericMethod(new Type[] { t.GenericTypeArguments[0] })
                            .CreateDelegate(typeof(Func<Task, IMethodInvocation, Action<Task>, Action<Task, Exception>, Task>), this);
                    }
                    else
                    {
                        return (task, _1, _2, _3) => task;
                    }
                }
            );
        }

        private async Task CreateWrapperTask(Task task, IMethodInvocation input, Action<Task> onTaskCompleted, Action<Task, Exception> onError)
        {
            try
            {
                await task.ConfigureAwait(false);
                onTaskCompleted(task);
            }
            catch (Exception ex)
            {
                onError(task, ex);
                throw;
            }
        }

        private Task CreateGenericWrapperTask<T>(Task task, IMethodInvocation input, Action<Task> onTaskCompleted, Action<Task, Exception> onError)
        {
            return this.DoCreateGenericWrapperTask<T>((Task<T>)task, input, onTaskCompleted, onError);
        }

        private async Task<T> DoCreateGenericWrapperTask<T>(Task<T> task, IMethodInvocation input, Action<Task> onTaskCompleted, Action<Task, Exception> onError)
        {
            try
            {
                T value = await task.ConfigureAwait(false);
                onTaskCompleted(task);
                return value;
            }
            catch (Exception ex)
            {
                onError(task, ex);
                throw;
            }
        }
    }
}

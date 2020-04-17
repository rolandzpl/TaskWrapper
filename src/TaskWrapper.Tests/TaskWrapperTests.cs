using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Lithium
{
    [TestFixture]
    public class TaskWrapperTests
    {
        [Test]
        public void Test1()
        {
            var sw = new Stopwatch();
            var input = (IMethodInvocation)null;
            var target = new TaskWrapper();
            sw.Start();
            var wrappedTask = target.GetWrapperCreator(typeof(int))
                (
                    Task.Run(() => { Task.Delay(100).Wait(); return 1; }),
                    input,
                    t => sw.Stop(),
                    (t, ex) => sw.Stop()
                );
            wrappedTask.Wait();
            Assert.That(sw.ElapsedMilliseconds, Is.GreaterThan(100));
        }
    }
}
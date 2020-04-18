using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Lithium
{
    [TestFixture]
    public class TaskWrapperTests
    {
        [Test]
        public void GetWrapperCreator_TargetTaskCompletes_CompletionCallbackIsCalledAfterTaskCompletes()
        {
            var sw = new Stopwatch();
            sw.Start();
            wrapper
                .GetWrapperCreator(typeof(int))
                (
                    Task.Delay(100).ContinueWith(t => 1),
                    t => sw.Stop(),
                    (t, ex) => sw.Stop()
                )
                .Wait();
            Assert.That(sw.ElapsedMilliseconds, Is.GreaterThan(100));
        }

        [Test]
        public void GetWrapperCreator_TargetTaskCompletes_CanReadResult()
        {
            int result = wrapper
                .GetWrapperCreator(typeof(int))
                (
                    Task.FromResult(1),
                    t => { },
                    (t, ex) => { }
                )
                .GetResult<int>();
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void GetWrapperCreator_TargetTaskFailsWithException_ErrorCallbackIsCalled()
        {
            Exception exception = null;
            wrapper
                .GetWrapperCreator(typeof(int))
                (
                    Task.FromException(new Exception()),
                    t => { },
                    (t, ex) => { exception = ex; }
                )
                .Wait();
            Assert.That(exception, Is.Not.Null);
        }

        [SetUp]
        protected void SetUp()
        {
            wrapper = new TaskWrapper();
        }

        private TaskWrapper wrapper;
    }
}
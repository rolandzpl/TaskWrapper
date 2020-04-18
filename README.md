# TaskWrapper

This project implements a task wrapper to support interception of
async/await methods in dotnet. This allows to control fully the moment
when task starts and when it completes what is very important when
it comes to implement Aspect Oriented Programming fully.

Direct demand to implement this solution was necessity to measure
execution time of certain parts of the system.The problem was that
half of methods were implemented in an async/await way.

## Class TaskWrapper

The core of the project is the class TaskWrapper. It contains method
GetWrapperCreator which provides a wrapper function enabling towrap
instance of Task (that is a result of calling async method).

Here's an example how to wrap the task:

````c#
var wrappedTask = target.GetWrapperCreator(typeof(int))
(
    Task.Run(. . ., // Here the target task comes
    t => { },       // Task completed
    (t, ex) => { }  // Task failed
);
````

Later, it is easy to get result from a completed task. Example below shows
how to get result from a task returninh integer value:

````c#
wrappedTask.GetResult<int>();
````

The extension method ````GetResult()```` is useful because wrapped task
being returned is not generic and it is not possible to directly read ````Result```` property.

## Class TaskFromResult

Class ````TaskFromResult```` is a convenient factory when it comes to use
syntax like ````Task.FromResult<T>()```` but not using generic syntax. Example:

````c#
var task = TaskFromResult.Create(typeof(int), 1);
````


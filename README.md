# TaskWrapper

This project implements a task wrapper to support interception of
async/await methods in dotnet. This allows to control fully the moment
when task starts and when it completes what is very important when
it comes to implement Aspect Oriented Programming fully.

Direct demand to implement this solution was necessity to measure
execution time of certain parts of the system.The problem was that
half of methods were implemented in an async/await way.

Currently,the implementation was tightly coupled with Unity container
(and its AOP supporter: Unity.Interception). Perhaps there will be demand to change it in the future.


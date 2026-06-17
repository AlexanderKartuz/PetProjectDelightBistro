
using MultyThreadExample;

var user = new User();
var payload = new Payload();
payload.counter = 0;

var taskAAA = new Task(() =>
{
    user.Do(payload);
});

var taskBBB = new Task(() =>
{
    user.Do(payload);
});

taskAAA.Start();
taskBBB.Start();


Thread.Sleep(1000 * 2000);
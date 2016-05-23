using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Async.ReturningCompletedTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new MySynchronousImplementation();
            Console.WriteLine(
                AsyncContext.Run<int>(() =>
                    a.GetValueAsync()
                )
            );
            Console.ReadKey();
        }

        static Task<T> NotImplementedAsync<T>()
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(new NotImplementedException());
            return tcs.Task;
        }

    }

    interface IMyAsyncInterface
    {
        Task<int> GetValueAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    class MySynchronousImplementation : IMyAsyncInterface
    {
        public Task<int> GetValueAsync()
        {
            //Task.FromResult只返回正确的结果，不返回Task其他类型的结果，如NotImplementedException。
            return Task.FromResult(13);
        }
    }


}

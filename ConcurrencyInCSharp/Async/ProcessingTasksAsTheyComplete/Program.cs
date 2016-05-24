using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace ProcessingTasksAsTheyComplete
{
    class Program
    {
        static void Main(string[] args)
        {

            AsyncContext.Run(() =>
                UseOrderByCompletionAsync()
                );

            Console.ReadKey();
        }
        
        static async Task<int> DelayAndReturnAsync(int val)
        {
            await Task.Delay(TimeSpan.FromSeconds(val));
            return val;
        }

        static async Task UseOrderByCompletionAsync()
        {
            Task<int> task1 = DelayAndReturnAsync(20);
            Task<int> task2 = DelayAndReturnAsync(30);
            Task<int> task3 = DelayAndReturnAsync(10);
            var tasks = new[] { task1, task2, task3 }; 
            
            foreach(var task in tasks.OrderByCompletion())
            {
                var result=await task;
                Console.WriteLine(result);
            }
        }

        async Task ResumeOnContextAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            // 这个方法在同一个上下文中恢复运行。
        }
        async Task ResumeWithoutContextAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            // 这个方法在恢复运行时，会丢弃上下文。
        }
    }
}

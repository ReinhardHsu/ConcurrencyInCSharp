using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace HandlingExceptionsFromAsyncTaskMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncContext.Run(() =>
                TestAsync()
                );
        }

        static async Task ThrowExceptionAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            throw new InvalidOperationException("test");
        }

        //static async Task TestAsync()
        //{
        //    try
        //    {
        //        await ThrowExceptionAsync();
        //    }
        //    catch(InvalidOperationException ex)
        //    {

        //    }
        //}

        static async Task TestAsync()
        {
            //抛出异常，并将其存储在Task中
            Task task = ThrowExceptionAsync();
            try
            {
                //Task对象被await调用，异常在这里再次被引发
                await task;
            }
            catch (InvalidOperationException ex)
            {
                //异常在这里被捕获
            }
        }
    }
}

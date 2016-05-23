using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;
using System.Net.Http;

namespace WaitingForASetOfTasksToComplete
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine( AsyncContext.Run<string>(() =>
            //     DownloadAllAsync(new string[] {"http://baidu.com","http://taobao.com" })
            // )
            // );

            //AsyncContext.Run(() =>
            //    ObserveOneExceptionAsync()
            // );

            AsyncContext.Run(() =>
                ObserveAllExceptionsAsync()
             );

            Console.ReadKey();
        }

        static async Task<string> DownloadAllAsync(IEnumerable<string> urls)
        {
            var httpClient = new HttpClient();
            var downloads = urls.Select(url => httpClient.GetStringAsync(url));
            //到这里还有真正开始

            //所有URL下载同时开始
            Task<string>[] downloadTasks = downloads.ToArray();

            //用异步方式等待所有下载完成
            string[] htmlPages = await Task.WhenAll(downloadTasks);

            return string.Concat(htmlPages);
        }

        static async Task ThrowNotImplementedExceptionAsync()
        {
            throw new NotImplementedException();
        }

        static async Task ThrowInvalidOperationExceptionAsync()
        {
            throw new InvalidOperationException();
        }

        static async Task ObserveOneExceptionAsync()
        {
            var task1 = ThrowInvalidOperationExceptionAsync();
            var task2 = ThrowNotImplementedExceptionAsync();
            try
            {
                await Task.WhenAll(task1, task2);
            }
            catch(Exception ex)
            {
                //这里只能得到一个异常，要么是task1，要么是task2
            }
        }

        static async Task ObserveAllExceptionsAsync()
        {
            var task1 = ThrowInvalidOperationExceptionAsync();
            var task2 = ThrowNotImplementedExceptionAsync();

            Task allTasks= Task.WhenAll(task1, task2);
            try
            {
                await allTasks;
            }
            catch
            {
                AggregateException allException = allTasks.Exception;
            }
        }
    }
}

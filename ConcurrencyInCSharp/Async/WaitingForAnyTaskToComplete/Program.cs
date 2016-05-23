using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace WaitingForAnyTaskToComplete
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(AsyncContext.Run<string>(() =>
               FirstRespondUrlAsync("http://msdynax.net", "http://baidu.com")
                )
                );

            Console.ReadKey();
        }

        private static async Task<string> FirstRespondUrlAsync(string urlA,string urlB)
        {
            var httpClient = new HttpClient();

            //并发地开始两个下载任务
            Task<string> downloadTaskA = httpClient.GetStringAsync(urlA);
            Task<string> downloadTaskB = httpClient.GetStringAsync(urlB);

            //等待任意一个任务完成
            Task<string> completedTask = await Task.WhenAny(downloadTaskA, downloadTaskB);

            //返回从URL得到的数据
            var data = await completedTask;
            return data;
        }
    }
}

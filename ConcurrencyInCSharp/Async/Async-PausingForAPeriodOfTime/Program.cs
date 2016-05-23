using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;
using System.Net.Http;

namespace Async.PausingForAPeriodOfTime
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine( AsyncContext.Run<string>( ()=>

                DownloadStringWithTimeout("http://ssss.sss")
            ));
            Console.ReadKey();
        }
        
        /// <summary>
        /// 暂停一段时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        static async Task<T> DelayResult<T>(T result,TimeSpan delay)
        {
            await Task.Delay(delay);
           return  result;
        }


        /// <summary>
        /// 指数退避。实际开发最好使用微软企业库的瞬时错误处理模块。
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        static async Task<string> DownloadStringWithRetries(string uri)
        {
            using(var client=new HttpClient())
            {
                var nextDelay = TimeSpan.FromSeconds(1);

                //遇到异常，重试三次，间隔分别为1秒，2秒，4秒
                for(int i = 0; i != 3; ++i)
                {
                    try
                    {
                        return await client.GetStringAsync(uri);
                    }
                    catch
                    {

                    }

                    await Task.Delay(nextDelay);
                    nextDelay = nextDelay + nextDelay;
                }

                //如果前面三次都遇到异常，那就再试一次。如果再遇到异常就返回给调用者
                return await client.GetStringAsync(uri);
            }
        }

        /// <summary>
        /// 带有超时功能的任务。Task.Delay适合对异步代码进行单元测试或实现重试逻辑。要实现超时功能，最好使用CancellationToken。
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        static async Task<string> DownloadStringWithTimeout(string uri)
        {
            using(var client=new HttpClient())
            {
                var downloadTask = client.GetStringAsync(uri);
                var timeoutTask = Task.Delay(3000);
                
                //参数是两个任务，谁先完成谁先返回
                var completedTask = await Task.WhenAny(downloadTask, timeoutTask);

                //如果超时任务先完成了，就返回超时任务
                if (completedTask == timeoutTask)
                {
                    return null;
                }

                return await downloadTask;
            }
        }
    }
}

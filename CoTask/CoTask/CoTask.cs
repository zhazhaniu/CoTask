using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoTask
{
    public interface ICoTask<out TResult>
    {
        TResult Result { get; }
        Task<object> awaitObject();
    }

    public class CoTask<T, TResult> : ICoTask<TResult>
    {
        public CoTask(Task<T> t)
        {
            task = t;
        }

        Task<T> task;
        public TResult Result
        {
            get
            {
                return (TResult)(object)task.Result;
            }
        }

        public async Task<object> awaitObject()
        {
            var t = await task;
            return (TResult)(object)t;
        }
    }

}

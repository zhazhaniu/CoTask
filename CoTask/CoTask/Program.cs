/*
 * Created by zhazha 2018.05.20
 * This Program show you how to solve Task<A> assign to Task<B> when B is derived from A，
 * and you actually know the return value is B type
 */


using System;
using System.Threading;
using System.Threading.Tasks;
using CoTask;

namespace CoTask
{
    public class A
    {
        public int aaa = 0;
    }

    public class B : A
    {
        public int bbb = 1;
    }

    class Program
    {
        static void Main(string[] args)
        {
            doTest();
            Console.WriteLine("Program End");
            Console.ReadLine();
        }

        static async void doTest()
        {
            /*
             * In ATest, it always new base class A
             * but return ICoTask<B> as a proxy of TaskCompletionSource<B>
            */
            test = ATest<B>;
            B b = new B();
            b.bbb = 123;
            var tcs = test(b);
            //Synchronize
            //B ret = tcs.Result;

            //Asynchronize
            var awaitResult = tcs.awaitObject();
            B ret = (B)await awaitResult;
            Console.WriteLine(ret.aaa + " " + ret.bbb);
        }

        /*
         * Just a place to store the TaskCompletionSource, sync/async example
         * In most time, you will need a Dictionary or List to store these task values
         */
        static TaskCompletionSource<A> TCS;
        static Func<B, ICoTask<B>> test;

        //this function new TaskCompletionSource<A> but return ICoTask<B>, the proxy of TaskCompletionSource<B>
        static ICoTask<T> ATest<T>(A a) where T : A
        {
            var tcs = new TaskCompletionSource<A>();
            new Thread(() =>
            {
                //assign value after 3 seconds, simulate async method
                Thread.Sleep(3000);
                B b = new B();
                b.aaa = 111;
                b.bbb = 222;
                //some times you will find tcs in a Dictionary, with a SN in a RPC callback
                TCS.SetResult(b);
            }).Start();

            var coTask = new CoTask<A, T>(tcs.Task);
            TCS = tcs;
            return coTask;
        }

        

        
    }
}

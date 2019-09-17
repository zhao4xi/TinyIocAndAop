using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyIocAndAop
{
    class TinyAopTest
    {
        public static void Test()
        {
            Console.WriteLine("Tiny Demo Start --------------------------------");
            var service = TinyAop.Create<IServiceA, ServiceA>(new LogInterceptor());
            Console.WriteLine();
            Console.WriteLine("Method Say Start --------------------------------");
            service.Say("hello world");
            Console.WriteLine("Method Say End ----------------------------------");
            Console.WriteLine();
            Console.WriteLine("Method Error Start --------------------------------");
            service.Error();
            Console.WriteLine("Method Error End ----------------------------------");
            Console.WriteLine();
            Console.WriteLine("Tiny Demo End ----------------------------------");
            Console.Read();
        }
    }

    public interface IServiceA { void Say(string word); void Error(); }

    public class ServiceA : IServiceA
    {

        public void Error()
        {
            throw new Exception("出错啦");
        }

        public void Say(string word)
        {
            Console.WriteLine(word);
        }
    }

    class LogInterceptor : Interceptor
    {
        public void Invoke(Invocation invocation)
        {
            try
            {
                Console.WriteLine("pre log");
                invocation.Proceed();
                Console.WriteLine("after log");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                invocation.ReturnValue = "出错啦";
            }
        }
    }

}

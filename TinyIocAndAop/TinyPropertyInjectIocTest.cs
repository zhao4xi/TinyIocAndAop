using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyIocAndAop
{
    class TinyPropertyInjectIocTest
    {
        public static void Test()
        {
            TinyIoc.Register<IServiceA1, ServiceA1>(true);
            TinyIoc.Register<IServiceB1, ServiceB1>(true);

            var serviceA = TinyIoc.ResolveByProp<IServiceA1>();
            serviceA.Say("Hello World");
            Console.Read();
        }
    }

    interface IServiceA1 { void Say(string word); }

    class ServiceA1 : IServiceA1
    {
        public IServiceB1 ServiceB { get; set; }

        public void Say(string word)
        {
            Console.WriteLine("ServiceA1 say: " + word);
            ServiceB.Say(word); 
        }
    }

    interface IServiceB1 { void Say(string word); }

    class ServiceB1 : IServiceB1
    {
        public IServiceA1 ServiceA { get; set; }

        public void Say(string word)
        {
            Console.WriteLine("ServiceB1 say: " + word);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyIocAndAop
{
    class TinyCtorInjectorIocTest
    {
        public static void Test()
        {
            TinyIoc.Register<IServiceA2, ServiceA2>(true);
            TinyIoc.Register<IServiceB2, ServiceB2>(true);
            TinyIoc.Register<IServiceA3, ServiceA3>(true);
            TinyIoc.Register<IServiceB3, ServiceB3>(true);

            try
            {
                var serviceA = TinyIoc.ResolveByCtor<IServiceA2>();
                serviceA.Say("Hello World");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var serviceA3 = TinyIoc.ResolveByCtor<IServiceA3>();
            serviceA3.Say("Hello World");


            Console.Read();
        }
    }

    interface IServiceA2 { void Say(string word); }

    class ServiceA2 : IServiceA2
    {
        private readonly IServiceB2 _serviceB;

        public ServiceA2(IServiceB2 serviceB)
        {
            _serviceB = serviceB;
        }

        public void Say(string word)
        {
            Console.WriteLine("ServiceA2 say: " + word);
            _serviceB.Say(word);
        }
    }

    interface IServiceB2 { void Say(string word); }

    class ServiceB2 : IServiceB2
    {
        private readonly IServiceA2 _serviceA;

        public ServiceB2(IServiceA2 serviceA)
        {
            _serviceA = serviceA;
        }

        public void Say(string word)
        {
            Console.WriteLine("ServiceB2 say: " + word);
        }
    }

    interface IServiceA3 { void Say(string word); }

    class ServiceA3 : IServiceA3
    {
        private readonly IServiceB3 _serviceB;
        public ServiceA3(IServiceB3 serviceB)
        {
            _serviceB = serviceB;
        }

        public void Say(string word)
        {
            Console.WriteLine("ServiceB3 say: " + word);
            _serviceB.Say(word);
        }
    }

    interface IServiceB3 { void Say(string word); }

    class ServiceB3 : IServiceB3
    {

        public ServiceB3()
        {
        }

        public void Say(string word)
        {
            Console.WriteLine("ServiceB3 say: " + word);
        }
    }
}

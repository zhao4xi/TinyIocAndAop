using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyIocAndAop
{
    class Program
    {
        static void Main(string[] args)
        {

            var container = new Cat().Register<IServiceA, ServiceA>(Lifetime.Transient);
            var tt = container.GetService<IServiceA>();
            tt.Say("hello ");
            Console.Read();


            TinyPropertyInjectIocTest.Test();

            TinyCtorInjectorIocTest.Test();

            TinyAopTest.Test();
        }
    }
}

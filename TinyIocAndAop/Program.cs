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
            TinyPropertyInjectIocTest.Test();

            TinyCtorInjectorIocTest.Test();

            TinyAopTest.Test();
        }
    }
}

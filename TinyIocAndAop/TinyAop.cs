using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace TinyIocAndAop
{
    class TinyAop
    {
        public static IT Create<IT, T>(Interceptor interceptors)
        {
            T instance = Activator.CreateInstance<T>();
            RealProxyEx<IT> realProxy = new RealProxyEx<IT>(instance, interceptors);
            IT transparentProxy = (IT)realProxy.GetTransparentProxy();
            return transparentProxy;
        }
    }

    class RealProxyEx<T> : RealProxy
    {
        private object _target;
        private Interceptor _interceptors;

        public RealProxyEx(object target, Interceptor interceptors) : base(typeof(T))
        {
            _target = target;
            _interceptors = interceptors;
        }

        public override IMessage Invoke(IMessage msg)
        {
            Invocation invocation = new Invocation(msg, _target);
            if (_interceptors == null)
            {
                invocation.Proceed();
            }
            else
            {
                _interceptors.Invoke(invocation); 
            }
            return new ReturnMessage(invocation.ReturnValue, new object[0], 0, null, invocation.CallMessage);
        }
    }

    class Invocation
    {
        public MethodBase Method { get; set; }

        public object ReturnValue { get; set; }

        public IMethodCallMessage CallMessage => (IMethodCallMessage)_msg;

        private IMessage _msg;

        private object _target;

        public Invocation(IMessage msg, object target)
        {
            _target = target;
            _msg = msg;
        }

        public void Proceed()
        {
            IMethodCallMessage callMessage = (IMethodCallMessage)_msg;
            Method = callMessage.MethodBase;
            ReturnValue = Method.Invoke(_target, callMessage.Args);
        }
    }

    interface Interceptor
    {
        void Invoke(Invocation invocation);
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyIocAndAop
{
    public class ServiceRegistry
    {
        public Type ServiceType { get; set; }

        public Lifetime Lifetime { get; set; }

        public Func<Cat, Type[], object> Factory { get; set; }

        internal ServiceRegistry Next { get; set; }

        public ServiceRegistry(Type serviceType, Lifetime lifeTime, Func<Cat, Type[], object> factory)
        {
            ServiceType = serviceType;
            Lifetime = lifeTime;
            Factory = factory;
        }

        internal IEnumerable<ServiceRegistry> AsEnumerable()
        {
            var list = new List<ServiceRegistry>();
            for (var self = this; self != null; self = self.Next)
            {
                list.Add(self);
            }
            return list;
        }


    }
}

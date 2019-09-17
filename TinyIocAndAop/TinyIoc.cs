using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TinyIocAndAop
{
    class TinyIoc
    {
        private static Dictionary<Type, Type> _typeImplDic = new Dictionary<Type, Type>();
        private static Dictionary<Type, object> _instanceTypeDic = new Dictionary<Type, object>();
        private static Dictionary<Type, object> _earlyInstanceTypeDic = new Dictionary<Type, object>();

        private static object ResolveByProp(Type typeofT)
        {
            if (_instanceTypeDic.ContainsKey(typeofT))
            {
                return _instanceTypeDic[typeofT];
            }
            if (_earlyInstanceTypeDic.ContainsKey(typeofT))
            {
                return _earlyInstanceTypeDic[typeofT];
            }
            if (!_typeImplDic.ContainsKey(typeofT))
            {
                return null;
            }
            var typeImp = _typeImplDic[typeofT];
            var impObj = Activator.CreateInstance(typeImp);
            _earlyInstanceTypeDic.Add(typeofT, impObj);
            var propes = typeImp.GetProperties();
            foreach (var item in propes)
            {
                item.SetValue(impObj, ResolveByProp(item.PropertyType));
            }
            _instanceTypeDic.Add(typeofT, typeImp);
            return impObj;
        }

        public static T ResolveByProp<T>()
        {
            var typeofT = typeof(T);
            return (T)ResolveByProp(typeofT);
        }

        public static T ResolveByCtor<T>()
        {
            var typeofT = typeof(T);
            List<Type> circleDependency = new List<Type>();
            return (T)ResolveByCtor(typeofT, ref circleDependency);
        }


        private static object ResolveByCtor(Type typeofT, ref List<Type> circleDependency)
        {
            if (_instanceTypeDic.ContainsKey(typeofT))
            {
                return _instanceTypeDic[typeofT];
            }

            if (!_typeImplDic.ContainsKey(typeofT))
            {
                return null;
            }
            if (circleDependency.Contains(typeofT))
            {
                throw new Exception($"循环依赖了: {string.Join("->", circleDependency)}-> {typeofT.ToString()}");
            }
            var typeImp = _typeImplDic[typeofT];
            circleDependency.Add(typeofT);

            var ctor = typeImp.GetConstructors().FirstOrDefault();
            var paramInfo = ctor.GetParameters();
            var parameterValues = new object[paramInfo.Length];
            for (int i = 0; i < parameterValues.Length; i++)
            {
                parameterValues[i] = ResolveByCtor(paramInfo[i].ParameterType, ref circleDependency);
            }
            var result = ctor.Invoke(parameterValues);
            _instanceTypeDic.Add(typeofT, result);
            return result;
        }

        public static void Register<IT, T>(bool singletonInstance = true)
        {
            _typeImplDic.Add(typeof(IT), typeof(T));
        }
    }
}

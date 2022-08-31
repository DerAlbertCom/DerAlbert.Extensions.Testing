using System.Reflection;

namespace DerAlbert.Extensions.Fakes.Internal;

internal class SubjectFactory
{
    private readonly FakeFactory _fakeFactory;

    public SubjectFactory(FakeFactory fakeFactory)
    {
        _fakeFactory = fakeFactory;
    }

    public T Create<T>() where T : class
    {
        return (T) CreateSubject(typeof(T));
    }

    private object CreateSubject(Type type)
    {
        return CreateSubjectInScope(fakeFactory => CreateInstance(type, fakeFactory));
    }

    private object CreateInstance(Type type, FakeFactory fakeFactory)
    {
        var constructor = GetGreediestConstructor(type);

        var parameterTypes = GetParameterTypes(constructor);
        var parameters = new object[parameterTypes.Length];

        for (var index = 0; index < parameterTypes.Length; index++)
        {
            var parameterType = parameterTypes[index];
            if (parameterType.GetTypeInfo().IsClass && !parameterType.GetTypeInfo().IsAbstract)
            {
                parameters[index] = GetConcreteInstance(parameterType, fakeFactory);
            }
            else
            {
                parameters[index] = fakeFactory.The(parameterType);
            }
        }
        return constructor.Invoke(parameters);
    }

    private Object GetConcreteInstance(Type parameterType, FakeFactory fakeFactory)
    {
        if (fakeFactory.TryExistingThe(parameterType, out var existingInstance))
        {
            return existingInstance;
        }
        var newInstance = fakeFactory.CreateInstance(parameterType, false) ?? CreateSubject(parameterType);
        fakeFactory.Inject(parameterType, newInstance);
        return newInstance;
    }

    private ConstructorInfo GetGreediestConstructor(Type type)
    {
        var maxParameters = type.GetConstructors().Max(ci => ci.GetParameters().Length);

        return type.GetConstructors().First(ci => ci.GetParameters().Length >= maxParameters);
    }

    private Type[] GetParameterTypes(MethodBase constructorInfo)
    {
        var constructorParameterTypes =
            from pi in constructorInfo.GetParameters()
            select pi.ParameterType;

        return constructorParameterTypes.ToArray();
    }

    private object CreateSubjectInScope(Func<FakeFactory, object> callback)
    {
        return _fakeFactory.CallbackInServiceScope(callback);
    }
        
        
}
using Xunit.Abstractions;

namespace DerAlbert.Extensions.Fakes.Tests;

public class SubjecTestClassToInject
{
    public IAttributeInfo One { get; }

    public SubjecTestClassToInject(IAttributeInfo one)
    {
        One = one;
    }
}
public class SubjectTestClass
{
    public IAttributeInfo One { get; }
    public IAttributeInfo Two { get; }
    public IAssemblyInfo Info { get; }
    public SubjecTestClassToInject Injected { get; }

    public SubjectTestClass(IAttributeInfo one, IAttributeInfo two, IAssemblyInfo info, SubjecTestClassToInject injected)
    {
        One = one;
        Two = two;
        Info = info;
        Injected = injected;
    }
}
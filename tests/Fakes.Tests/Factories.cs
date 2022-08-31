using DerAlbert.Extensions.Fakes.Internal;

namespace DerAlbert.Extensions.Fakes.Tests;

internal class Factories
{
    public SubjectFactory CreateSubjectFactory(FakeMode fakeMode)
    {
        var factories = CreateFactories(fakeMode);
        return factories.subjectFactory;
    }

    public FakeFactory CreateFakeFactory(FakeMode fakeMode)
    {
        return new FakeFactory(new FakeServiceCollection(fakeMode));
    }
    

    public (SubjectFactory subjectFactory, FakeFactory fakeFactory) CreateFactories(FakeMode fakeMode)
    {
        var fakeFactory = CreateFakeFactory(fakeMode);
        var subjectFactory = new SubjectFactory(fakeFactory);

        return (subjectFactory, fakeFactory);
    }
}
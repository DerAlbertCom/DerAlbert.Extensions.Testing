using DerAlbert.Extensions.Fakes.Internal;

namespace DerAlbert.Extensions.Fakes.Tests;

internal class Factories
{
    public SubjectFactory CreateSubjectFactory()
    {
        var factories = CreateFactories();
        return factories.subjectFactory;
    }

    public FakeFactory CreateFakeFactory()
    {
        return new FakeFactory(new FakeServiceCollection());
    }


    public (SubjectFactory subjectFactory, FakeFactory fakeFactory) CreateFactories()
    {
        var fakeFactory = CreateFakeFactory();
        var subjectFactory = new SubjectFactory(fakeFactory);

        return (subjectFactory, fakeFactory);
    }
}

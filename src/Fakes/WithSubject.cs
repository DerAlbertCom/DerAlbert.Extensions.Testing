using JetBrains.Annotations;

namespace DerAlbert.Extensions.Fakes;

/// <summary>
/// Enables to easy generate a Subject under test which automaticly resolving
/// the dependencies in the constructor. You are abled to configure the fakes which
/// a needed in the constructor.
/// Also you can add concrete Services to the ServiceCollection.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class WithSubject<T> : WithFakes where T : class
{
    private readonly Lazy<T> _lazySubject;

    /// <inheritdoc />
    protected WithSubject() : base()
    {
        _lazySubject = new Lazy<T>(CreateSubject);
    }

    protected WithSubject(FakeMode fakeMode) : base(fakeMode)
    {
        _lazySubject = new Lazy<T>(CreateSubject);
    }

    private T CreateSubject()
    {
        var subject = CreateSubjectUnderTest<T>();
        ModifySubject(subject);
        return subject;
    }

    /// <summary>
    /// Overload this method if yoy need the modfiy the created subject
    /// after creating while accessing the *Subject*
    /// </summary>
    /// <param name="subject"></param>
    protected virtual void ModifySubject(T subject)
    {
    }

    /// <summary>
    /// Get the instance of subject under test. It will be created on the
    /// first access, after access the Subject you are not able to configure
    /// the dependencys which are injected in the constructor
    /// </summary>
    [NotNull]
    protected T Subject => _lazySubject.Value;
}
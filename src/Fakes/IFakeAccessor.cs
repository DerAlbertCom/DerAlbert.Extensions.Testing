using Microsoft.Extensions.DependencyInjection;

namespace DerAlbert.Extensions.Fakes;

/// <summary>
/// 
/// </summary>
public interface IFakeAccessor
{
    /// <summary>
    /// Get's the only one instance of the supplied, this is the same instance which
    /// is injected in the constructor
    /// </summary>
    /// <typeparam name="TMock"></typeparam>
    /// <returns></returns>
    TMock The<TMock>() where TMock : class;

    /// <summary>
    /// Creates a fresh instance of suplied type.
    /// </summary>
    /// <typeparam name="TMock"></typeparam>
    /// <returns></returns>
    TMock An<TMock>() where TMock : class;

    /// <summary>
    /// Inject a concrete instance for the given type, will be used for constructor injection
    /// </summary>
    /// <typeparam name="TMock"></typeparam>
    /// <param name="instance"></param>
    void Inject<TMock>(TMock instance) where TMock : class;

    /// <summary>
    /// A ServiceCollection, add concrete Services in the Dependency Injection system which 
    /// get's resolved when automaticaly 
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Add Behaviors to the dependency configuration, can be used reuse Fake Configuration
    /// in differnt tests. Each Behavior wil be executed immediately and will only runs one.
    /// You can add different Behavior
    /// </summary>
    /// <typeparam name="TBehavior">a behaviour which is used</typeparam>
    /// <param name="parameters">These parameters will be passed as additional parameters to the constructor 
    /// of the behavior</param>
    /// <returns>The instance of the behavior</returns>
    TBehavior With<TBehavior>(params object[] parameters) where TBehavior : class, IMockBehavior;
}
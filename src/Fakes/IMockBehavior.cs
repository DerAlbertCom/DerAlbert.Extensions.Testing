namespace DerAlbert.Extensions.Fakes;

/// <summary>
/// For creating behaviors
/// </summary>
public interface IMockBehavior
{
    /// <summary>
    /// Will be called before creating the Subject
    /// </summary>
    void OnEstablish();
}
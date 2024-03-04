public interface IWindowObject
{
    public string Patch { get; }

    public object InstanceObject() => this;
}
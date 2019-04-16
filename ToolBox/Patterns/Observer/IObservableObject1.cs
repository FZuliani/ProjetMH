namespace ToolBox.Patterns.Observer
{
    public interface IObservableObject
    {
        event System.Action<IObservableObject> PropertyChanged;
    }
}
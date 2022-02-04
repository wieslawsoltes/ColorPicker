using ReactiveUI;

namespace ThemeEditor.ViewModels;

public interface IColor : IReactiveNotifyPropertyChanged<IReactiveObject>, IReactiveObject
{
    IColor Clone();
}
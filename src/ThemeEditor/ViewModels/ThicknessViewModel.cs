using System;
using System.Runtime.Serialization;
using ReactiveUI;

namespace ThemeEditor.ViewModels;

[DataContract]
public class ThicknessViewModel : ReactiveObject
{
    private double _left;
    private double _top;
    private double _right;
    private double _bottom;

    [IgnoreDataMember]
    public string Text
    {
        get { return this.ToTextString(); }
        set
        {
            if (value != null)
            {
                try
                {
                    value.FromTextString(out _left, out _top, out _right, out _bottom);
                    this.RaisePropertyChanged(nameof(Left));
                    this.RaisePropertyChanged(nameof(Top));
                    this.RaisePropertyChanged(nameof(Right));
                    this.RaisePropertyChanged(nameof(Bottom));
                }
                catch (Exception) { }
            }
        }
    }

    [DataMember]
    public double Left
    {
        get { return _left; }
        set
        {
            this.RaiseAndSetIfChanged(ref _left, value);
            this.RaisePropertyChanged(nameof(Text));
        }
    }

    [DataMember]
    public double Top
    {
        get { return _top; }
        set
        {
            this.RaiseAndSetIfChanged(ref _top, value);
            this.RaisePropertyChanged(nameof(Text));
        }
    }

    [DataMember]
    public double Right
    {
        get { return _right; }
        set
        {
            this.RaiseAndSetIfChanged(ref _right, value);
            this.RaisePropertyChanged(nameof(Text));
        }
    }

    [DataMember]
    public double Bottom
    {
        get { return _bottom; }
        set
        {
            this.RaiseAndSetIfChanged(ref _bottom, value);
            this.RaisePropertyChanged(nameof(Text));
        }
    }

    public ThicknessViewModel Clone()
    {
        return new ThicknessViewModel()
        {
            Left = this.Left,
            Top = this.Top,
            Right = this.Right,
            Bottom = this.Bottom
        };
    }
}
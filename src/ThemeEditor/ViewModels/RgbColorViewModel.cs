using System.Runtime.Serialization;
using ReactiveUI;

namespace ThemeEditor.ViewModels
{
    [DataContract]
    public class RgbColorViewModel : ReactiveObject, IColor
    {
        private byte _r;
        private byte _g;
        private byte _b;

        //[IgnoreDataMember]
        //public string Hex
        //{
        //    get { return this.ToHexString(); }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            try
        //            {
        //                value.FromHexString(out _r, out _g, out _b);
        //                this.RaisePropertyChanged(nameof(R));
        //                this.RaisePropertyChanged(nameof(G));
        //                this.RaisePropertyChanged(nameof(B));
        //            }
        //            catch (Exception) { }
        //        }
        //    }
        //}

        [DataMember]
        public byte R
        {
            get { return _r; }
            set
            {
                this.RaiseAndSetIfChanged(ref _r, value);
                //this.RaisePropertyChanged(nameof(Hex));
            }
        }

        [DataMember]
        public byte G
        {
            get { return _g; }
            set
            {
                this.RaiseAndSetIfChanged(ref _g, value);
                //this.RaisePropertyChanged(nameof(Hex));
            }
        }

        [DataMember]
        public byte B
        {
            get { return _b; }
            set
            {
                this.RaiseAndSetIfChanged(ref _b, value);
                //this.RaisePropertyChanged(nameof(Hex));
            }
        }

        public IColor Clone()
        {
            return new RgbColorViewModel()
            {
                R = this.R,
                G = this.G,
                B = this.B
            };
        }
    }
}

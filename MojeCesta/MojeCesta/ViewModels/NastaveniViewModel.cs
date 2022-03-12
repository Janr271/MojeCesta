using System.ComponentModel;
using MojeCesta.Services;

namespace MojeCesta.ViewModels
{
    public class NastaveniViewModel : INotifyPropertyChanged
    {
        public NastaveniViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Rezim
        {
            get => Promenne.Rezim;

            set
            {
                if (Promenne.Rezim == value)
                    return;

                Promenne.Rezim = value;
                OnPropertyChanged(nameof(Rezim));
            }
        }

        public bool PouzeWifi
        {
            get => AktualizaceDat.PouzeWifi;

            set
            {
                if (AktualizaceDat.PouzeWifi == value)
                    return;

                AktualizaceDat.PouzeWifi = value;
                OnPropertyChanged(nameof(PouzeWifi));
            }
        }

        public bool AutomatickaAktualizace
        {
            get => AktualizaceDat.AutomatickaAktualizace;

            set
            {
                if (AktualizaceDat.AutomatickaAktualizace == value)
                    return;

                AktualizaceDat.AutomatickaAktualizace = value;
                OnPropertyChanged(nameof(AutomatickaAktualizace));
            }
        }


        public string PosledniAktualizace
        {
            get
            {
                if (AktualizaceDat.PosledniAktualizace != null)
                {
                    return AktualizaceDat.PosledniAktualizace.Value.ToShortDateString();
                }
                else
                {
                    return "nikdy";
                }
            }
        }

        public string PlatnostDat
        {
            get
            {
                if(AktualizaceDat.PosledniAktualizace != null)
                {
                    return Database.InformaceODatabazi().Result.Feed_end_date.ToShortDateString();
                }
                else
                {
                    return "nikdy";
                }
            }
        }

        void OnPropertyChanged(string jmeno)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(jmeno));
        }
    }
}
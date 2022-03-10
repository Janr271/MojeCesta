using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using MojeCesta.Services;

namespace MojeCesta.ViewModels
{
    public class NastaveniViewModel : INotifyPropertyChanged
    {
        public NastaveniViewModel()
        {
            Aktualizovat = new Command(() => AktualizaceDat.Aktualizovat(true));
        }

        public Command Aktualizovat { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Rezim
        {
            get => Models.GlobalniPromenne.Rezim;

            set
            {
                if (Models.GlobalniPromenne.Rezim == value)
                    return;

                Models.GlobalniPromenne.Rezim = value;
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
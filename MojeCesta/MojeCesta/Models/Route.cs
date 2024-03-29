﻿using SQLite;
using FileHelpers;
using System.Collections.Generic;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    public class Route
    {
        [PrimaryKey]
        public string Route_id { get; set; } 
        public string Agency_id { get; set; } 
        public string Route_short_name { get; set; } 
        [FieldQuoted]
        public string Route_long_name { get; set; } 
        public RouteType Route_type { get; set; } 
        [FieldQuoted]
        public string Route_url { get; set; } 
        public string Route_color { get; set; } 
        public string Route_text_color { get; set; } 
        public bool Is_night { get; set; } 
        public bool Is_regional { get; set; }
        public bool Is_substitute_transport { get; set; }

        [Ignore, FieldHidden]
        public Route_stop[] Zastavky { get; set; }

        [Ignore, FieldHidden]
        public Route_stop.Direction Smer { get; set; }

        [Ignore, FieldHidden]
        public int Navstiveno { get; set; }

        [Ignore, FieldHidden]
        public bool Pesky { get; set; }

        [Ignore, FieldHidden]
        public List<string> DnesniSpoje { get; set; }

        public Route()
        {

        }

        public Route(Route r)
        {
            Route_id = r.Route_id;
            Route_short_name = r.Route_short_name;

        }

        public Route(Route_stop[] zastavky, Route_stop.Direction smer, Route r, bool pesky)
        {
            Zastavky = zastavky;
            Smer = smer;
            Navstiveno = zastavky.Length;
            Pesky = pesky;
            Route_id = r.Route_id;
            Route_short_name = r.Route_short_name;
        }

        public enum RouteType : ushort
        {
            Tram = 0,
            Metro = 1,
            Rail = 2,
            Bus = 3,
            Ferry = 4,
            CableTram = 5,
            CableCar = 6,
            Funicular = 7,
            Trolleybus = 11,
            Monorail = 12
        }
    }
}

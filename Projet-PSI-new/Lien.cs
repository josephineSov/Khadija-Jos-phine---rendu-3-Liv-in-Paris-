using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Projet_PSI_new
{

    public class Lien
    {
        public Noeud source;
        public Noeud destination;
        public double distance;
        public double temps;
        public const double VITESSE_METRO = 30.0;
        public Lien(Noeud source, Noeud destination)
        {
            this.source = source;
            this.destination = destination;
            this.distance = CalculerDistance(source, destination);
            this.temps = CalculerTemps(this.distance);

        }

        public double CalculerTemps(double distance)
        {
            return (distance / VITESSE_METRO) * 60;
        }
        public double CalculerDistance(Noeud a, Noeud b)
        {
            const double R = 6371;
            double lat1 = a.Lat * Math.PI / 180;
            double lon1 = a.Lon * Math.PI / 180;
            double lat2 = b.Lat * Math.PI / 180;
            double lon2 = b.Lon * Math.PI / 180;

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            double haversine = Math.Pow(Math.Sin(dLat / 2), 2) +
                               Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dLon / 2), 2);

            double distance = 2 * R * Math.Asin(Math.Sqrt(haversine));
            return distance;
        }

        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        public double Temps
        {
            get { return temps; }
            set { temps = value; }
        }
        public Noeud Source
        {
            get { return source; }
            set { source = value; }
        }
        public Noeud Destination
        {
            get { return destination; }
            set { destination = value; }
        }
    }


}

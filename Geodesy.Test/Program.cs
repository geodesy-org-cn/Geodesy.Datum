using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Geodesy.Datum;
using Geodesy.Datum.Frame;
using Geodesy.Datum.Earth;
using Geodesy.Datum.Time;
using Geodesy.Datum.Coordinate;
using Geodesy.Datum.Transformation;
using Geodesy.Datum.Earth.Projection;
using Geodesy.Datum.Earth.GeodeticProblem;

namespace Geodesy.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            GeoPoint point = new GeoPoint(new Latitude(0), new Longitude(0), Ellipsoid.CGCS2000);
            Angle angle = new Angle(90, 0, 0.01);
            Bessel bessel = new Bessel(point, 1000.0, angle);
            Gauss gauss = new Gauss(point, 1000.0, angle);
            Vincenty vincenty = new Vincenty(point, 1000.0, angle);
            
            TransParameters trans = new TransParameters(null, null, -15.415, 157.025, 94.74, -1.465, 0.312, 0.08, 0.102);
            BursaWolf bursa = new BursaWolf(trans);

            GeodeticCoord pnt1 = new GeodeticCoord(new Latitude(35), new Longitude(100), 0);
            SpaceRectangularCoord xyz=  Conversion.BLH_XYZ(Ellipsoid.Krassovsky, pnt1);
            xyz = bursa.Transform(xyz);
            GeodeticCoord p11 = Conversion.XYZ_BLH(Ellipsoid.WGS84, xyz);

            GeodeticCoord pnt2 = new GeodeticCoord(new Latitude(35), new Longitude(100), 3000);
            xyz = Conversion.BLH_XYZ(Ellipsoid.Krassovsky, pnt2);
            xyz = bursa.Transform(xyz);
            GeodeticCoord p22 = Conversion.XYZ_BLH(Ellipsoid.WGS84, xyz);

            double d = Math.Abs(pnt1.Height - pnt2.Height) - Math.Abs(p11.Height - p22.Height);

            //Origin origin = new Origin(lat, lng, 123);
            //object[] value = origin.GetPoint();
            //Longitude lg = (Longitude)value[1];

            //string json = JsonConvert.SerializeObject(Ellipsoid.CGCS2000);
            //Ellipsoid ellip = JsonConvert.DeserializeObject<Ellipsoid>(json);

            //GeoPoint point = new GeoPoint(lat, lng, ellip);
            //json = JsonConvert.SerializeObject(point);
            //GeoPoint pnt = JsonConvert.DeserializeObject<GeoPoint>(json);

            //Bessel bessel = new Bessel(new GeoPoint(lat, lng), 1000, new Angle(12, 23, 34), Ellipsoid.CGCS2000);
            //Angle a = bessel.Bearing;

            //Ellipsoid w84 = new Ellipsoid(6378137, 298.257222101, 7.292115e-5, 3.986004418e14);
            //Ellipsoid g84 = new Ellipsoid(6378137.0, 1.082629832258e-3, Angle.FromRadians(7.292115e-5), 3.986004418e14);

            //double ep = w84.ivf - g84.ivf;

            //Ellipsoid ellipsoid = Ellipsoid.WGS84;
            //GaussKrueger gauss = new GaussKrueger(new Ellipsoid(6378140, 298.257));
            //gauss.Inverse(2280131, 465804, out Latitude lat, out Longitude lng);
            //lng += new Angle(111);

            //string aa = lat.Degrees.ToString() +"\\" +lng.Degrees.ToString();

            //Dictionary<ProjectionParameter, double> parameters = new Dictionary<ProjectionParameter, double>();
            //parameters.Add(ProjectionParameter.SemiMajor, Ellipsoid.WGS84.a);
            //parameters.Add(ProjectionParameter.InverseFlattening, -1);
            //parameters.Add(ProjectionParameter.FalseEasting, 5.0);
            //parameters.Add(ProjectionParameter.FalseNorthing, 6.0);
            //parameters.Add(ProjectionParameter.CenterMeridian, 120.0);
            //parameters.Add(ProjectionParameter.LatitudeOfOrigin, 30.0);

            //CassiniSoldner cassini = new CassiniSoldner(parameters);
            //cassini.Forward(new Latitude(34), new Longitude(123), out double northing, out double easting);
            //cassini.Inverse(northing, easting, out Latitude lat, out Longitude lng);
        }
    }
}
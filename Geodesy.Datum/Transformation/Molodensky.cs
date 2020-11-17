using System;
using Geodesy.Datum.Earth;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Transformation
{
    /// <summary>
    /// Molodensky transformation
    /// </summary>
    public static class Molodensky
    {
        /// <summary>
        /// transform the geodetic datum
        /// </summary>
        /// <param name="point">geodetic point</param>
        /// <param name="from">source ellipsoid</param>
        /// <param name="to">target ellipsoid</param>
        /// <param name="para">three translation parameters</param>
        /// <returns>geodetic point in new datum</returns>
        public static GeodeticCoord Transform(GeodeticCoord point, Ellipsoid from, Ellipsoid to, TransParameters para)
        {
            double B = point.Latitude.Radians;
            double L = point.Longitude.Radians;
            double H = point.Height;

            double sinB = Math.Sin(B);
            double cosB = Math.Cos(B);
            double sinL = Math.Sin(L);
            double cosL = Math.Cos(L);

            double a = from.SemiMajorAxis;
            double b_a = 1 - from.Flattening;
            double da = to.SemiMajorAxis - from.SemiMajorAxis;
            double df = to.Flattening - from.Flattening;
            
            double ee = from.ee;
            double w = 1.0 - ee * sinB * sinB;
            double Rm = a * (1.0 - ee) / Math.Pow(w, 1.5);
            double Rn = a / Math.Sqrt(w);

            // formula is from http://earth-info.nga.mil/GandG/coordsys/datums/standardmolodensky.html
            double dB = (-para.Tx * sinB * cosL - para.Ty * sinB * sinL + para.Tz * cosB 
                         + da * (Rn * ee * sinB * cosB) / a 
                         + df * (Rm / b_a + Rn * b_a) * sinB * cosB) / (Rm + H);
            double dL = (-para.Tx * sinL + para.Ty * cosL) / ((Rn + H) * cosB);
            double dH = para.Tx * cosB * cosL + para.Ty * cosB * sinL + para.Tz * sinB
                        - da * a / Rn + df * Rn * b_a * sinB * sinB;

            return new GeodeticCoord(Latitude.FromRadians(B + dB), Longitude.FromRadians(L + dL), H + dH);
        }

        /// <summary>
        /// transform the geodetic datum
        /// </summary>
        /// <param name="point">geodetic point</param>
        /// <param name="ellipsoid">source ellipsoid</param>
        /// <param name="da">semi-major difference of ellipsoids</param>
        /// <param name="df">flattening difference of ellipsoids</param>
        /// <param name="dX">translation along the X-axis</param>
        /// <param name="dY">translation along the Y-axis</param>
        /// <param name="dZ">translation along the Z-axis</param>
        /// <returns>geodetic point in new datum</returns>
        public static GeodeticCoord Transform(GeodeticCoord point, Ellipsoid ellipsoid, double da, double df, double dX, double dY, double dZ)
        {
            double B = point.Latitude.Radians;
            double L = point.Longitude.Radians;
            double H = point.Height;

            double sinB = Math.Sin(B);
            double cosB = Math.Cos(B);
            double sinL = Math.Sin(L);
            double cosL = Math.Cos(L);

            double a = ellipsoid.SemiMajorAxis;
            double b_a = 1 - ellipsoid.Flattening;

            double ee = ellipsoid.ee;
            double w = 1.0 - ee * sinB * sinB;
            double Rm = a * (1.0 - ee) / Math.Pow(w, 1.5);
            double Rn = a / Math.Sqrt(w);

            // formula is from http://earth-info.nga.mil/GandG/coordsys/datums/standardmolodensky.html
            double dB = (-dX * sinB * cosL - dY * sinB * sinL + dZ * cosB
                         + da * (Rn * ee * sinB * cosB) / a
                         + df * (Rm / b_a + Rn * b_a) * sinB * cosB) / (Rm + H);
            double dL = (-dX * sinL + dY * cosL) / ((Rn + H) * cosB);
            double dH = dX * cosB * cosL + dY * cosB * sinL + dZ * sinB
                        - da * a / Rn + df * Rn * b_a * sinB * sinB;

            return new GeodeticCoord(Latitude.FromRadians(B + dB), Longitude.FromRadians(L + dL), H + dH);
        }
    }
}

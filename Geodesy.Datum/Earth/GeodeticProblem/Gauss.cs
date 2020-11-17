using System;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth.GeodeticProblem
{
    /// <summary>
    /// Gauss mid-latitude formula of geodetic problem solution
    /// </summary>
    /// <remarks>
    /// The formula refers to Wuan University PPT
    /// </remarks>
    public sealed class Gauss : GeodesicSolution
    {
        public Gauss()
        { }
        
        /// <summary>
        /// Create a class of geodetic problem with Gauss mid-latitude solution
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="distance">distance of geodesic</param>
        /// <param name="bearing">azimuth of geodesic</param>
        public Gauss(GeoPoint start, double distance, Angle bearing)
            : base(start, distance, bearing)
        { }

        /// <summary>
        /// Create a class of geodetic problem with Gauss mid-latitude solution
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="end">end point of geodesic</param>
        public Gauss(GeoPoint start, GeoPoint end)
            : base(start, end)
        { }

        /// <summary>
        /// Directed solution of geodetic problem from start point, distance and azimuth to end point and inverse azimuth.
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="distance">distance of geodesic</param>
        /// <param name="bearing">azimuth of geodesic</param>
        /// <param name="end">end point of geodesic</param>
        /// <param name="ivBearing">inverse azimuth of geodesic</param>
        protected override void Direct(GeoPoint start, double distance, Angle bearing, out GeoPoint end, out Angle ivBearing)
        {
            double ses = start.Ellipsoid._ee;
            double c = start.Ellipsoid.c;

            double B0 = start.Latitude.Radians;
            double L0 = start.Longitude.Radians;
            double A0 = bearing.Radians;

            double sA = Math.Sin(A0);
            double cA = Math.Cos(A0);
            double cB = Math.Cos(B0);
            double tB = Math.Tan(B0);

            double V = Math.Sqrt(1 + ses * cB * cB);
            double SN = distance / c * V;
            double dL = SN * sA / cB;
            double dB = SN * cA * V * V;
            double dA = SN * sA * tB;

            double Bm = B0 + dB / 2;
            double Lm = L0 + dL / 2;
            double Am = A0 + dA / 2;

            while (Math.Abs(Am - A0) > Settings.Epsilon4 || Math.Abs(Lm - L0) > Settings.Epsilon4 || Math.Abs(Am - A0) > Settings.Epsilon4)
            {
                A0 = Am;
                B0 = Bm;
                L0 = Lm;

                sA = Math.Sin(A0);
                cA = Math.Cos(A0);
                cB = Math.Cos(B0);
                tB = Math.Tan(B0);

                V = Math.Sqrt(1 + ses * cB * cB);
                double t2 = tB * tB;
                double eta2 = ses * cB * cB;
                SN = distance / c * V;

                dB = (1 + SN * SN / 24 * (sA * sA * (2 + 3 * t2 + 2 * eta2)
                        - 3 * cA * cA * eta2 * (t2 - 1 - eta2 + 4 * eta2 * t2))) * SN * V * V * cA;
                dL = (1 + SN * SN / 24 * (t2 * sA * sA - cA * cA * (1 + eta2 - 9 * eta2 * t2))) * SN * sA / cB;
                dA = (1 + SN * SN / 24 * (cA * cA * (2 + 7 * eta2 + 9 * eta2 * t2 + 5 * eta2 * eta2)
                        + sA * sA * (2 + t2 + 2 * eta2))) * SN * sA * tB;

                Bm = start.Latitude.Radians + dB / 2;
                Lm = start.Longitude.Radians + dL / 2;
                Am = bearing.Radians + dA / 2;
            }
            end = new GeoPoint(start.Latitude + Angle.FromRadians(dB), start.Longitude + Angle.FromRadians(dL));

            ivBearing = bearing + Angle.FromRadians(dA);
            ivBearing += (bearing < Angle.Pi) ? -Angle.Pi : Angle.Pi;
            ivBearing.Normalize();
        }

        /// <summary>
        /// Inverse solution of geodetic problem from start point, end point to the distance and azimuth.
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="end">end point of geodesic</param>
        /// <param name="distance">distance of geodesic</param>
        /// <param name="bearing">azimuth of geodesic</param>
        /// <param name="ivBearing">inverse azimuth of geodesic</param>
        protected override void Inverse(GeoPoint start, GeoPoint end, out double distance, out Angle bearing, out Angle ivBearing)
        {
            double a = start.Ellipsoid.a;
            double es = start.Ellipsoid.ee;
            double ses = start.Ellipsoid._ee;

            double Bm = (start.Latitude.Radians + end.Latitude.Radians) / 2;
            double dB = end.Latitude.Radians - start.Latitude.Radians;
            double dL = end.Longitude.Radians - start.Longitude.Radians;

            double cB = Math.Cos(Bm);
            double sB = Math.Sin(Bm);
            double tB = Math.Tan(Bm);

            double t2 = tB * tB;
            double eta2 = ses * cB * cB;
            double V2 = 1 + ses * cB * cB;
            double N = a / Math.Sqrt(1 - es * sB * sB);

            double r01 = N * cB;
            double r21 = N * cB * (1 + eta2 - 9 * eta2 * t2) / 24;
            double r03 = -N * cB * cB * cB * t2 / 24;
            double SsinA = r01 * dL + r21 * dB * dB * dL + r03 * dL * dL * dL;

            double s10 = N / V2;
            double s12 = -N * cB * cB * (2 + 3 * t2 + 3 * t2 * eta2) / 24;
            double s30 = N * (eta2 - t2 * eta2) / 8;
            double ScosA = s10 * dB + s12 * dB * dL * dL + s30 * dB * dB * dB;

            double Am;
            if (Math.Abs(dB) >= Math.Abs(dL))
            {
                Am = Math.Atan(Math.Abs(SsinA / ScosA));
            }
            else
            {
                double cotAm = Math.Abs(ScosA / SsinA);
                Am = Math.PI / 4 + Math.Atan((1 - cotAm) / (1 + cotAm));
            }

            if (dB < 0 && dL >= 0)
            {
                Am = Math.PI - Am;
            }
            else if (dB <= 0 && dL < 0)
            {
                Am = Math.PI + Am;
            }
            else
            if (dB > 0 && dL < 0)
            {
                Am = 2 * Math.PI - Am;
            }
            else if (dB == 0 && dL > 0)
            {
                Am = Math.PI / 2;
            }
            distance = SsinA / Math.Sin(Am);

            double t01 = tB * cB;
            double t21 = cB * tB * (3 + 2 * eta2 - 2 * eta2 * eta2) / 24;
            double t03 = cB * cB * cB * tB * (1 + eta2) / 12;
            double dA = t01 * dL + t21 * dB * dB * dL + t03 * dL * dL * dL;

            double A1 = Am - dA / 2;
            bearing = Angle.FromRadians(A1);
            bearing.Normalize();

            double A2 = Am + dA / 2;
            A2 += (A1 > Math.PI) ? Math.PI : -Math.PI;
            ivBearing = Angle.FromRadians(A2);
            ivBearing.Normalize();
        } 
    }
}

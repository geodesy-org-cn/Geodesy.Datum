using System;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth.GeodeticProblem
{
    /// <summary>
    /// Bessel's geodetic problem solution
    /// </summary>
    public sealed class Bessel : GeodesicSolution
    {
        public Bessel()
        { }

        /// <summary>
        /// Create a class of geodetic problem with Bessel solution
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="distance">distance of geodesic</param>
        /// <param name="bearing">azimuth of geodesic</param>
        public Bessel(GeoPoint start, double distance, Angle bearing)
            : base(start, distance, bearing)
        { }

        /// <summary>
        /// Create a class of geodetic problem with Bessel solution
        /// </summary>
        /// <param name="start">start point of geodesic</param>
        /// <param name="end">end point of geodesic</param>
        public Bessel(GeoPoint start, GeoPoint end)
            : base(start, end)
        { }

        #region Implement of Bessel solution algorithm
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
            double a = start.Ellipsoid.a;
            double es = start.Ellipsoid.ee;
            double ses = start.Ellipsoid._ee;

            double B1 = start.Latitude.Radians;
            double L1 = start.Longitude.Radians;
            double A1 = bearing.Radians;

            double u1 = Math.Atan(Math.Sqrt(1 - es) * Math.Tan(B1));                             //u1 is (-90, 90)
            double m = Math.Cos(u1) * Math.Sin(A1);                                              //sin(m)
            m = Math.Atan(m / Math.Sqrt(1 - m * m));                                             //m is (-90, 90)
            if (m < 0) m += 2 * Math.PI;

            double M = Math.Atan(Math.Tan(u1) / Math.Cos(A1));                                   //M is (-90, 90)
            if (M < 0) M += Math.PI;                                                             //change M to (0, 180)

            //compute cofficients
            double KK = ses * Math.Pow(Math.Cos(m), 2);
            double alpha = Math.Sqrt(1 + ses) * (1 - KK / 4 + 7 * KK * KK / 64 - 15 * Math.Pow(KK, 3) / 256) / a;
            double beta = KK / 4 - KK * KK / 8 + 37 * Math.Pow(KK, 3) / 512;
            double gamma = KK * KK * (1 - KK) / 128;

            //loop for compute sigma
            double sigma, temp;
            sigma = alpha * distance;
            temp = 0;
            while (Math.Abs(temp - sigma) > Settings.Epsilon5)                                  //精度0.00001秒
            {
                temp = sigma;
                sigma = alpha * distance + beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) + gamma * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma);
            }

            double A2 = Math.Atan(Math.Tan(m) / Math.Cos(M + sigma));                            //A2 is (-90, 90)
            if (A2 < 0) A2 += Math.PI;
            if (A1 < Math.PI) A2 += Math.PI;
            ivBearing = Angle.FromRadians(A2);

            double u2 = Math.Atan(-Math.Cos(A2) * Math.Tan(M + sigma));                          //u2 is (-90, 90)
            Latitude B = Latitude.FromRadians(Math.Atan(Math.Sqrt(1 + ses) * Math.Tan(u2)));     //B in (-90, 90)

            double lamda1;
            lamda1 = Math.Atan(Math.Sin(u1) * Math.Tan(A1));                                     //lamda1
            if (lamda1 < 0) lamda1 += Math.PI;
            if (m >= Math.PI) lamda1 += Math.PI;

            double lamda2;                                                                       //lamda2
            lamda2 = Math.Atan(Math.Sin(u2) * Math.Tan(A2));
            if (Math.Abs(lamda2) < Math.Exp(-15)) lamda2 = 0;                                    //当A2为180度时，上式因舍入误差使得lamda2为负 
            if (lamda2 < 0) lamda2 += Math.PI;
            if (m >= Math.PI)
            {
                if (M + sigma < Math.PI) lamda2 += Math.PI;
            }
            else
            {
                if (M + sigma > Math.PI) lamda2 += Math.PI;
            }

            KK = es * Math.Pow(Math.Cos(m), 2);
            alpha = es / 2 + es * es / 8 + Math.Pow(es, 3) / 16 - es * (1 + es) * KK / 16 + 3 * es * KK * KK / 128;
            beta = es * (1 + es) * KK / 16 - es * KK * KK / 32;
            gamma = es * KK * KK / 256;

            double dL = lamda2 - lamda1 - Math.Sin(m) * (alpha * sigma + beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) + gamma * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma));
            end = new GeoPoint(B, Longitude.FromRadians(L1 + dL));
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
            double lat1 = start.Latitude.Radians;
            double lng1 = start.Longitude.Radians;
            double lat2 = end.Latitude.Radians;
            double lng2 = end.Longitude.Radians;

            // If the two point on the same meridian
            if (Math.Abs(lng1 - lng2) < double.Epsilon)
            {
                Meridian meridian = new Meridian(start.Longitude, start.Latitude, end.Latitude, start.Ellipsoid);
                distance = meridian.Length;
                bearing = meridian.Azimuth;
                ivBearing = meridian.InverseAzimuth;
                return;
            }

            double a = start.Ellipsoid.a;
            double es = start.Ellipsoid.ee;
            double ses = start.Ellipsoid._ee;

            double u1 = Math.Atan(Math.Sqrt(1 - es) * Math.Tan(lat1));
            double u2 = Math.Atan(Math.Sqrt(1 - es) * Math.Tan(lat2));
            double dL = lng2 - lng1;

            double sigma = Math.Sin(u1) * Math.Sin(u2) + Math.Cos(u1) * Math.Cos(u2) * Math.Cos(dL);     //Cos(sigma)
            sigma = Math.Atan(Math.Sqrt(1 - sigma * sigma) / sigma);
            if (sigma < 0) sigma += Math.PI;

            double m = Math.Cos(u1) * Math.Cos(u2) * Math.Sin(dL) / Math.Sin(sigma);                     //Sin(m)
            m = Math.Atan(m / Math.Sqrt(1 - m * m));

            double KK, alpha, beta, gamma;
            double temp = dL;
            double lamda = dL + 0.003351 * sigma * Math.Sin(m);
            double M = Math.PI / 2;

            while (Math.Abs(lamda - temp) > Settings.Epsilon5 )                                        //精度0.00001秒
            {
                temp = lamda;

                m = Math.Cos(u1) * Math.Cos(u2) * Math.Sin(lamda) / Math.Sin(sigma);                     //Sin(m)
                m = Math.Atan(m / Math.Sqrt(1 - m * m));

                double tanA = Math.Sin(lamda) / (Math.Cos(u1) * Math.Tan(u2) - Math.Sin(u1) * Math.Cos(lamda));

                M = Math.Atan(Math.Sin(u1) * tanA / Math.Sin(m));
                if (M < 0) M += Math.PI;

                KK = es * Math.Pow(Math.Cos(m), 2);
                alpha = es / 2 + es * es / 8 + Math.Pow(es, 3) / 16 - es * (1 + es) * KK / 16 + 3 * es * KK * KK / 128;
                beta = es * (1 + es) * KK / 16 - es * KK * KK / 32;
                gamma = es * KK * KK / 256;

                lamda = dL + Math.Sin(m) * (alpha * sigma + beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) + gamma * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma));

                sigma = Math.Sin(u1) * Math.Sin(u2) + Math.Cos(u1) * Math.Cos(u2) * Math.Cos(lamda);     //Cos(sigma)
                sigma = Math.Atan(Math.Sqrt(1 - sigma * sigma) / sigma);
                if (sigma < 0) sigma += Math.PI;
            }

            double A1 = Math.Atan(Math.Sin(lamda) / (Math.Cos(u1) * Math.Tan(u2) - Math.Sin(u1) * Math.Cos(lamda)));
            if (A1 < 0) A1 += Math.PI;
            if (m < 0) A1 += Math.PI;
            bearing = Angle.FromRadians(A1);

            double A2 = Math.Atan(Math.Sin(lamda) / (Math.Sin(u2) * Math.Cos(lamda) - Math.Tan(u1) * Math.Cos(u2)));
            if (A2 < 0) A2 += Math.PI;
            if (m > 0) A2 += Math.PI;
            ivBearing = Angle.FromRadians(A2);

            KK = ses * Math.Pow(Math.Cos(m), 2);
            alpha = Math.Sqrt(1 + ses) * (1 - KK / 4 + 7 * KK * KK / 64 - 15 * Math.Pow(KK, 3) / 256) / a;
            beta = KK / 4 - KK * KK / 8 + 37 * Math.Pow(KK, 3) / 512;
            gamma = KK * KK * (1 - KK) / 128;

            distance = (sigma - beta * Math.Sin(sigma) * Math.Cos(2 * M + sigma) - gamma * Math.Sin(2 * sigma) * Math.Cos(4 * M + 2 * sigma)) / alpha;
        }
        #endregion
    }
}

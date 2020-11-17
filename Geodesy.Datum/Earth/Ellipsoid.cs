using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth
{
    /// <summary>
    /// Earth ellipsoid, the shape of the earth
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Ellipsoid : Spheroid
    {
        #region constructors
        /// <summary>
        /// Create a ellipsoid by a speroid. It is uesd when the ellipsoid is defined by a and b.
        /// </summary>
        /// <param name="spheroid">speriod defined by semi-major axis and semi-minor axis</param>
        /// <param name="name">name of ellipsoid</param>
        /// <param name="alias">abbreviated alias of ellipsoid</param>
        public Ellipsoid(Spheroid spheroid, string name = "", string alias = "")
            : base(spheroid.SemiMajorAxis, spheroid.SemiMinorAxis)
        {
            Name = name;
            Alias = alias;
        }

        /// <summary>
        /// Create an earth ellipsoid
        /// </summary>
        /// <param name="a">semi-major axis</param>
        /// <param name="ivf">inverse of flattening</param>
        /// <param name="name">name of ellipsoid</param>
        /// <param name="alias">abbreviated alias of ellipsoid</param>
        public Ellipsoid(double a, double ivf, string name = "", string alias = "")
            : base(a, a * (1 - 1 / ivf))
        {
            AngularVelocity = double.NaN;
            GM = double.NaN;

            Name = name;
            Alias = alias;

            //计算并存储两个偏心率，因为使用较多，所以就单独存储了
            _ivf = ivf;
            _es = 2 / _ivf - 1 / _ivf / _ivf;
            _ses = _es / (1 - _es);
        }

        /// <summary>
        /// Create an earth ellipsoid
        /// </summary>
        /// <param name="a">semi-major axis</param>
        /// <param name="unit">unit of semi-major axis</param>
        /// <param name="ivf">inverse of flattening</param>
        /// <param name="name">name of ellipsoid</param>
        /// <param name="alias">abbreviated alias of ellipsoid</param>
        public Ellipsoid(double a, LinearUnit unit, double ivf, string name = "", string alias = "")
            : base(a, a * (1 - 1 / ivf))
        {
            AngularVelocity = double.NaN;
            GM = double.NaN;

            Unit = unit;
            Name = name;
            Alias = alias;

            //计算并存储两个偏心率，因为使用较多，所以就单独存储了
            _ivf = ivf;
            _es = 2 / _ivf - 1 / _ivf / _ivf;
            _ses = _es / (1 - _es);
        }

        /// <summary>
        /// Create an earth ellipsoid
        /// </summary>
        /// <param name="a">semi-major axis</param>
        /// <param name="ivf">inverse of flattening</param>
        /// <param name="ω">angular velocity, rad/s</param>
        /// <param name="gm">gravitational static readonly ant</param>
        /// <param name="name">name of ellipsoid</param>
        /// <param name="alias">abbreviated alias of ellipsoid</param>
        public Ellipsoid(double a, double ivf, double ω, double gm, string name = "", string alias = "")
            : base(a, a * (1 - 1 / ivf))
        {
            AngularVelocity = ω;
            GM = gm;

            Name = name;
            Alias = alias;

            //更新父类参数，降低重复计算的舍入误差
            _ivf = ivf;
            _es = 2 / _ivf - 1 / _ivf / _ivf;
            _ses = _es / (1 - _es);
        }

        /// <summary>
        /// Create an earth ellipsoid
        /// </summary>
        /// <param name="a">semi-major axis</param>
        /// <param name="J2">second dynamic form factor</param>
        /// <param name="ω">angular velocity</param>
        /// <param name="gm">gravitational static readonly ant</param>
        /// <param name="name">name of the normal ellipsoid</param>
        /// <param name="alias">abbreviated alias of ellipsoid</param>
        public Ellipsoid(double a, double J2, Angle ω, double gm, string name = "", string alias = "")
            : base(a, a * 297.0 / 298.0)   //父类中的b是近似值，需重新计算
        {
            GM = gm;
            AngularVelocity = ω.Radians;

            Name = name;
            Alias = alias;

            //由J2计算扁率等参数，并更新父类参数
            J2ToIvf(J2);
        }

        /// <summary>
        /// Create an earth ellipsoid
        /// </summary>
        /// <param name="id">identifier</param>
        /// <param name="a">semi-major axis</param>
        /// <param name="C20"></param>
        /// <param name="gm">gravitational static readonly ant</param>
        /// <param name="ω">angular velocity</param>
        /// <param name="name">name of the normal ellipsoid</param>
        /// <param name="alias">abbreviated alias of ellipsoid</param>
        public Ellipsoid(double a, double C20, double gm, Angle ω, string name = "", string alias = "")
            : base(a, a * 297.0 / 298.0)   //父类中的b是近似值，需重新计算
        {
            GM = gm;
            AngularVelocity = ω.Radians;

            Name = name;
            Alias = alias;

            //由J2计算扁率等参数，并更新父类参数
            J2ToIvf(-C20 * Math.Sqrt(5.0));
        }

        /// <summary>
        /// Compute the ivf, e2, e'2 by J2
        /// </summary>
        /// <param name="J2">J2 value</param>
        private void J2ToIvf(double J2)
        {
            double m = 4 * Math.Pow(ω * _a, 2) * _a / GM / 15;

            //迭代初值
            _ivf = 298.2572;
            _es = (2 * _ivf - 1) / _ivf / _ivf;
            double sqe = _es + 1;

            //迭代求解第一偏心率的平方
            int counter = 0;
            while (Math.Abs(_es - sqe) > 1E-15)
            {
                sqe = _es;
                _ses = sqe / (1 - sqe);
                double E = Math.Sqrt(_ses);

                double q0 = (1 + 3 / _ses) * Math.Atan(E) - 3 / E;
                _es = 3 * J2 + m * Math.Pow(sqe, 1.5) / q0;

                // sometimes, it can't converge.
                if (counter++ > 50) break;
            }

            //由第一偏心率求解第二偏心率和扁率倒数
            _ses = _es / (1 - _es);
            _ivf = 1 / (1 - Math.Sqrt(1 - _es));
        }
        #endregion

        #region properties
        /// <summary>
        /// identifier if the ellipsoid
        /// </summary>
        //[JsonProperty(Order = 6, NullValueHandling = NullValueHandling.Ignore)]
        public Identifier Identifier { get; set; } = new Identifier(typeof(Ellipsoid));

        /// <summary>
        /// Name of the ellipsoid
        /// </summary>
        [JsonProperty(Order = 4, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Abbreviated alias of the ellipsoid
        /// </summary>
        [JsonProperty(Order = 5, NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        /// <summary>
        /// 旋转角速度，单位是 radians/sec
        /// </summary>
        public double ω => AngularVelocity;

        /// <summary>
        /// 旋转角速度，单位是 radians/sec
        /// </summary>
        [JsonProperty(Order = 2)]
        public double AngularVelocity { get; set; }

        /// <summary>
        /// 重力常数，即地球质量与万有引力常数之积，单位是 m^3/s^2
        /// </summary>
        [JsonProperty(Order = 3)]
        public double GM { get; set; }

        /// <summary>
        /// 重力常数，即地球总质量与万有引力常数之积，单位是 m^3/s^2
        /// </summary>
        public double GravitationalConst => GM;

        /// <summary>
        /// 以千米为单位的地球质量与万有引力常数之积，单位是 km^3/s^2
        /// </summary>
        public double GM_km => GM / 1.0E9;
        #endregion


        #region Ellipsoid constants from http://en.wikipedia.org/wiki/Earth_ellipsoid
        public static readonly Ellipsoid Spheroid = new Ellipsoid(new Earth.Spheroid(6371000, 6371000))
        {
            Identifier = new Identifier("EPSG", "7035", "SPHERE"),
        };

        /// <summary>
        /// CGCS2000
        /// </summary>
        public static readonly Ellipsoid CGCS2000 = new Ellipsoid(6378137, 298.257222101, 7.292115e-5, 3.986004418e14, "China Geodetic Coordinate System 2000", "CGCS2000")
        {
        };

        /// <summary>
        /// ITRF
        /// </summary>
        public static readonly Ellipsoid ITRF = new Ellipsoid(6378137, 298.2572, 7.292115e-5, 3.986005e14, "International Terrestrial Reference Frame", "ITRF")
        {
        };

        /// <summary>
        /// World Geodetic System 1984
        /// </summary>
        public static readonly Ellipsoid WGS84 = new Ellipsoid(6378137.0, 298.257223563, 7.292115e-5, 3.986004418e14, "World Geodetic System 1984", "WGS84")
        {
            Identifier = new Identifier("EPSG", "7030", "WGS 84", "WGS84"),
        };

        /// <summary>
        /// World Geodetic System 1972
        /// </summary>
        public static readonly Ellipsoid WGS72 = new Ellipsoid(6378135, 298.26, 7.292115147e-5, 3.986005e14, "World Geodetic System 1972", "WGS72")
        {
            Identifier = new Identifier("EPSG", "7043", "WGS 72", "WGS72"),
        };

        /// <summary>
        /// World Geodetic System 1966
        /// </summary>
        public static readonly Ellipsoid WGS66 = new Ellipsoid(6378145, 298.25, "World Geodetic System 1966", "WGS66")
        {
            Identifier = new Identifier("EPSG", "7025", "WGS 66", "WGS66"),
        };

        /// <summary>
        /// PZ-90
        /// </summary>
        public static readonly Ellipsoid PZ90 = new Ellipsoid(6378136, 298.257839303, 7.292115e-5, 3.9860044e14, "Parametry Zemli 1990", "PZ90");

        /// <summary>
        /// IUGG 1967
        /// </summary>
        public static readonly Ellipsoid GRS67 = new Ellipsoid(6378160, 298.247167427, "Geodetic Reference System 1967", "GRS67")
        {
            Identifier = new Identifier("EPSG", "7036", "GRS 1967", "GRS67"),
        };

        /// <summary>
        /// IUGG 1975
        /// </summary>
        public static readonly Ellipsoid GRS75 = new Ellipsoid(6378140, 298.257, 7.292115e-5, 3.986005e14, "Geodetic Reference System 1975", "GRS75")
        {
        };

        /// <summary>
        /// IUGG 1980
        /// </summary>
        /// <remarks>
		/// Adopted by IUGG 1979 Canberra.
		/// Inverse flattening is derived from geocentric gravitational static readonly ant GM = 3986005e8 m*m*m/s/s;
		/// dynamic form factor J2 = 108263e8 and Earth's angular velocity = 7292115e-11 rad/s.
		/// </remarks>
        public static readonly Ellipsoid GRS80 = new Ellipsoid(6378137.0, 1.08263E-3, Angle.FromRadians(7.292115e-5), 3.986005e14, "Geodetic Reference System 1980", "GRS80")
        {
            Identifier = new Identifier("EPSG", "7019", "GRS 1980", "GRS80"),
        };

        /// <summary>
        /// Maupertuis 1738
        /// </summary>
        public static readonly Ellipsoid Maupertuis = new Ellipsoid(6397300, 191.0, "Maupertuis 1738");

        /// <summary>
        /// Plessis 1817
        /// </summary>
        public static readonly Ellipsoid Plessis = new Ellipsoid(6376523.0, 308.64, "Plessis 1817");

        /// <summary>
        /// Hayford 1910
        /// </summary>
        public static readonly Ellipsoid Hayford = new Ellipsoid(6378388, 297.0, "Hayford 1910");

        /// <summary>
        /// New International 1967
        /// </summary>
        public static readonly Ellipsoid International1967 = new Ellipsoid(6378157.5, 298.24961539, "New International 1967");
        #endregion

        #region Ellipsoid constants from http://www.geocachingtoolbox.com/?page=datumEllipsoidDetails
        /// <summary>
        /// Airy 1830
        /// </summary>
        public static readonly Ellipsoid Airy = new Ellipsoid(6377563.396, 299.3249646, "Airy 1830")
        {
            Identifier = new Identifier("EPSG", "7001", "AIRY 1830", "airy"),
        };

        /// <summary>
        /// Airy Modified 1849
        /// </summary>
        public static readonly Ellipsoid AiryModified = new Ellipsoid(6377340.189, 299.3249646, "Airy Modified 1849")
        {
            Identifier = new Identifier("EPSG", "7002", "Airy Modified 1849", "mod_airy"),
        };

        /// <summary>
        /// Australian National
        /// </summary>
        public static readonly Ellipsoid AustralianNational = new Ellipsoid(6378160, 298.25, "Australian National");

        /// <summary>
        /// Bessel 1841
        /// </summary>
        public static readonly Ellipsoid Bessel1841 = new Ellipsoid(6377397.155, 299.1528128, "Bessel 1841")
        {
            Identifier = new Identifier("EPSG", "7004", "Bessel 1841", "Bessel_1841"),
        };

        /// <summary>
        /// Bessel 1841 (Namibia)
        /// </summary>
        public static readonly Ellipsoid BesselNamibia = new Ellipsoid(6377483.865, 299.1528128, "Bessel 1841 (Namibia)")
        {
            Identifier = new Identifier("EPSG", "7046", "Bessel Namibia (GLM)", "bess_nam"),
        };

        /// <summary>
        /// Bessel Modified
        /// </summary>
        public static readonly Ellipsoid BesselModified = new Ellipsoid(6377492.018, 299.1528128, "Bessel Modified");

        /// <summary>
        /// Clarke 1858
        /// </summary>
        public static readonly Ellipsoid Clarke1858 = new Ellipsoid(6378235.6, 294.2606768, "Clarke 1858");

        /// <summary>
        /// Clarke 1866
        /// </summary>
		/// <remarks>
		/// Original definition a=20926062 and b=20855121 (British) feet. Uses Clarke's 1865 inch-metre ratio of 39.370432 to obtain metres.
        /// (Metric value then converted to US survey feet for use in the United States using 39.37 exactly giving a=20925832.16 ft US).
		/// </remarks>
        public static readonly Ellipsoid Clarke1866 = new Ellipsoid(new Spheroid(6378206.4, 6356583.8), "Clarke 1866")
        {
            Identifier = new Identifier("EPSG", "7008", "Clarke 1866", "Clarke_1866"),
        };

        /// <summary>
        /// Clarke 1880
        /// </summary>
        public static readonly Ellipsoid Clarke1880 = new Ellipsoid(6378249.145, 293.465, "Clarke 1880")
        {
        };

        /// <summary>
        /// Clarke 1880 (Arc)
        /// </summary>
        public static readonly Ellipsoid Clarke1880_Arc = new Ellipsoid(6378249.145, 293.4663077, "Clarke 1880 (Arc)")
        {
            Identifier = new Identifier("EPSG", "7013", "Clarke 1880 (Arc)", "Clarke_1880_Arc"),
        };

        /// <summary>
        /// Clarke 1880 (Benoit)
        /// </summary>
        public static readonly Ellipsoid Clarke1880_Benoit = new Ellipsoid(6378300.789, 293.46631553898, "Clarke 1880 (Benoit)");

        /// <summary>
        /// Clarke 1880 (IGN)
        /// </summary>
        public static readonly Ellipsoid Clarke1880_IGN = new Ellipsoid(6378249.2, 293.46602129363, "Clarke 1880 (IGN)")
        {
            Identifier = new Identifier("EPSG", "7011", "Clarke 1880 (IGN)", "Clarke_1880_IGN"),
        };

        /// <summary>
        /// Clarke 1880 (RGS) or Clarke 1880 modified
        /// </summary>
        public static readonly Ellipsoid Clarke1880_RGS = new Ellipsoid(6378249.2, 293.465, "Clarke 1880 (RGS)")
        {
            Identifier = new Identifier("EPSG", "7012", "Clarke 1880 (RGS)", "Clarke_1880_mod"),
        };

        /// <summary>
        /// Everest (Brunei, E. Malaysia (Sabah and Sarawak))
        /// </summary>
        public static readonly Ellipsoid Everest = new Ellipsoid(6377298.556, 300.8017, "Everest (Brunei, E. Malaysia (Sabah and Sarawak))");

        /// <summary>
        /// Everest 1830
        /// </summary>
        public static readonly Ellipsoid Everest1830 = new Ellipsoid(6377276.345, 300.8017, "Everest 1830");

        /// <summary>
        /// Everest 1956 (India and Nepal)
        /// </summary>
        public static readonly Ellipsoid Everest1956 = new Ellipsoid(6377301.243, 300.8017, "Everest 1956 (India and Nepal)");

        /// <summary>
        /// Everest (Pakistan)
        /// </summary>
        public static readonly Ellipsoid Everest_Pakistan = new Ellipsoid(6377309.613, 300.8017, "Everest (Pakistan)");

        /// <summary>
        /// Everest 1948 (W. Malaysia and Singapore)
        /// </summary>
        public static readonly Ellipsoid Everest1948 = new Ellipsoid(6377304.063, 300.8017, "Everest 1948 (W. Malaysia and Singapore)");

        /// <summary>
        /// Everest 1969 (W. Malaysia)
        /// </summary>
        public static readonly Ellipsoid Everest1969 = new Ellipsoid(6377295.664, 300.8017, "Everest 1969 (W. Malaysia)");

        /// <summary>
        /// Helmert 1906
        /// </summary>
        public static readonly Ellipsoid Helmert = new Ellipsoid(6378200, 298.3, "Helmert 1906");

        /// <summary>
        /// Hough 1960
        /// </summary>
        public static readonly Ellipsoid Hough = new Ellipsoid(6378270, 297.0, "Hough 1960");

        /// <summary>
        /// Indonesian 1974
        /// </summary>
        public static readonly Ellipsoid Indonesian = new Ellipsoid(6378160, 298.247, "Indonesian 1974");

        /// <summary>
        /// International 1924 / Hayford 1909
        /// </summary>
		/// <remarks>
		/// Described as a=6378388 m. and b=6356909m. from which 1/f derived to be 296.95926. 
		/// The figure was adopted as the International ellipsoid in 1924 but with 1/f taken as
		/// 297 exactly from which b is derived as 6356911.946m.
		/// </remarks>
        public static readonly Ellipsoid International1924 = new Ellipsoid(6378388, 297.0, "International 1924");

        /// <summary>
        /// Krassovsky 1940
        /// </summary>
        public static readonly Ellipsoid Krassovsky = new Ellipsoid(6378245, 298.3, "Krassovsky 1940")
        {
            Identifier = new Identifier("EPSG", "7024", "Krassowski 1940", "Krassowski_1940"),
        };

        /// <summary>
        /// Modified Fischer 1960
        /// </summary>
        public static readonly Ellipsoid ModifiedFischer = new Ellipsoid(6378155, 298.3, "Modified Fischer 1960");

        /// <summary>
        /// South American 1969
        /// </summary>
        public static readonly Ellipsoid SouthAmerican = new Ellipsoid(6378160, 298.25, "South American 1969");

        /// <summary>
        /// War Office
        /// </summary>
        public static readonly Ellipsoid WarOffice = new Ellipsoid(6378300, 296.0, "War Office");
        #endregion
    }
}

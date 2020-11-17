using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth
{
    /// <summary>
    /// spheroid, the basic class of ellipsoid
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class Spheroid 
    {
        /// <summary>
        /// semi-major axis
        /// </summary>
        protected double _a;

        /// <summary>
        /// inverse of flattening
        /// </summary>
        protected double _ivf;

        /// <summary>
        /// squared of the first eccentricity
        /// </summary>
        protected double _es;

        /// <summary>
        /// squared of the second eccentricity
        /// </summary>
        protected double _ses;

        /// <summary>
        /// Create a spheroid
        /// </summary>
        /// <param name="semiMajor">semi-major axis</param>
        /// <param name="semiMinor">semi-minor axis</param>
        public Spheroid(double semiMajor, double semiMinor)
        {
            _a = semiMajor;

            if (Math.Abs(semiMajor - semiMinor) < double.Epsilon)
            {
                _ivf = double.PositiveInfinity;
                _es = 0;
                _ses = 0;
            }
            else
            {
                _ivf = semiMajor / (semiMajor - semiMinor);
                _es = 1 - Math.Pow(semiMinor / semiMajor, 2);
                _ses = Math.Pow(semiMajor / semiMinor, 2) - 1;
            }
        }

        /// <summary>
        /// Create a spheroid
        /// </summary>
        /// <param name="semiMajor">semi-major axis</param>
        /// <param name="semiMinor">semi-minor axis</param>
        /// <param name="unit">length unit</param>
        public Spheroid(double semiMajor, double semiMinor, LinearUnit unit)
            : this(semiMajor, semiMinor)
        {
            Unit = unit;
        }

        /// <summary>
        /// Unit of the axis length.
        /// </summary>
        public LinearUnit Unit { get; set; } = Settings.BaseLinearUnit;

        #region parameters of ellipsoid
        /// <summary>
        /// semi-major axis
        /// </summary>
        [JsonProperty(Order = 0)]
        public double a
        {
            get => _a;
            set => _a = value;
        }

        /// <summary>
        /// semi-major axis
        /// </summary>
        public double SemiMajorAxis => _a;

        /// <summary>
        /// semi-major axis, km
        /// </summary>
        public double a_km => _a / 1000.0;

        /// <summary>
        /// semi-minor axis
        /// </summary>
        public double b => (_es == 0) ? _a : _a * (_ivf - 1) / _ivf;

        /// <summary>
        /// semi-minor axis
        /// </summary>
        public double SemiMinorAxis => b;

        /// <summary>
        /// polar curvature radius
        /// </summary>
        public double c => (_es == 0) ? _a : _a * _ivf / (_ivf - 1);

        /// <summary>
        /// polar curvature radius
        /// </summary>
        public double PolarCurvatureRadius => c;

        /// <summary>
        /// inverse flattening
        /// </summary>
        [JsonProperty(Order = 1)]
        public double ivf
        {
            get => _ivf;
            set
            {
                _ivf = value;
                if (double.IsInfinity(_ivf))
                {
                    _es = 0;
                    _ses = 0;
                }
                else
                {
                    _es = 2 / _ivf - 1 / _ivf / _ivf;
                    _ses = _es / (1 - _es);
                }
            }
        }

        /// <summary>
        /// inverse flattening
        /// </summary>
        public double InverseFlattening => _ivf;

        /// <summary>
        /// flattening
        /// </summary>
        public double f => 1 / _ivf;

        /// <summary>
        /// flattening
        /// </summary>
        public double Flattening => 1 / _ivf;

        /// <summary>
        /// the second flattening, rarely used.
        /// </summary>
        public double SecondFlattening => 1 / (_ivf - 1);

        /// <summary>
        /// the third flattening
        /// </summary>
        public double n => 1 / (2 * _ivf - 1);

        /// <summary>
        /// the third flattening
        /// </summary>
        public double ThirdFlattening => 1 / (2 * _ivf - 1);

        /// <summary>
        /// the first eccentricity
        /// </summary>
        public double e => Math.Sqrt(_es);

        /// <summary>
        /// the squared first eccentricity
        /// </summary>
        public double ee => _es;

        /// <summary>
        /// the first eccentricity
        /// </summary>
        public double FirstEccentricity => Math.Sqrt(_es);

        /// <summary>
        /// square of the first eccentricity
        /// </summary>
        public double SquaredFirstEccentricity => _es;

        /// <summary>
        /// the second eccentricity
        /// </summary>
        public double _e => Math.Sqrt(_ses);

        /// <summary>
        /// square of the second eccentricity
        /// </summary>
        public double _ee => _ses;

        /// <summary>
        /// the second eccentricity
        /// </summary>
        public double SecondEccentricity => Math.Sqrt(_ses);

        /// <summary>
        /// square of the second eccentricity
        /// </summary>
        public double SquaredSecondEccentricity => _ses;

        /// <summary>
        /// radius of the rectifying sphere
        /// </summary>
        public double r
        {
            get
            {
                double n = 1 / (2 * _ivf - 1);
                return _a * (1 + n * n / 4) / (1 + n);
            }
        }
        #endregion

        #region properties
        /// <summary>
        /// The total surface area of the ellipsoid in square meters
        /// </summary>
        public double Area
        {
            get
            {
                if (_es == 0)
                {
                    return 4 * Math.Pow(_a, 2);
                }
                else
                {
                    double e = Math.Sqrt(_es);
                    return 2 * Math.PI * _a * _a * (1 + (1 - _es) / e * Math.Log((1 + e) / (1 - e)) / Math.Log(Math.E) / 2);
                }
            }
        }

        /// <summary>
        /// The total surface area of the ellipsoid in square kilometers
        /// </summary>
        public double Area_km => Area * 1E-6;

        /// <summary>
        /// Total volume of the ellipsoid, in cubic meters
        /// </summary>
        public double Volume
        {
            get
            {
                double b = _a * Math.Sqrt(1 - _es);
                return 4 * Math.PI * _a * _a * b / 3;
            }
        }

        /// <summary>
        /// Total volume of the ellipsoid, in cubic kilometers
        /// </summary>
        public double Volume_km
        {
            get
            {
                double b = _a * Math.Sqrt(1 - _es);
                return 4 * Math.PI * _a * _a * b / 3 * 1E-9;
            }
        }

        /// <summary>
        /// distance from the equator to the pole
        /// </summary>
        public double QuarterMeridian
        {
            get
            {
                double e4 = _es * _es;
                double e6 = _es * e4;
                double e8 = e4 * e4;
                double e10 = e4 * e6;
                double rB = Math.PI / 2;

                //子午线弧长公式的系数
                double cA, cB, cC, cD, cE, cF;
                cA = 1 + 3 * _es / 4 + 45 * e4 / 64 + 175 * e6 / 256 + 11025 * e8 / 16384 + 43659 * e10 / 65536;
                cB = 3 * _es / 4 + 15 * e4 / 16 + 525 * e6 / 512 + 2205 * e8 / 2048 + 72765 * e10 / 65536;
                cC = 15 * e4 / 64 + 105 * e6 / 256 + 2205 * e8 / 4096 + 10395 * e10 / 16384;
                cD = 35 * e6 / 512 + 315 * e8 / 2048 + 31185 * e10 / 131072;
                cE = 315 * e8 / 131072 + 3465 * e10 / 65536;
                cF = 693 * e10 / 131072;

                //子午线弧长
                return _a * (1 - _es) * (cA * rB - cB * Math.Sin(2 * rB) / 2 + cC * Math.Sin(4 * rB) / 4
                                        - cD * Math.Sin(6 * rB) / 6 + cE * Math.Sin(8 * rB) / 8 - cF * Math.Sin(10 * rB) / 10);
            }
        }

        /// <summary>
        /// mean radius
        /// </summary>
        public double MeanRadius
        {
            get
            {
                double b = _a * Math.Sqrt(1 - _es);
                return (2 * _a + b) / 3;
            }
        }

        /// <summary>
        /// authalic radius is the radius of a hypothetical perfect sphere that has the same surface area as spheroid.
        /// </summary>
        public double AuthalicRadius
        {
            get
            {
                // when the a=b.
                if (_es == 0) return _a;

                double b = _a * Math.Sqrt(1 - _es);
                double e = Math.Sqrt(_es);
                double tanh = Math.Log((1 + e) / (1 - e)) / Math.Log(Math.E) * 0.5;
                return Math.Sqrt(_a * _a / 2 + b * b / 2 * tanh / e);
            }
        }

        /// <summary>
        /// volumetric radius is the radius of a sphere of volume equal to the spheroid.
        /// </summary>
        public double VolumetricRadius
        {
            get
            {
                double b = _a * Math.Sqrt(1 - _es);
                return Math.Pow(_a * _a * b, 1 / 3);
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// the first auxiliary function
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <returns></returns>
        public double GetW(Latitude lat)
        {
            double sinB = Math.Sin(lat.Radians);
            return Math.Sqrt(1 - _es * sinB * sinB);
        }

        /// <summary>
        /// the second auxiliary function
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <returns></returns>
        public double GetV(Latitude lat)
        {
            double cosB = Math.Cos(lat.Radians);
            return Math.Sqrt(1 + _ses * cosB * cosB);
        }
        #endregion
    }
}

using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth
{
    /// <summary>
    /// Normal ellipsoid
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class NormalEllipsoid : Spheroid
    {
        #region basic variables
        /// <summary>
        /// angular velocity
        /// </summary>
        private double _ω;
        /// <summary>
        /// gravitational static readonly ant
        /// </summary>
        private double _GM;

        /// <summary>
        /// the second dynamic form factor
        /// </summary>
        private readonly double _J2;
        /// <summary>
        /// the second degree zonal gravitational coefficient
        /// </summary>
        private readonly double _C20;
        #endregion

        #region constructors
        /// <summary>
        /// Create a normal ellipsoid by an ellipsoid
        /// </summary>
        /// <param name="ellipsoid"></param>
        public NormalEllipsoid(Ellipsoid ellipsoid)
            : this(ellipsoid.a, ellipsoid.ivf, ellipsoid.ω, ellipsoid.GM, ellipsoid.Name, ellipsoid.Alias)
        { }

        /// <summary>
        /// Create a normal ellipsoid
        /// </summary>
        /// <param name="a">semi-major axis</param>
        /// <param name="ivf">flattening</param>
        /// <param name="ω">angular velocity</param>
        /// <param name="GM">gravitational static readonly ant</param>
        /// <param name="name">name of the normal ellipsoid</param>
        /// <param name="alias">abbreviated alias of ellipsoid</param>
        public NormalEllipsoid(double a, double ivf, double ω, double GM, string name = "", string alias = "")
            : base(a, a * (1 - 1 / ivf))
        {
            _ω = ω;
            _GM = GM;

            Identifier = new Identifier(typeof(NormalEllipsoid));
            Name = name;
            Alias = alias;

            //更新父类参数，降低重复计算的舍入误差
            _ivf = ivf;
            _es = 2 / _ivf - 1 / _ivf / _ivf;
            _ses = _es / (1 - _es);

            //由扁率计算J2、C20
            double _e = Math.Sqrt(_ses);
            double q0 = (1 + 3 / _ses) * Math.Atan(_e) - 3 / _e;
            double m = _ω * _ω * _a * _a * _a / _GM;

            _J2 = _es / 3 * (1 - 4 * m * e / 15 / q0);
            _C20 = -_J2 / Math.Sqrt(5.0);
        }

        /// <summary>
        /// Create a normal ellipsoid
        /// </summary>
        /// <param name="a">semi-major axis</param>
        /// <param name="J2">second dynamic form factor</param>
        /// <param name="ω">angular velocity</param>
        /// <param name="GM">gravitational static readonly ant</param>
        /// <param name="name">name of the normal ellipsoid</param>
        /// <param name="alias">abbreviated alias of ellipsoid</param>
        public NormalEllipsoid(double a, double J2, Angle ω, double GM, string name = "", string alias = "")
            : base(a, a * 297 / 298)
        {
            _GM = GM;
            _ω = ω.Radians;

            Identifier = new Identifier(typeof(NormalEllipsoid));
            Name = name;
            Alias = alias;
            
            //由J2计算扁率等参数，并更新父类参数
            double _J2 = J2;
            double _C20 = -J2 / Math.Sqrt(5.0);
            J2ToIvf(J2);
        }

        /// <summary>
        /// Create a normal ellipsoid
        /// </summary>
        /// <param name="a">semi-major axis</param>
        /// <param name="C20">second degree zonal gravitational coefficient</param>
        /// <param name="GM">gravitational static readonly ant</param>
        /// <param name="ω">angular velocity</param>
        /// <param name="name">name of the normal ellipsoid</param>
        /// <param name="alias">abbreviated alias of ellipsoid</param>
        public NormalEllipsoid(double a, double C20, double GM, Angle ω, string name = "", string alias = "")
            : base(a, a * 297 / 298)
        {
            _GM = GM;
            _ω = ω.Radians;

            Identifier = new Identifier(typeof(NormalEllipsoid));
            Name = name;
            Alias = alias;

            //由J2计算扁率等参数，并更新父类参数
            _C20 = C20;
            _J2 = -C20 * Math.Sqrt(5.0);
            J2ToIvf(_J2);
        }

        /// <summary>
        /// Compute the ivf, e2, e'2 by J2
        /// </summary>
        /// <param name="J2">J2 value</param>
        private void J2ToIvf(double J2)
        {
            double m = 4 * Math.Pow(_ω * _a, 2) * _a / _GM / 15;

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
        /// identifier if the ellipsoid
        /// </summary>
        [JsonProperty(Order = 6, NullValueHandling = NullValueHandling.Ignore)]
        public Identifier Identifier { get; set; }

        /// <summary>
        /// 旋转角速度，单位是 radians/sec
        /// </summary>
        [JsonProperty(Order = 2)]
        public double ω
        {
            get => _ω;
            set => _ω = value;
        }

        /// <summary>
        /// 旋转角速度，单位是 radians/sec
        /// </summary>
        public double AngularVelocity => _ω;

        /// <summary>
        /// 重力常数，即地球质量与万有引力常数之积，单位是 m^3/s^2
        /// </summary>
        [JsonProperty(Order = 3)]
        public double GM
        {
            get => _GM;
            set => _GM = value;
        }

        /// <summary>
        /// 重力常数，即地球总质量与万有引力常数之积，单位是 m^3/s^2
        /// </summary>
        public double GravitationalConst => _GM;

        /// <summary>
        /// 以千米为单位的地球质量与万有引力常数之积，单位是 km^3/s^2
        /// </summary>
        public double GM_km => _GM / 1.0e9;

        /// <summary>
        /// 动力学形状因子
        /// </summary>
        public double J2 => _J2;

        /// <summary>
        /// 二阶带谐系数
        /// </summary>
        public double C20 => _C20;

        /// <summary>
        /// assisted variable m
        /// </summary>
        public double m => 4 * Math.Pow(_ω * _a, 2) * _a / _GM / 15;
        #endregion

        /// <summary>
        /// normal gravity at equator
        /// </summary>
        public double ϒe
        {
            get
            {
                double b = _a * (1 - _ivf);
                double m = _ω * _ω * _a * _a * b / _GM;
                double mf = m * _ivf;
                return _GM / _a / b * (1 - 3 * m / 2 - 3 * mf / 7 - 125 * mf * _ivf / 294);
            }
        }

        /// <summary>
        /// get the normal gravity at earth surface
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <returns>gravity value</returns>
        public double GetSurfaceGravity(Latitude lat)
        {
            double b = _a * (1 - _ivf);
            double m = _ω * _ω * _a * _a * b / _GM;
            double mf = m * _ivf;
            double beta1 = -_ivf * _ivf / 8 + 5 * mf / 8;
            double beta = -_ivf + 5 * m / 2 - 17 * mf / 14 + 15 * m * m / 4;

            double ϒe = _GM / _a / b * (1 - 3 * m / 2 - 3 * mf / 7 - 125 * mf * _ivf / 294);

            double sinB = Math.Sin(lat.Radians);
            double sin2B = Math.Sin(lat.Radians * 2);
            return ϒe * (1 + beta * sinB * sinB - beta1 * sin2B * sin2B);
        }

        /// <summary>
        /// get the normal gravity at earth space point
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="hgt">ellipsoidal height</param>
        /// <returns>gravity value</returns>
        public double GetNormalGravity(Latitude lat, double hgt)
        {
            return GetSurfaceGravity(lat) - 0.3083 * hgt;
        }
    }
}

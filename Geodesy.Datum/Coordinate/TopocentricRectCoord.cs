using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Topocentric rectangular coordinate system
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class TopocentricRectCoord : CartesianCoord
    {
        /// <summary>
        /// 
        /// </summary>
        public TopocentricRectCoord()
            : base(double.NaN, double.NaN, double.NaN)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="northing"></param>
        /// <param name="easting"></param>
        /// <param name="upping"></param>
        public TopocentricRectCoord(double easting, double northing, double upping)
            : base(easting, northing, upping)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enu"></param>
        public TopocentricRectCoord(double[] enu)
        {
            if (enu.Length != 3)
            {
                throw new GeodeticException("The coordinate length is error.");
            }
            _coord = enu;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(Order = 0)]
        public double Easting
        {
            get => _coord[0];
            private set => _coord[0] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(Order = 1)]
        public double Northing
        {
            get => _coord[1];
            private set => _coord[1] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(Order = 2)]
        public double Upping
        {
            get => _coord[2];
            private set => _coord[2] = value;
        }

        /// <summary>
        /// easting
        /// </summary>
        public double E => _coord[0];

        /// <summary>
        /// northing
        /// </summary>
        public double N => _coord[1];

        /// <summary>
        /// upping
        /// </summary>
        public double U => _coord[2];

        /// <summary>
        /// Set coordinate value
        /// </summary>
        /// <param name="e">component X</param>
        /// <param name="n">component Y</param>
        /// <param name="u">component Z</param>
        public void SetCoordinate(double e, double n, double u)
        {
            _coord = new double[3];
            _coord[0] = e;
            _coord[1] = n;
            _coord[2] = u;
        }

        /// <summary>
        /// Set coordinate value
        /// </summary>
        /// <param name="e">component X</param>
        /// <param name="n">component Y</param>
        /// <param name="u">component Z</param>
        public void SetCoordinate(double e, double n, double u, LinearUnit unit)
        {
            _coord = new double[3];
            _coord[0] = e;
            _coord[1] = n;
            _coord[2] = u;
            Unit = unit;
        }

        /// <summary>
        /// Rotate the coordinate
        /// </summary>
        /// <param name="theta">rotated angle</param>
        /// <param name="fixedAxis">1 - X axis, 2 - Y axis, 3 - Z axis</param>
        /// <returns></returns>
        public new TopocentricRectCoord Rotate(Angle theta, int fixedAxis)
        {
            return new TopocentricRectCoord(Rotate3D(theta, fixedAxis).GetCoordinate());
        }

        /// <summary>
        /// translate/shift the coordinate origin
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public TopocentricRectCoord Shift(TopocentricRectCoord delta)
        {
            double[] coord = new double[3];
            coord[0] = _coord[0] + delta.E;
            coord[1] = _coord[1] + delta.N;
            coord[2] = _coord[2] + delta.U;
            return new TopocentricRectCoord(coord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!double.IsNaN(Easting))
            {
                return "E:" + Northing.ToString("# ###.###") + ", N:" + Easting.ToString("# ###.###") + ", U:" + Upping.ToString("# ###.###");
            }
            else
            {
                return base.ToString();
            }
        }
    }
}
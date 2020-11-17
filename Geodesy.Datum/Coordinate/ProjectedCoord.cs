using System;
using Newtonsoft.Json;
using Geodesy.Datum.Units;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// Projection coordinate is 2D coordinate on planar surface.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ProjectedCoord : CartesianCoord
    {
        /// <summary>
        /// Create a null projected coordinate object.
        /// </summary>
        public ProjectedCoord()
            : this(double.NaN, double.NaN)
        { }

        /// <summary>
        /// Create a projected coordinate object.
        /// </summary>
        /// <param name="northing">northing in north-south direction</param>
        /// <param name="easting">easting in east-west direction</param>
        public ProjectedCoord(double northing, double easting)
        {
            _coord = new double[2];
            _coord[0] = northing;
            _coord[1] = easting;
        }

        /// <summary>
        /// Create a projected coordinate object.
        /// </summary>
        /// <param name="xy"></param>
        public ProjectedCoord(double[] xy)
        {
            if (xy.Length !=2)
            {
                throw new GeodeticException("The coordinate length is error.");
            }

            _coord = xy;
        }

        /// <summary>
        /// Get the x component in north-south direction.
        /// </summary>
        public double x => _coord[0];
        
        /// <summary>
        /// Get the northing component in north-south direction.
        /// </summary>
        [JsonProperty(Order = 0)]
        public double Northing
        {
            get => _coord[0];
            private set => _coord[0] = value;
        }

        /// <summary>
        /// Get the y component in east-west direction.
        /// </summary>
        public double y => _coord[1];

        /// <summary>
        /// Get the easting component in east-west direction.
        /// </summary>
        [JsonProperty(Order = 1)]
        public double Easting
        {
            get => _coord[1];
            private set => _coord[1] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="northing"></param>
        /// <param name="easting"></param>
        public void SetCoordinate(double northing, double easting)
        {
            _coord[0] = northing;
            _coord[1] = easting;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="northing"></param>
        /// <param name="easting"></param>
        /// <param name="unit"></param>
        public void SetCoordinate(double northing, double easting, LinearUnit unit)
        {
            _coord[0] = northing;
            _coord[1] = easting;
            Unit = unit;
        }

        /// <summary>
        /// Rotate the coordinate
        /// </summary>
        /// <param name="theta">rotated angle</param>
        /// <returns></returns>
        public ProjectedCoord Rotate(Angle theta)
        {
            return new ProjectedCoord(Rotate2D(theta).GetCoordinate());
        }

        /// <summary>
        /// translate/shift the coordinate origin
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public ProjectedCoord Shift(ProjectedCoord delta)
        {
            double[] coord = new double[2];
            coord[0] = _coord[0] + delta.x;
            coord[1] = _coord[1] + delta.y;
            return new ProjectedCoord(coord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = string.Empty;

            if (!double.IsNaN(Northing))
            {
                str = "x=" + Northing.ToString("### ###.###");
                str += ", y=" + Easting.ToString("### ###.###");
            }

            return str;
        }
    }
}

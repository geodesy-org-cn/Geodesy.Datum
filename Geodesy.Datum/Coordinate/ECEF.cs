using System;
using Newtonsoft.Json;

namespace Geodesy.Datum.Coordinate
{
    /// <summary>
    /// ECEF (acronym for earth-centered, earth-fixed), also known as ECR (initialism for 
    /// earth-centered rotational), is a geographic and Cartesian coordinate system and is 
    /// sometimes known as a "conventional terrestrial" system.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ECEF : SpaceRectangularCoord
    {
        /// <summary>
        /// Create a null ECEF coordinate object.
        /// </summary>
        public ECEF()
        { }

        /// <summary>
        /// Create a ECEF coordinate object.
        /// </summary>
        /// <param name="X">X component</param>
        /// <param name="Y">Y component</param>
        /// <param name="Z">Z component</param>
        public ECEF(double X, double Y, double Z)
            : base(X, Y, Z)
        { }

        /// <summary>
        /// Create a ECEF coordinate object.
        /// </summary>
        /// <param name="xyz">coordinate components array</param>
        public ECEF(double[] xyz)
            : base(xyz)
        { }

        /// <summary>
        /// Rotate the coordinate
        /// </summary>
        /// <param name="theta">rotated angle</param>
        /// <param name="fixedAxis">1 - X axis, 2 - Y axis, 3 - Z axis</param>
        /// <returns></returns>
        public new ECEF Rotate(Angle theta, int fixedAxis)
        {
            return new ECEF(Rotate3D(theta, fixedAxis).GetCoordinate());
        }

        /// <summary>
        /// translate/shift the coordinate origin
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public ECEF Shift(ECEF delta)
        {
            double[] coord = new double[3];
            coord[0] = _coord[0] + delta.X;
            coord[1] = _coord[1] + delta.Y;
            coord[2] = _coord[2] + delta.Z;
            return new ECEF(coord);
        }
    }
}

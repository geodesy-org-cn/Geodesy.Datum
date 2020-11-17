using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Transformation
{
    /// <summary>
    /// Molodensky-Badekas (7+3) Transformations
    /// </summary>
    public static class MolodenskyBadekas
    {
        /// <summary>
        /// Molodensky-Badekas transformations
        /// </summary>
        /// <param name="point">space rectangular point</param>
        /// <param name="para">(7+3) parameters</param>
        /// <returns>space point in new datum</returns>
        public static SpaceRectangularCoord Transform(SpaceRectangularCoord point, TransParameters para)
        {
            // the last three is special
            double dX = point.X - para.GetValue("Px");
            double dY = point.Y - para.GetValue("Py");
            double dZ = point.Z - para.GetValue("Pz");

            double X, Y, Z;

            // formula is from https://en.wikipedia.org/wiki/Geographic_coordinate_conversion 
            X = point.X + para.Tx + (dX - para.Rz * dY + para.Ry * dZ) + para.S * 1E-6 * dX;
            Y = point.Y + para.Ty + (para.Rz * dX + dY - para.Rx * dZ) + para.S * 1E-6 * dY;
            Z = point.Z + para.Tz + (-para.Ry * dX + para.Rx * dY + dZ) + para.S * 1E-6 * dZ;

            return new SpaceRectangularCoord(X, Y, Z);
        }
    }
}

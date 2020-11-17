using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// key of projection parameter
    /// </summary>
    public enum ProjectionParameter
    {
        /// <summary>
        /// semi-major of ellipsoid.
        /// </summary>
        Semi_Major,

        /// <summary>
        /// inverse flattening of ellipsoid. 
        /// <para>If you approximate the Earth as a sphere, set InverseFlattening to a nagetive number.</para>
        /// </summary>
        Inverse_Flattening,

        /// <summary>
        /// false easting
        /// </summary>
        False_Easting,

        /// <summary>
        /// false northing
        /// </summary>
        False_Northing,

        /// <summary>
        /// central meridian, also called longitude of center.
        /// </summary>
        Central_Meridian,

        /// <summary>
        /// latitude of origin, also called latitude of center.
        /// </summary>
        Latitude_Of_Origin,

        /// <summary>
        /// the first standard parallel of secant conformal conic projections.
        /// </summary>
        Standard_Parallel_1,

        /// <summary>
        /// the second standard parallel of secant conformal conic projections.
        /// </summary>
        Standard_Parallel_2,

        /// <summary>
        /// scale factor
        /// </summary>
        Scale_Factor,

        /// <summary>
        /// latitude of true scale
        /// </summary>
        True_Scale_Latitude,

        /// <summary>
        /// azimuth of the initial line of oblique projections
        /// </summary>
        Azimuth,

        /// <summary>
        /// angle from the rectified grid to the skew (oblique) grid of oblique projections
        /// </summary>
        Rectified_Grid_Angle,

        /// <summary>
        /// width of projection zone in degree unit, usually is 6.0 or 3.0 in Gauss-Kruger projection.
        /// </summary>
        Width_Of_Zone,
    }
}
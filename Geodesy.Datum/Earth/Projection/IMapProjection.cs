using Geodesy.Datum.Coordinate;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// Interface of map proection.
    /// </summary>
    public interface IMapProjection
    {
        /// <summary>
        /// Identifier of projection.
        /// </summary>
        Identifier Identifier { get; set; }

        /// <summary>
        /// Direct projection from ellispoid to projected plane.
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="northing"></param>
        /// <param name="easting"></param>
        void Forward(Latitude lat, Longitude lng, out double northing, out double easting);

        /// <summary>
        /// Direct projection from ellispoid to projected plane.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <returns></returns>
        (double northing, double easting) Forward(Latitude lat, Longitude lng);

        /// <summary>
        /// Direct projection from ellispoid to projected plane.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        ProjectedCoord Forward(GeographicCoord point);

        /// <summary>
        /// Direct projection from ellispoid to projected plane.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        List<ProjectedCoord> Forward(List<GeodeticCoord> points);

        /// <summary>
        /// Direct projection from ellispoid to projected plane.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        ProjectedCoord Forward(GeodeticCoord point);

        /// <summary>
        /// Direct projection from ellispoid to projected plane.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        List<ProjectedCoord> Forward(List<GeographicCoord> points);

        /// <summary>
        /// Reverse projection from projected plane to ellispoid.
        /// </summary>
        /// <param name="northing"></param>
        /// <param name="easting"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        void Reverse(double northing, double easting, out Latitude lat, out Longitude lng);

        /// <summary>
        /// Reverse projection from projected plane to ellispoid.
        /// </summary>
        /// <param name="northing"></param>
        /// <param name="easting"></param>
        /// <returns>latitude and longitude in degree unit</returns>
        (Latitude lat, Longitude lng) Reverse(double northing, double easting);

        /// <summary>
        /// Reverse projection from projected plane to ellispoid.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        GeographicCoord Reverse(ProjectedCoord point);

        /// <summary>
        /// Reverse projection from projected plane to ellispoid.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        List<GeographicCoord> Reverse(List<ProjectedCoord> points);
    }
}

namespace Geodesy.Datum.Coordinate
{
    public interface IGeographicCoord
    {
        /// <summary>
        /// 
        /// </summary>
        Latitude Latitude { get; }

        /// <summary>
        /// 
        /// </summary>
        Longitude Longitude { get; }

        /// <summary>
        /// 
        /// </summary>
        Units.AngularUnit AngularUnit { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        void SetCoordinate(Latitude lat, Longitude lng);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="unit"></param>
        void SetCoordinate(double lat, double lng, Units.AngularUnit unit);
    }
}

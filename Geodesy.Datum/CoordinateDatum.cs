using Newtonsoft.Json;
using Geodesy.Datum.CRS;
using Geodesy.Datum.Earth;
using System.Collections.Generic;
using Geodesy.Datum.Earth.Projection;

namespace Geodesy.Datum
{
    /// <summary>
    /// Geodetic datum, reference system
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class CoordinateDatum
    {
        /// <summary>
        /// Identifier of the coordinate datum
        /// </summary>
        public Identifier Identifier { get; set; }

        /// <summary>
        /// name of geodetic datum
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// abbreviated name
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// the datum used area
        /// </summary>
        public string AreaOfUse { get; set; }

        /// <summary>
        /// scope of application
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// earth ellipsoid
        /// </summary>
        public Ellipsoid Ellipsoid { get; set; }

        /// <summary>
        /// prime meridian, default is Greenwich
        /// </summary>
        public PrimeMeridian PrimeMeridian { get; set; } = PrimeMeridian.Greenwich;

        /// <summary>
        /// Coordinate system
        /// </summary>
        public Cartesian3DSystem CoordinateSystem { get; set; }

        /// <summary>
        /// map projection instant from ellipsoid surface to projected plane. 
        /// </summary>
        public MapProjection Projection { get; set; }

        /// <summary>
        /// parameters of map projection.
        /// </summary>
        public Dictionary<ProjectionParameter, double> ProjectionParameters { get; set; }

        /// <summary>
        /// some additional information
        /// </summary>
        public string Remarks { get; set; }
    }
}

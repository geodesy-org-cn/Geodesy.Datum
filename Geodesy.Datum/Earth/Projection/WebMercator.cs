using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// Web Mercator, Google Web Mercator, Spherical Mercator, WGS 84 Web Mercator or WGS 84/
    /// Pseudo-Mercator is a variant of the Mercator projection and is the de facto standard for 
    /// Web mapping applications. It rose to prominence when Google Maps adopted it in 2005. 
    /// It is used by virtually all major online map providers, including Google Maps, Mapbox, 
    /// Bing Maps, OpenStreetMap, Mapquest, Esri, and many others.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class WebMercator : Mercator
    {
        /// <summary>
        /// 
        /// </summary>
        private WebMercator()
            : this(new Dictionary<ProjectionParameter, double>
            {
                { ProjectionParameter.Semi_Major, Settings.Ellipsoid.a },
                { ProjectionParameter.Inverse_Flattening, -1 },
                { ProjectionParameter.Scale_Factor, 1.0 },
            })
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public WebMercator(Dictionary<ProjectionParameter, double> parameters)
            :base(parameters)
        {
            Identifier = new Identifier(typeof(WebMercator));
            Surface = ProjectionSurface.Cylindrical;

            if (CenteralMaridian==null)
            {
                SetParameter(ProjectionParameter.Central_Meridian, 0.0);
            }
            if (double.IsNaN(FalseEasting))
            {
                SetParameter(ProjectionParameter.False_Easting, 0.0);
            }
            if (double.IsNaN(FalseNorthing))
            {
                SetParameter(ProjectionParameter.False_Northing, 0.0);
            }
        }
    }
}

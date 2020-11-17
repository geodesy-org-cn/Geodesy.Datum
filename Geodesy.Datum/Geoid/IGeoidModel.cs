using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Geoid
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The geoid code is rewrite from rtklib 2.4.3 geoid.c
    ///  
    ///      GEOID_EMBEDDED   : embedded model(1x1deg)
    ///      GEOID_EGM96_M150 : EGM96 15x15"
    ///      GEOID_EGM2008_M25: EGM2008 2.5x2.5"
    ///      GEOID_EGM2008_M10: EGM2008 1.0x1.0"
    ///      GEOID_GSI2000_M15: GSI geoid 2000 1.0x1.5"
    /// 
    ///  the following geoid models can be used
    ///      WW15MGH.DAC                                     : EGM96 15x15" binary grid height
    ///      Und_min2.5x2.5_egm2008_isw= 82_WGS84_TideFree_SE: EGM2008 2.5x2.5"
    ///      Und_min1x1_egm2008_isw= 82_WGS84_TideFree_SE    : EGM2008 1.0x1.0"
    ///      gsigeome_ver4                                   : GSI geoid 2000 1.0x1.5" (japanese area)
    /// 
    ///  (byte-order of binary files must be compatible to cpu)
    /// </remarks>
    public interface IGeoidModel
    {
        /// <summary>
        /// 
        /// </summary>
        public ModelType Model { get; }

        /// <summary>
        /// 
        /// </summary>
        public double GridLng { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double GridLat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Columns { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Rows { get; }

        /// <summary>
        /// 
        /// </summary>
        public double Left { get; }

        /// <summary>
        /// 
        /// </summary>
        public double Right { get; }

        /// <summary>
        /// 
        /// </summary>
        public double Top { get; }

        /// <summary>
        /// 
        /// </summary>
        public double Bottom { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public double GetHeight(Latitude lat, Longitude lng);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public double GetHeight(double lat, double lng);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public int[] GetBoundary(ref double lat, ref double lng);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public double[] ReadGrid(int[] position);
    }
}

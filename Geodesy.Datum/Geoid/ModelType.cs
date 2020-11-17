using System;
using System.Collections.Generic;
using System.Text;

namespace Geodesy.Datum.Geoid
{
    public enum ModelType
    {
        /// <summary>
        /// Embedded model (1x1degree)
        /// </summary>
        Geoid_Embedded,

        /// <summary>
        /// EGM96 15*15'
        /// </summary>
        Geoid_EGM96_M150,

        /// <summary>
        /// EGM2008 2.5*2.5'
        /// </summary>
        Geoid_EGM2008_M25,

        /// <summary>
        /// EGM2008 1.0*1.0'
        /// </summary>
        Geoid_EGM2008_M10,

        /// <summary>
        /// GSI geoid 2000 1.0*1.5'
        /// </summary>
        Geoid_GSI2000_M15,

        /// <summary>
        /// unknown
        /// </summary>
        Unknown
    }
}

using System;
using Geodesy.Datum.Units;

namespace Geodesy.Datum
{
    /// <summary>
    /// 
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// The defalut ellipsoid.
        /// </summary>
        public static Earth.Ellipsoid Ellipsoid { get; set; } = new Earth.Ellipsoid(6378137, 298.257222101);

        #region constants of epsilon
        /// <summary>
        /// Limit of 0.000001 second precise iteration 
        /// </summary>
        public const double Epsilon6 = 1E-7 * Math.PI / 180 / 3600;

        /// <summary>
        /// Limit of 0.00001 second precise iteration 
        /// </summary>
        public const double Epsilon5 = 1E-6 * Math.PI / 180 / 3600;

        /// <summary>
        /// Limit of 0.0001 second precise iteration
        /// </summary>
        public const double Epsilon4 = 1E-5 * Math.PI / 180 / 3600;

        /// <summary>
        /// Limit of 0.001 second precise iteration
        /// </summary>
        public const double Epsilon3 = 1E-4 * Math.PI / 180 / 3600;

        /// <summary>
        /// Limit of 0.01 second precise iteration
        /// </summary>
        public const double Epsilon2 = 1E-3 * Math.PI / 180 / 3600;

        /// <summary>
        /// Limit of 0.1 second precise iteration
        /// </summary>
        public const double Epsilon1 = 1E-2 * Math.PI / 180 / 3600;

        /// <summary>
        /// Limit of 1 second precise iteration
        /// </summary>
        public const double Epsilon0 = 1E-1 * Math.PI / 180 / 3600;
        #endregion

        #region base unit
        /// <summary>
        /// The default basic linear unit.
        /// </summary>
        public static LinearUnit BaseLinearUnit { get; set; } = LinearUnit.Meter;

        /// <summary>
        /// The default basic angular unit.
        /// </summary>
        public static AngularUnit BaseAngularUnit { get; set; } = AngularUnit.Degree;

        /// <summary>
        /// The default basic time unit.
        /// </summary>
        public static TimeUnit BaseTimeUnit { get; set; } = TimeUnit.Second;

        /// <summary>
        /// The default basic mass unit.
        /// </summary>
        public static MassUnit BaseMassUnit { get; set; } = MassUnit.Kilogram;

        /// <summary>
        /// The default basic velocity unit.
        /// </summary>
        public static VelocityUnit BaseVelocityUnit { get; set; } = VelocityUnit.MeterPerSecond;
        #endregion
    }
}

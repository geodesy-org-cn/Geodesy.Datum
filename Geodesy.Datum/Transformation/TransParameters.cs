using System;
using Geodesy.Datum.Frame;
using System.Collections.Generic;

namespace Geodesy.Datum.Transformation
{
    /// <summary>
    /// parameters of different datum transformation
    /// </summary>
    /// <remarks>
    /// 在坐标旋转中，外文文献大多以逆时针为正，而中文则以顺时针为正，所以旋转矩阵的符号存在差异。本软件遵循逆时针为正的原则，
    /// 所以在设置转换参数时，需要注意旋转角的符号！！！
    /// During the coordinate rotation, most foreign papers usually adopts counterclockwise as positive, 
    /// while Chinese adopts clockwise as posituve. so the signs of the rotation matrix are different. 
    /// This software follows the principle of counterclockwise as positive. When setting the conversion 
    /// parameters, pay attention to the sign of the rotation angle.
    /// </remarks>
    [Serializable]
    public sealed partial class TransParameters
    {
        private const double Second_To_Radian = Math.PI / 180 / 3600;

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<string, double> _values;

        /// <summary>
        /// create the transformation parameters
        /// </summary>
        /// <param name="values">The parameters appears in order by Tx, Ty, Tz, S, Rx, Ry, Rz, Px, Py, Pz. Usually, translation parameter unit is meter, sacle factor is ppm, rotation parameter unit is second.</param>
        public TransParameters(CoordinateDatum from, CoordinateDatum to, params double[] values)
        {
            From = from;
            To = to;

            _values = new Dictionary<string, double>();
            #region Three pamaters
            if (values.Length > 0) _values.Add("Tx", values[0]);
            if (values.Length > 1) _values.Add("Ty", values[1]);
            if (values.Length > 2) _values.Add("Tz", values[2]);
            #endregion

            #region Four parameters
            if (values.Length > 3) _values.Add("S", values[3]);
            #endregion

            #region Seven parameters
            if (values.Length > 4) _values.Add("Rx", values[4]);
            if (values.Length > 5) _values.Add("Ry", values[5]);
            if (values.Length > 6) _values.Add("Rz", values[6]);
            #endregion

            #region Ten parameters (Molodensky-Badekas transform)
            if (values.Length > 7) _values.Add("Px", values[7]);
            if (values.Length > 8) _values.Add("Py", values[8]);
            if (values.Length > 9) _values.Add("Pz", values[9]);
            #endregion
        }

        /// <summary>
        /// create the transformation parameters
        /// </summary>
        /// <param name="values">parameters. Usually, translation parameter unit is meter, rotation parameter unit is second, sacle factor is ppm.</param>
        /// <param name="from">source datum</param>
        /// <param name="to">target datum</param>
        /// <param name="code">code of transformation</param>
        /// <param name="location">region of use</param>
        public TransParameters(CoordinateDatum from, CoordinateDatum to, string code = "", string location = "", params double[] values)
            : this(from, to, values)
        {
            Code = code;
            Location = location;
        }

        #region Properties
        /// <summary>
        /// Identifier of transformation
        /// </summary>
        public Identifier Identifier { get; set; }

        /// <summary>
        /// source datum
        /// </summary>
        public CoordinateDatum From { get; set; }

        /// <summary>
        /// target datum
        /// </summary>
        public CoordinateDatum To { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// translation along the X-axis, meter
        /// </summary>
        public double Tx => GetValue("Tx");

        /// <summary>
        /// translation along the Y-axis, meter
        /// </summary>
        public double Ty => GetValue("Ty");

        /// <summary>
        /// translation along the Z-axis, meter
        /// </summary>
        public double Tz => GetValue("Tz");

        /// <summary>
        /// scale factor, 1E+6
        /// </summary>
        public double S => GetValue("S");

        /// <summary>
        /// rotation about the X-axis, radian
        /// </summary>
        public double Rx => GetValue("Rx") * Second_To_Radian;

        /// <summary>
        /// rotation about the Y-axis, radian
        /// </summary>
        public double Ry => GetValue("Ry") * Second_To_Radian;

        /// <summary>
        /// rotation about the Z-axis, radian
        /// </summary>
        public double Rz => GetValue("Rz") * Second_To_Radian;

        /// <summary>
        /// count of parameters
        /// </summary>
        public int Count => _values.Count;
        #endregion

        /// <summary>
        /// Set the value of transform parameter
        /// </summary>
        /// <param name="key">name of parameter</param>
        /// <param name="value">parameter value</param>
        public void SetValue(string key, double value)
        {
            if (_values.ContainsKey(key))
            {
                _values[key] = value;
            }
            else
            {
                _values.Add(key, value);
            }
        }

        /// <summary>
        /// Get the transform parameter value
        /// </summary>
        /// <param name="key"></param>
        /// <returns>parameter value</returns>
        public double GetValue(string key)
        {
            return _values.ContainsKey(key) ? _values[key] : 0;
        }

        /// <summary>
        /// invert the transformation parameters
        /// </summary>
        public void Invert()
        {
            foreach (string key in _values.Keys)
            {
                _values[key] *= -1;
            }

            CoordinateDatum datum = To;
            To = From;
            From = datum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = string.Empty;

            if (_values.Count >= 3)
                str += "Tx=" + _values["Tx"].ToString("0.000") +
                       ", Ty=" + _values["Ty"].ToString("0.000") +
                       ", Tz=" + _values["Tz"].ToString("0.000");
            if (_values.Count > 4)
                str += ", m=" + (_values["S"]).ToString("0.000") +
                       ", Rx=" + _values["Rx"].ToString("0.0000") +
                       ", Ry=" + _values["Ry"].ToString("0.0000") +
                       ", Rz=" + _values["Rz"].ToString("0.0000");

            return str;
        }

        /// <summary>
        /// The transformation parameters from WGS84(G1674) to WGS84 (G1762)
        /// </summary>
        public static readonly TransParameters WGS84G1674_WGS84G1762 = new TransParameters(
                    America.WGS84_G1674, America.WGS84_G1762, -0.004, +0.003, +0.004, 0, -6.9, -0.27, +0.27);

        /// <summary>
        /// The transformation parameters from WGS84(G1150) to WGS84 (G1762)
        /// </summary>
        public static readonly TransParameters WGS84G1150_WGS84G1762 = new TransParameters(
            America.WGS84_G1150, America.WGS84_G1762, -0.006, +0.005, 0, +0.020, -4.5, 0, 0);

        /// <summary>
        /// The transformation parameters from WGS84(G1150) to WGS84 (G1674)
        /// </summary>
        public static readonly TransParameters WGS84G1150_WGS84G1674 = new TransParameters(
            America.WGS84_G1150, America.WGS84_G1674, -0.0047, +0.0119, +0.0156, 0, +4.72, +0.52, +0.01);

        #region Tansformation parameters via http://www.geocachingtoolbox.com/?page=datumEllipsoidDetails
        /// <summary>
        /// The transformation parameters from PZ90(GLONASS) to WGS84 (Russia)
        /// </summary>
        public static readonly TransParameters PZ90_WGS84 = new TransParameters(
            Europe.PZ90, America.WGS84, -1.1, -0.3, -0.9, 0, 0, 0, 0.169);

        /// <summary>
        /// The transformation parameters from NAD83 to WGS84 (USA)
        /// </summary>
        public static readonly TransParameters NAD83_WGS84 = new TransParameters(
            America.NAD83, America.WGS84, 1.004, -1.910, -0.515, -0.0015, 0.0267, 0.0034, 0.011);

        /// <summary>
        /// The transformation parameters from WGS72 to WGS84 (USA)
        /// </summary>
        public static readonly TransParameters WGS72_WGS84 = new TransParameters(America.WGS72, America.WGS84, 0, 0, 4.5, 0.219, 0, 0, 0.554);

        /// <summary>
        /// The transformation parameters from Ordnance Survey Great Britain 1936 to WGS84 (England, Scotland, Wales)
        /// </summary>
        public static readonly TransParameters OSGB36_WGS84 = new TransParameters(null, America.WGS84, -446.448, 125.157, -542.060, 20.4894, -0.1502, -0.2470, -0.8421);

        /// <summary>
        /// The transformation parameters from European Datum 1950 to WGS84 (European)
        /// </summary>
        public static readonly TransParameters ED50_WGS84 = new TransParameters(Europe.ED50, America.WGS84, 89.5, 93.8, 123.1, -1.2, 0, 0, 0.156);

        /// <summary>
        /// The transformation parameters from Amersfoort to WGS84 (Netherlands)
        /// </summary>
        public static readonly TransParameters Netherlands_WGS84 = new TransParameters(null, America.WGS84, 565, 49.9, 465.8, 4.08, -0.409, 0.36, -1.869);

        /// <summary>
        /// The transformation parameters from ARC 1950 to WGS84 (Zimbabwe)
        /// </summary>
        public static readonly TransParameters Zimbabwe_WGS84 = new TransParameters(null, America.WGS84, -111.16, -186.64, -301.34, 1.776, -4.023, -2.838, 4.899);

        /// <summary>
        /// The transformation parameters from ARC 1950 to WGS84 (Kenya)
        /// </summary>
        public static readonly TransParameters Kenya_WGS84 = new TransParameters(null, America.WGS84, -62.44, -209.95, 17.83, 7.382, -6.7857, 7.66, 7.2059);

        /// <summary>
        /// The transformation parameters from Australian Geodetic 1984 to WGS84 (Australia)
        /// </summary>
        public static readonly TransParameters Australia_WGS84 = new TransParameters(null, America.WGS84, -116, -50.47, 141.69, 0.0983, 0.23, 0.39, 0.344);

        /// <summary>
        /// The transformation parameters from Bekaa Base South End to WGS84 (Lebanon)
        /// </summary>
        public static readonly TransParameters Lebanon_WGS84 = new TransParameters(null, America.WGS84, -465.05, 440.83, 41.4, 15.289, -3.88771, -11.3482, -27.4139);

        /// <summary>
        /// The transformation parameters from Berne 1898 to WGS84 (Switzerland)
        /// </summary>
        public static readonly TransParameters Switzerland_WGS84 = new TransParameters(null, America.WGS84, 660.077, 13.551, 369.344, 5.66, -0.805, -0.578, -0.952);

        /// <summary>
        /// The transformation parameters from S-JTSK to WGS84 (Slovakia)
        /// </summary>
        public static readonly TransParameters Slovakia_WGS84 = new TransParameters(null, America.WGS84, 559, 68.7, 451.5, 5.71, 7.92, 4.073, 4.251);

        /// <summary>
        /// The transformation parameters from Observatoroi to WGS84 (Mozambique South)
        /// </summary>
        public static readonly TransParameters MozambiqueSouth_WGS84 = new TransParameters(null, America.WGS84, -153, -227, -255, 16.99, -1.986, -0.033, 3.866);

        /// <summary>
        /// The transformation parameters from TETE to WGS84 (Mozambique)
        /// </summary>
        public static readonly TransParameters Mozambique_WGS84 = new TransParameters(null, America.WGS84, -107, -167, -211, 9.28, 0.871, 0.207, 0.992);

        /// <summary>
        /// The transformation parameters from Cyprus 1935 to WGS84 (Cyprus)
        /// </summary>
        public static readonly TransParameters Cyprus_WGS84 = new TransParameters(null, America.WGS84, -104.24, -16.713, 843.593, -60.095, 0.90497, 0.64131, 3.01174);

        /// <summary>
        /// The transformation parameters from European Datum 1950 to WGS84 (Oman)
        /// </summary>
        public static readonly TransParameters Oman_ED50_WGS84 = new TransParameters(null, America.WGS84, -137.34, -189.51, -2.6, -8.017, -4.5735, 2.6257, 0.6849);

        /// <summary>
        /// The transformation parameters from Fahud to WGS84 (Oman)
        /// </summary>
        public static readonly TransParameters Oman_Fahud_WGS84 = new TransParameters(null, America.WGS84, -173.69, -247.71, 162.08, 19.727, -1.141, -2.7308, 8.6343);

        /// <summary>
        /// The transformation parameters from Geodetic Datum 1949 to WGS84 (New Zealand)
        /// </summary>
        public static readonly TransParameters NewZealand_WGS84 = new TransParameters(null, America.WGS84, 59.47, -5.04, 187.44, -4.5993, 0.47, -0.1, 1.024);

        /// <summary>
        /// The transformation parameters from Helsinki. Kallio Church to WGS84 (Finland)
        /// </summary>
        public static readonly TransParameters Finland_WGS84 = new TransParameters(null, America.WGS84, -84.8, -208, -96.3, -0.023, 2.36, 1, 3.09);

        /// <summary>
        /// The transformation parameters from KKJ to WGS84 (Finland)
        /// </summary>
        public static readonly TransParameters Finland_KKJ_WGS84 = new TransParameters(null, America.WGS84, -90.7, -106.1, -119.2, 1.37, 4.09, 0.218, -1.05);

        /// <summary>
        /// The transformation parameters from Hermannskogel to WGS84 (Former Yugoslavia)
        /// </summary>
        public static readonly TransParameters FormerYugoslavia_WGS84 = new TransParameters(null, America.WGS84, 515.149, 186.233, 511.959, 0.782, 5.49721, 3.51742, -12.948);

        /// <summary>
        /// The transformation parameters from Hungarian Datum 1972 to WGS84 (Hungary)
        /// </summary>
        public static readonly TransParameters Hungary_WGS84 = new TransParameters(null, America.WGS84, -56.94, 67.91, 19.32, -1.09, 0.2, 0.32, 0.42);

        /// <summary>
        /// The transformation parameters from Indian to WGS84 (Bangladesh)
        /// </summary>
        public static readonly TransParameters Bangladesh_WGS84 = new TransParameters(null, America.WGS84, 79.2, 670.3, 230, 11.034, 0, 0, -7.274);

        /// <summary>
        /// The transformation parameters from Ireland 1965 to WGS84 (Ireland)
        /// </summary>
        public static readonly TransParameters Ireland_WGS84 = new TransParameters(null, America.WGS84, 482.53, -130.596, 564.557, 8.15, -1.042, -0.214, -0.631);

        /// <summary>
        /// The transformation parameters from Kandawala Jackson to WGS84 (Sri Lanka)
        /// </summary>
        public static readonly TransParameters SriLanka_WGS84 = new TransParameters(null, America.WGS84, 33.7, 886.1, 105.3, -20.187, -0.11, 0.369, 3.701);

        /// <summary>
        /// The transformation parameters from Kertau 1948 to WGS84 (Malaysia W & Sing.)
        /// </summary>
        public static readonly TransParameters Malaysia_WGS84 = new TransParameters(null, America.WGS84, -366.94, 719.29, -88.93, 9.093, 2.498, 2.142, -12.057);

        /// <summary>
        /// The transformation parameters from Timbalai 1968. Adj of 1948 to WGS84 (Malaysia E & Brunei)
        /// </summary>
        public static readonly TransParameters Malaysia1968_WGS84 = new TransParameters(null, America.WGS84, -528, 566.18, -75.24, 9.216, 1.137, 0.194, 3.034);

        /// <summary>
        /// The transformation parameters from Timbalai 1948 (Everest) to WGS84 (Malaysia E & Brunei)
        /// </summary>
        public static readonly TransParameters Malaysia_Everest_WGS84 = new TransParameters(null, America.WGS84, -582.33, 671.57, -108.15, 6.495, 1.744, 0.56, 2.876);

        /// <summary>
        /// The transformation parameters from Timbalai 1948 (Bessel) to WGS84 (Malaysia E & Brunei)
        /// </summary>
        public static readonly TransParameters Malaysia_Bessel_WGS84 = new TransParameters(null, America.WGS84, -496.34, 580.76, -44.31, 8.82, 0.098, 0.018, 4.146);

        /// <summary>
        /// The transformation parameters from Leigon to WGS84 (Ghana)
        /// </summary>
        public static readonly TransParameters Ghana_WGS84 = new TransParameters(null, America.WGS84, -135.58, 13.23, 364.13, 0.719, 2.0168, -0.0256, 0.8091);

        /// <summary>
        /// The transformation parameters from Lisbon (Castelo di Sao Jorge) D73 to WGS84 (Portugal)
        /// </summary>
        public static readonly TransParameters Portugal_WGS84 = new TransParameters(null, America.WGS84, -238.2, 85.2, 29.9, 2.03, 0.166, 0.046, 1.248);

        /// <summary>
        /// The transformation parameters from Rome 1940 to WGS84 (Italy - Sicily)
        /// </summary>
        public static readonly TransParameters ItalySicily_WGS84 = new TransParameters(null, America.WGS84, -50.2, -50.4, 84.8, -28.08, -0.69, -2.012, 0.459);

        /// <summary>
        /// The transformation parameters from NGO 1948 to WGS84 (Norway)
        /// </summary>
        public static readonly TransParameters Norway_WGS84 = new TransParameters(null, America.WGS84, 278.3, 93, 474.5, 6.21, 7.889, 0.05, -6.61);

        /// <summary>
        /// The transformation parameters from Belgium Datum 1972 to WGS84 (Belgium)
        /// </summary>
        public static readonly TransParameters Belgium_WGS84 = new TransParameters(null, America.WGS84, -99.1, 53.3, -112.5, -1, 0.419, -0.83, 1.885);

        /// <summary>
        /// The transformation parameters from Ordnance Greate Britain 1936 to WGS84 (Great Britain)
        /// </summary>
        public static readonly TransParameters GreatBritain_WGS84 = new TransParameters(null, America.WGS84, 446.448, -125.157, 542.06, -20.49, 0.15, 0.247, 0.8421);

        /// <summary>
        /// The transformation parameters from Prov. S American 1956 to WGS84 (Venezuela)
        /// </summary>
        public static readonly TransParameters Venezuela_WGS84 = new TransParameters(null, America.WGS84, -197.43, 139.39, -192.8, -5.109, 5.266, 1.238, -2.381);

        /// <summary>
        /// The transformation parameters from Pulkovo 1942 to WGS84 (Estonia)
        /// </summary>
        public static readonly TransParameters Estonia_WGS84 = new TransParameters(null, America.WGS84, 21.58719, -97.541, -60.925, -4.6121, 1.01378, 0.58117, 0.2348);

        /// <summary>
        /// The transformation parameters from Qatar National 1974 to WGS84 (Qatar)
        /// </summary>
        public static readonly TransParameters Qatar1974_WGS84 = new TransParameters(null, America.WGS84, -126.44, -298.86, -10.92, 3.73, 1.23, 0.27, 0.85);

        /// <summary>
        /// The transformation parameters from Qatar National 1995 to WGS84 (Qatar)
        /// </summary>
        public static readonly TransParameters Qatar1995_WGS84 = new TransParameters(null, America.WGS84, -119.425, -303.6587, -11.00061, 3.65707, 1.1643, 0.17446, 1.096259);

        /// <summary>
        /// The transformation parameters from DHDN (Rauenberg) to WGS84 (Germany)
        /// </summary>
        public static readonly TransParameters Germany_WGS84 = new TransParameters(null, America.WGS84, 582, 105, 414, 8.3, 1.04, 0.35, -3.08);

        /// <summary>
        /// The transformation parameters from RT90 to WGS84 (Sweden)
        /// </summary>
        public static readonly TransParameters Sweden_WGS84 = new TransParameters(null, America.WGS84, 414.1, 41.3, 603.1, 0, -0.855, 2.141, -7.023);

        /// <summary>
        /// The transformation parameters from Sapper Hill 1943 (2000 adjustment) to WGS84 (Falkland Islands)
        /// </summary>
        public static readonly TransParameters FalklandIslands_WGS84 = new TransParameters(null, America.WGS84, -120.379, 126.358, 95.91, -0.349, -0.09247, 2.49933, -10.54206);

        /// <summary>
        /// The transformation parameters from South East Island to WGS84 (Seychelles)
        /// </summary>
        public static readonly TransParameters Seychelles_WGS84 = new TransParameters(null, America.WGS84, 30.768, -129.01, -91.673, -10.901, -1.9847, 7.51328, 0.64532);

        /// <summary>
        /// The transformation parameters from Soviet Geodetic System 1985 to WGS84 (Russia)
        /// </summary>
        public static readonly TransParameters Russia_WGS84 = new TransParameters(null, America.WGS84, 0, 0, 4, 0, 0, 0, 0.6);

        /// <summary>
        /// The transformation parameters from System 1942/58 (Pulkovo 1942) to WGS84 (Poland)
        /// </summary>
        public static readonly TransParameters Poland_WGS84 = new TransParameters(null, America.WGS84, 33.4, -146.6, -76.3, -0.84, -0.359, -0.053, 0.844);

        /// <summary>
        /// The transformation parameters from Tananarive Observatory to WGS84 (Madagascar)
        /// </summary>
        public static readonly TransParameters Madagascar_WGS84 = new TransParameters(null, America.WGS84, -242.75, -191.8, -105.56, 1.149, 0.913, 1.137, -2.698);

        /// <summary>
        /// The transformation parameters from Luxembourg NT to WGS84 (Luxembourg)
        /// </summary>
        public static readonly TransParameters Luxembourg_WGS84 = new TransParameters(null, America.WGS84, -193, 13.7, -39.3, 0.43, -0.41, -2.933, 2.688);

        #endregion
    }
}

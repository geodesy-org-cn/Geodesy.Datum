using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum.Earth.Projection
{
    /// <summary>
    /// The Military Grid Reference System (MGRS) is the geocoordinate standard used by 
    /// NATO militaries for locating points on Earth. The MGRS is derived from the 
    /// Universal Transverse Mercator (UTM) grid system and the Universal Polar Stereographic 
    /// (UPS) grid system, but uses a different labeling convention. The MGRS is used as 
    /// geocode for the entire Earth.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class MGRS : UniversalTransverseMercator
    {
        /// <summary>
        /// Collection of letters in east
        /// </summary>
        private const string EastLetters = "ABCDEFGHJKLMNPQRSTUVWXYZ";

        /// <summary>
        /// Collection of letters in north
        /// </summary>
        private const string NorthLetters = "ABCDEFGHJKLMNPQRSTUV";

        /// <summary>
        /// Size of square is 100,000 meter
        /// </summary>
        private const long SquareSize = 100000; 

        /// <summary>
        /// Create a MGRS projection object.
        /// </summary>
        public MGRS()
            :this(Settings.Ellipsoid)
        { }

        /// <summary>
        /// Create a MGRS projection object.
        /// </summary>
        /// <param name="ellipsoid">EarthEllipsoid data model for earth</param>
        public MGRS(Ellipsoid ellipsoid)
            : base(ellipsoid)
        {
            Identifier = new Identifier(typeof(MGRS));
        }

        /// <summary>
        /// Decode s MGRS string to a UTM projected coordinate.
        /// </summary>
        /// <param name="mgrs">MGRS string coordinate</param>
        /// <param name="zone">longitude zone</param>
        /// <param name="band">latitude band</param>
        /// <param name="letterE">letter in east direction</param>
        /// <param name="letterN">letter in north direction</param>
        /// <param name="east">east component</param>
        /// <param name="north">north component</param>
        /// <returns>the MGRS string is valid or invalid</returns>
        private bool Decode(string mgrs, out int zone, out char band, out char letterE, out char letterN, out double east, out double north)
        {
            // Invalid value as default when the decoding occurs error.
            zone = 0;
            band = '-';
            letterE = '-';
            letterN = '-';
            east = 0;
            north = 0;

            // delete the delimeter in MGRS string
            mgrs = mgrs.ToUpper().Replace("-", "").Replace(" ", "");

            // If zone is from 1 to 9, insert 0 at the head of string.
            if (mgrs.Length % 2 == 0) mgrs = "0" + mgrs;

            try
            {
                zone = Convert.ToInt32(mgrs.Substring(0, 2));

                char[] temp = mgrs.ToCharArray();
                band = temp[2];
                letterE = temp[3];
                letterN = temp[4];

                int bits = (mgrs.Length - 5) / 2;
                east = Convert.ToDouble(mgrs.Substring(5, bits)) * Math.Pow(10, 5 - bits);
                north = Convert.ToDouble(mgrs.Substring(5 + bits, bits)) * Math.Pow(10, 5 - bits);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Convert a geographic coordinate to MGRS string
        /// </summary>
        /// <param name="coord">geographic coordinate</param>
        /// <param name="bits">bits of coordinate component value</param>
        /// <returns>MGRS string</returns>
        public string ToMgrs(GeographicCoord coord, int bits)
        {
            return ToMgrs(coord.Latitude, coord.Longitude, bits);
        }

        /// <summary>
        /// Convert a UTM projected coordinate to MGRS string.
        /// </summary>
        /// <param name="hemisphere">Hemisphere char ('N' for Northern, 'S' for Southern)</param>
        /// <param name="zone">UTM Longitudinal Zone number (1 to 60)</param>
        /// <param name="easting">east component value</param>
        /// <param name="northing">north component value</param>
        /// <param name="bits">data bit of per component</param>
        /// <returns>MGRS string</returns>
        public string ToMgrs(char hemisphere, char zone, double easting, double northing, int bits)
        {
            // inverse projection to get latitude band.
            _lngZone = zone;
            _hemisphere = hemisphere;
            Reverse(northing, easting, out _, out _);

            string mgrs = _lngZone.ToString() + LatBand.ToString();

            // letterE repeats from 'A' to 'Z' without 'I', 'O'. Every 6 zones is a group. 
            // Every one zone uses 8 number.
            // In longitude direction, grid muber is from 0km to 100km, so 'A' abstracts 1.
            int letterE = 'A' - 1 + (((zone - 1) % 6) * 8 + (int)Math.Floor(easting / 100000)) % 24;
            if (letterE > 'J') letterE++;                   // skip 'I'
            if (letterE > 'N') letterE++;                   // skip 'O'
            // letterN repeats from 'A' to 'V' without 'I', 'O'. 
            // In latitude direction, grid number is from 100 to 200km, so 'A' dosen't abstrat 1.
            int letterN = (zone % 2 == 1 ? 'A' : 'F') + (int)Math.Floor(northing / 100000) % 20;
            if (letterN > 'J') letterN++;                   // skip 'I'
            if (letterN > 'N') letterN++;                   // skip 'O'
            mgrs += Convert.ToChar(letterE).ToString() + Convert.ToChar(letterN).ToString();

            int east = (int)((easting % 100000) * Math.Pow(10, bits - 5));
            int north = (int)((northing % 100000) * Math.Pow(10, bits - 5));
            mgrs += String.Format("{0:0}{1:0}", east, north);

            return mgrs;
        }

        /// <summary>
        /// Convert latitude longitude coordinate to MGRS string
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="bits">data bit of per component</param>
        /// <returns>MGRS string</returns>
        public string ToMgrs(Latitude lat, Longitude lng, int bits)
        {
            char band = GetLatBand(lat);
            int zone = GetLngZone(lng, band);
            string mgrs = zone.ToString() + band.ToString();

            Forward(lat, lng, out double northing, out double easting);

            // refrence: https://www.maptools.com/images/5e63b08.png

            // letterE repeats from 'A' to 'Z' without 'I', 'O'. Every 6 zones is a group. 
            // Every one zone uses 8 number.
            // In longitude direction, grid muber is from 0km to 100km, so 'A' abstracts 1.
            int letterE = 'A' - 1 + (((zone - 1) % 6) * 8 + (int)Math.Floor(easting / 100000)) % 24;
            if (letterE > 'J') letterE++;                    // skip 'I'
            if (letterE > 'N') letterE++;                    // skip 'O'
            // letterN repeats from 'A' to 'V' without 'I', 'O'. 
            // In latitude direction, grid number is from 100 to 200km, so 'A' dosen't abstrat 1.
            int letterN = (zone % 2 == 1 ? 'A' : 'F') + (int)Math.Floor(northing / 100000) % 20;
            if (letterN > 'J') letterN++;                    // skip 'I'
            if (letterN > 'N') letterN++;                    // skip 'O'
            mgrs += Convert.ToChar(letterE).ToString() + Convert.ToChar(letterN).ToString();

            int east = (int)((easting % 100000) * Math.Pow(10, bits - 5));
            int north = (int)((northing % 100000) * Math.Pow(10, bits - 5));
            mgrs += String.Format("{0:0}{1:0}", east, north);

            return mgrs;
        }

        /// <summary>
        /// Convert MGRS string to geographic coordinate.
        /// </summary>
        /// <param name="mgrs">MGRS string</param>
        /// <returns>geographic coordinate</returns>
        public GeographicCoord FromMgrs(string mgrs)
        {
            FromMgrs(mgrs, out Latitude lat, out Longitude lng);
            return new GeographicCoord(lat, lng);
        }

        /// <summary>
        /// Convert a MGRS string to gegraphic coordinate
        /// </summary>
        /// <param name="mgrs">MGRS string</param>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        public void FromMgrs(string mgrs, out Latitude lat, out Longitude lng)
        {
            FromMgrs(mgrs, out double easting, out double northing);
            Reverse(northing, easting, out lat, out lng);
        }

        /// <summary>
        /// Convert a MGRS string to UTM projected coordinate.
        /// </summary>
        /// <param name="mgrs">MGRS string</param>
        /// <param name="easting">east of projected coordinate</param>
        /// <param name="northing">north of projected coordinate</param>
        public void FromMgrs(string mgrs, out double easting, out double northing)
        {
            Validate(mgrs);

            string letters = string.Empty;
            for (int i = 1; i < 25; i++)
            {
                letters += "ABCDEFGHJKLMNPQRSTUV";
            }

            Decode(mgrs, out _lngZone, out char band, out char letterE, out char letterN, out double east, out double north);

            var eIndex = EastLetters.IndexOf(letterE);
            var nIndex = NorthLetters.IndexOf(letterN);
            if (_lngZone / 2.0 == Math.Floor(_lngZone / 2.0))
            {
                nIndex -= 5;  // correction for even numbered zones
            }

            var eBase = SquareSize * (1 + eIndex - 8 * Math.Floor(Convert.ToDouble(eIndex) / 8));
            var latBand = EastLetters.IndexOf(band);
            var latBandLow = 8 * latBand - 96;
            var latBandHigh = 8 * latBand - 88;

            if (latBand < 2)
            {
                latBandLow = -90;
                latBandHigh = -80;
            }
            else if (latBand == 21)
            {
                latBandLow = 72;
                latBandHigh = 84;
            }
            else if (latBand > 21)
            {
                latBandLow = 84;
                latBandHigh = 90;
            }

            var lowLetter = Math.Floor(100 + 1.11 * latBandLow);
            var highLetter = Math.Round(100 + 1.11 * latBandHigh);
            int l = Convert.ToInt32(lowLetter);
            int h = Convert.ToInt32(highLetter);

            string latBandLetters;
            if (_lngZone / 2.0 == Math.Floor(_lngZone / 2.0))
            {
                latBandLetters = letters.Substring(l + 5, h + 5).ToString();
            }
            else
            {
                latBandLetters = letters.Substring(l, h).ToString();
            }

            var nBase = SquareSize * (lowLetter + latBandLetters.IndexOf(letterN));
            //latBandLetters.IndexOf(nltr) value causing incorrect Northing below -80
            easting = eBase + east;
            northing = nBase + north;
            if (northing > FalseNorthing)
            {
                northing -= FalseNorthing;
            }
            if (nBase >= FalseNorthing)
            {
                northing = nBase + north - FalseNorthing;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mgrs"></param>
        /// <returns></returns>
        public bool Validate(string mgrs)
        {
            if (!Decode(mgrs, out int zone, out char band, out char letterE, out char letterN, out double easting, out double northing))
            {
                throw new GeodeticException("The MGRS string can not be decode.");
            }

            if (zone < 1 || zone > 60)
            {
                throw new GeodeticException("Longitudinal zone out of range. UTM longitudinal zones must be between 1-60.");
            }

            if (band < 'C' || band == 'I' || band == 'O' || band > 'X')
            {
                throw new GeodeticException("Latitudinal zone invalid. UTM latitudinal zone was unrecognized.");
            }

            if (!EastLetters.Contains(letterE.ToString()))
            {
                throw new GeodeticException("East letter invalid. MGRS east letter was unrecognized.");
            }

            if (!NorthLetters.Contains(letterN.ToString()))
            {
                throw new GeodeticException("North letter invalid. MGRS north letter was unrecognized.");
            }

            if (northing < 0 || northing > 10000000)
            {
                throw new GeodeticException("Northing out of range. Northing must be between 0-10,000,000.");
            }

            return true;
        }
    }
}

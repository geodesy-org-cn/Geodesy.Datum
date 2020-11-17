using System;
using Newtonsoft.Json;
using Geodesy.Datum.Coordinate;

namespace Geodesy.Datum
{
    /// <summary>
    /// Geodetic origin
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public sealed class GeodeticOrigin
    {
        /// <summary>
        /// create a geodetic origin object
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="name">origin point name</param>
        public GeodeticOrigin(Latitude lat, Longitude lng, string name = "")
        {
            Name = name;
            Latitude = lat;
            Longitude = lng;

            Height = double.NaN;
            Azimuth = Angle.NaN;
        }

        /// <summary>
        /// create a geodetic origin object
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="hgt">height</param>
        /// <param name="azm">azimuth</param>
        /// <param name="name">origin point name</param>
        public GeodeticOrigin(Latitude lat, Longitude lng, double hgt, Angle azm, string name = "")
            : this(lat, lng, name)
        {
            Height = hgt;
            Azimuth = azm;
        }

        /// <summary>
        /// create a geodetic origin object
        /// </summary>
        /// <param name="pnt">origin point</param>
        /// <param name="azm">azimuth</param>
        /// <param name="name">origin point name</param>
        public GeodeticOrigin(GeodeticCoord pnt, Angle azm, string name = "")
        {
            Name = name;
            Latitude = pnt.Latitude;
            Longitude = pnt.Longitude;
            Height = pnt.Height;
            Azimuth = azm;
        }

        #region properties
        /// <summary>
        /// Name of geodetic origin point
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of geodetic origin point.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Latitude of geodetic origin point
        /// </summary>
        [JsonConverter(typeof(AngleJsonConverter<Latitude>))]
        public Latitude Latitude { get; set; }

        /// <summary>
        /// Longitude of geodetic origin point
        /// </summary>
        [JsonConverter(typeof(AngleJsonConverter<Longitude>))]
        public Longitude Longitude { get; set; }

        /// <summary>
        /// Geodetic height of geodetic origin point
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Azimuth to some direction of geodetic origin point
        /// </summary>
        [JsonConverter(typeof(AngleJsonConverter<Angle>))]
        public Angle Azimuth { get; set; }

        /// <summary>
        /// Ellipsoid orientation type
        /// </summary>
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public EllipsoidOrientation Orientation { get; set; }

        /// <summary>
        /// Location of geodetic origin point
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Vertical deviation
        /// </summary>
        public VerticalDeviation VerticalDeviation { get; set; }

        /// <summary>
        /// Elevation anomaly
        /// </summary>
        public double ElevationAnomaly { get; set; }
        #endregion

        #region get/set data value
        /// <summary>
        /// set geodetic origin data
        /// </summary>
        /// <param name="lng">longitude</param>
        /// <param name="lat">latitude</param>
        /// <param name="hgt">height</param>
        /// <param name="az">azimuth</param>
        public void SetValue(Latitude lat, Longitude lng, double hgt, Angle az)
        {
            Longitude = lng;
            Latitude = lat;
            Height = hgt;
            Azimuth = az;
        }

        /// <summary>
        /// set geodetic origin data
        /// </summary>
        /// <param name="point">getdetic point</param>
        /// <param name="azm">azimuth</param>
        public void SetValue(GeodeticCoord point, Angle azm)
        {
            Longitude = point.Longitude;
            Latitude = point.Latitude;
            Height = point.Height;
            Azimuth = azm;
        }

        /// <summary>
        /// get geodetic origin data
        /// </summary>
        /// <param name="L">longitude</param>
        /// <param name="lat">latitude</param>
        /// <param name="hgt">height</param>
        /// <param name="azm">azimuth</param>
        public void GetValue(out Latitude lat, out Longitude lng, out double hgt, out Angle azm)
        {
            lng = Longitude;
            lat = Latitude;
            hgt = Height;
            azm = Azimuth;
        }

        /// <summary>
        /// get geodetic origin data
        /// </summary>
        /// <param name="point">geodetic point</param>
        /// <param name="azm">azimuth</param>
        public void GetValue(out GeodeticCoord point, out Angle azm)
        {
            point = new Coordinate.GeodeticCoord(Latitude, Longitude, Height);
            azm = Azimuth;
        }
        #endregion

        #region constants
        /// <summary>
        /// Xi'An 1980
        /// </summary>
        public static readonly GeodeticOrigin XiAn1980 = new GeodeticOrigin(new Latitude(34, 32, 27), new Longitude(108, 55, 25), "Xi'An 1980");


        /// 
        ///          http://earth-info.nga.mil/GandG/coordsys/datums/datumorigins.html
        /// 

        /********************************************************************************************/
        /*                             《Practical Geodesy： Use Computer》P26-27                    */
        /********************************************************************************************/

        /// <summary>
        /// Station 15 Adindan
        /// </summary>
        public static readonly GeodeticOrigin Station15Adindan = new GeodeticOrigin(new Latitude(22, 10, 7.11), new Longitude(31, 29, 21.608), "Station 15 Adindan");

        /// <summary>
        /// Betty 13 ECC
        /// </summary>
        public static readonly GeodeticOrigin Betty13ECC = new GeodeticOrigin(new Latitude(-14, 20, 8.34), new Longitude(-170, 42, 52.25), "Betty 13 ECC");

        /// <summary>
        /// Buffelsfontein
        /// </summary>
        public static readonly GeodeticOrigin Buffelsfontein = new GeodeticOrigin(new Latitude(-33, 59, 32), new Longitude(25, 30, 44.622), "Buffelsfontein");

        /// <summary>
        /// Campo Inchauspe
        /// </summary>
        public static readonly GeodeticOrigin CampoInchauspe = new GeodeticOrigin(new Latitude(-35, 58, 16.56), new Longitude(-62, 10, 12.03), "Campo Inchauspe");

        /// <summary>
        /// Mean of three Sta.
        /// </summary>
        public static readonly GeodeticOrigin MeanOfThreeSta = new GeodeticOrigin(new Latitude(-7, 57, 0), new Longitude(-14, 23, 0), "Mean of three Sta.");

        /// <summary>
        /// O.L.-Vrouwe Tower
        /// </summary>
        public static readonly GeodeticOrigin OLVrouweTower = new GeodeticOrigin(new Latitude(52, 9, 22.178), new Longitude(5, 23, 15.5), "O.L.-Vrouwe Tower");

        /// <summary>
        /// Johnston Geodetic
        /// </summary>
        public static readonly GeodeticOrigin JohnstonGeodetic = new GeodeticOrigin(new Latitude(-25, 56, 54.5515), new Longitude(133, 12, 30.0771), "Johnston Geodetic");

        /// <summary>
        /// Ft. George
        /// </summary>
        public static readonly GeodeticOrigin FtGeorge = new GeodeticOrigin(new Latitude(32, 22, 44.36), new Longitude(-64, 40, 58.11), "Ft. George");

        /// <summary>
        /// Bern Observatory
        /// </summary>
        public static readonly GeodeticOrigin BernObservatory = new GeodeticOrigin(new Latitude(46, 57, 8.66), new Longitude(7, 26, 22.5), "Bern Observatory");

        /// <summary>
        /// 1966 Secor Astro (Betio Island 1966 datum)
        /// </summary>
        public static readonly GeodeticOrigin SecorAstro1966_BetioIsland = new GeodeticOrigin(new Latitude(1, 21, 42.03), new Longitude(172, 55, 47.9), "1966 Secor Astro");

        /// <summary>
        /// Royal Observatory
        /// </summary>
        public static readonly GeodeticOrigin RoyalObservatory = new GeodeticOrigin(new Latitude(50, 47, 57.704), new Longitude(4, 21, 24.983), "Royal Observatory");

        /// <summary>
        /// Camp Area Astro
        /// </summary>
        public static readonly GeodeticOrigin CampAreaAstro = new GeodeticOrigin(new Latitude(-77, 50, 52.521), new Longitude(166, 40, 13.753), "Camp Area Astro");

        /// <summary>
        /// 1966 Canton Secor
        /// </summary>
        public static readonly GeodeticOrigin CantonSecor1966 = new GeodeticOrigin(new Latitude(-2, 46, 28.99), new Longitude(-171, 43, 16.53), "1966 Canton Secor");

        /// <summary>
        /// Chua Astro
        /// </summary>
        public static readonly GeodeticOrigin ChuaAstro = new GeodeticOrigin(new Latitude(-19, 45, 41.16), new Longitude(-48, 6, 7.56), "Chua Astro");

        /// <summary>
        /// Corrego Alegre
        /// </summary>
        public static readonly GeodeticOrigin CorregoAlegre = new GeodeticOrigin(new Latitude(-19, 50, 15.14), new Longitude(-48, 57, 42.75), "Corrego Alegre");

        /// <summary>
        /// Potsdam-Helmert Twr (European 1950 datum)
        /// </summary>
        public static readonly GeodeticOrigin PotsdamHelmertTwr_European1950 = new GeodeticOrigin(new Latitude(52, 22, 51.4456), new Longitude(13, 3, 58.9283), "Potsdam-Helmert Twr");

        /// <summary>
        /// Munich D-7835
        /// </summary>
        public static readonly GeodeticOrigin MunichD7835 = new GeodeticOrigin(new Latitude(48, 8, 22.2273), new Longitude(11, 34, 26.4862), "Munich D-7835");

        /// <summary>
        /// Papatahi Trig.
        /// </summary>
        public static readonly GeodeticOrigin PapatahiTrig = new GeodeticOrigin(new Latitude(-41, 19, 8.9), new Longitude(175, 2, 51), "Papatahi Trig.");

        /// <summary>
        /// Leigon G.C.S 121
        /// </summary>
        public static readonly GeodeticOrigin LeigonGCS121 = new GeodeticOrigin(new Latitude(5, 38, 52.27), new Longitude(-0, 11, 46.08), "Leigon G.C.S 121");

        /// <summary>
        /// SW Base 1948
        /// </summary>
        public static readonly GeodeticOrigin SWBase1948 = new GeodeticOrigin(new Latitude(39, 3, 54.934), new Longitude(-28, 2, 23.882), "SW Base 1948");

        /// <summary>
        /// Papatahi
        /// </summary>
        public static readonly GeodeticOrigin Papatahi = new GeodeticOrigin(new Latitude(-41, 19, 8.9), new Longitude(175, 2, 51), "Papatahi");

        /// <summary>
        /// Gux I
        /// </summary>
        public static readonly GeodeticOrigin GuxI = new GeodeticOrigin(new Latitude(-9, 27, 5.272), new Longitude(159, 58, 31.752), "Gux I");

        /// <summary>
        /// Togcha or Lee NO.7
        /// </summary>
        public static readonly GeodeticOrigin TogchaOrLeeNO7 = new GeodeticOrigin(new Latitude(13, 22, 38.49), new Longitude(144, 58, 51.56), "Togcha or Lee NO.7");

        /// <summary>
        /// Hjorsey
        /// </summary>
        public static readonly GeodeticOrigin Hjorsey = new GeodeticOrigin(new Latitude(64, 31, 29.26), new Longitude(-22, 22, 5.84), "Hjorsey");

        /// <summary>
        /// Hu-Tzu-Shan
        /// </summary>
        public static readonly GeodeticOrigin HuTzuShan = new GeodeticOrigin(new Latitude(23, 58, 32.34), new Longitude(120, 58, 25.975), "Hu-Tzu-Shan");

        /// <summary>
        /// Iben Astro 1947
        /// </summary>
        public static readonly GeodeticOrigin IbenAstro1947 = new GeodeticOrigin(new Latitude(7, 29, 13.05), new Longitude(151, 49, 44.42), "Iben Astro 1947");

        /// <summary>
        /// Paris 1922
        /// </summary>
        public static readonly GeodeticOrigin Paris1922 = new GeodeticOrigin(new Latitude(48, 50, 14), new Longitude(2, 20, 12.025), "Paris 1922");

        /// <summary>
        /// Kalianpur 1895
        /// </summary>
        public static readonly GeodeticOrigin Kalianpur1895 = new GeodeticOrigin(new Latitude(24, 7, 11.26), new Longitude(77, 39, 17.57), "Kalianpur 1895");

        /// <summary>
        /// Padang
        /// </summary>
        public static readonly GeodeticOrigin Padang = new GeodeticOrigin(new Latitude(0, 56, 38.414, 'S'), new Longitude(100, 22, 8.804), "Padang");

        /// <summary>
        /// Donard Slieve
        /// </summary>
        public static readonly GeodeticOrigin DonardSlieve = new GeodeticOrigin(new Latitude(54, 10, 48.2675), new Longitude(-5, 55, 11.8675), "Donard Slieve");

        /// <summary>
        /// Station 038
        /// </summary>
        public static readonly GeodeticOrigin Station038 = new GeodeticOrigin(new Latitude(18, 43, 44.93), new Longitude(-110, 57, 20.72), "Station 038");

        /// <summary>
        /// Johnston Island 1961
        /// </summary>
        public static readonly GeodeticOrigin JohnstonIsland1961 = new GeodeticOrigin(new Latitude(16, 44, 49.729), new Longitude(-169, 30, 55.219), "Johnston Island 1961");

        /// <summary>
        /// Allen Sodano Light
        /// </summary>
        public static readonly GeodeticOrigin AllenSodanoLight = new GeodeticOrigin(new Latitude(-5, 21, 48.8), new Longitude(162, 58, 3.28), "Allen Sodano Light");

        /// <summary>
        /// Robertsfield Astro
        /// </summary>
        public static readonly GeodeticOrigin RobertsfieldAstro = new GeodeticOrigin(new Latitude(6, 13, 53.02), new Longitude(-10, 21, 35.44), "Robertsfield Astro");

        /// <summary>
        /// Ba1anacan
        /// </summary>
        public static readonly GeodeticOrigin Balanacan = new GeodeticOrigin(new Latitude(13, 33, 41), new Longitude(121, 52, 3), "Ba1anacan");

        /// <summary>
        /// Midway Astro 1961
        /// </summary>
        public static readonly GeodeticOrigin MidwayAstro1961 = new GeodeticOrigin(new Latitude(28, 11, 34.5), new Longitude(-177, 23, 35.72), "Midway Astro 1961");

        /// <summary>
        /// Merchich
        /// </summary>
        public static readonly GeodeticOrigin Merchich = new GeodeticOrigin(new Latitude(33, 26, 59.672), new Longitude(-7, 33, 27.295), "Merchich");

        /// <summary>
        /// Schwarzeck
        /// </summary>
        public static readonly GeodeticOrigin Schwarzeck = new GeodeticOrigin(new Latitude(-22, 45, 35.82), new Longitude(18, 40, 34.549), "Schwarzeck");

        /// <summary>
        /// Naporima
        /// </summary>
        public static readonly GeodeticOrigin Naporima = new GeodeticOrigin(new Latitude(10, 16, 44.86), new Longitude(-61, 27, 34.62), "Naporima");

        /// <summary>
        /// Minna
        /// </summary>
        public static readonly GeodeticOrigin Minna = new GeodeticOrigin(new Latitude(9, 38, 8.87), new Longitude(6, 30, 58.76), "Minna");

        /// <summary>
        /// Meades Ranch
        /// </summary>
        public static readonly GeodeticOrigin MeadesRanch = new GeodeticOrigin(new Latitude(39, 13, 26.686), new Longitude(-98, 32, 30.506), "Meades Ranch");

        /// <summary>
        /// Central
        /// </summary>
        public static readonly GeodeticOrigin Central = new GeodeticOrigin(new Latitude(28, 29, 32.364), new Longitude(-80, 34, 38.77), "Central");

        /// <summary>
        /// Kent 1909
        /// </summary>
        public static readonly GeodeticOrigin Kent1909 = new GeodeticOrigin(new Latitude(32, 30, 27.079), new Longitude(-106, 28, 58.694), "Kent 1909");

        /// <summary>
        /// Munich
        /// </summary>
        public static readonly GeodeticOrigin Munich = new GeodeticOrigin(new Latitude(48, 8, 20), new Longitude(11, 34, 26.483), "Munich");

        /// <summary>
        /// Oahu West Base
        /// </summary>
        public static readonly GeodeticOrigin OahuWestBase = new GeodeticOrigin(new Latitude(21, 18, 13.89), new Longitude(-157, 50, 55.8), "Oahu West Base");

        /// <summary>
        /// Herstmonceux
        /// </summary>
        public static readonly GeodeticOrigin Herstmonceux = new GeodeticOrigin(new Latitude(50, 51, 55.271), new Longitude(0, 20, 45.882), "Herstmonceux");

        /// <summary>
        /// Pico de las Nieves
        /// </summary>
        public static readonly GeodeticOrigin PicoDeLasNieves = new GeodeticOrigin(new Latitude(27, 57, 41.273), new Longitude(-15, 34, 10.524), "Pico de las Nieves");

        /// <summary>
        /// La Canoa
        /// </summary>
        public static readonly GeodeticOrigin LaCanoa = new GeodeticOrigin(new Latitude(8, 34, 17.17), new Longitude(-63, 51, 34.88), "La Canoa");

        /// <summary>
        /// Hito XVIII, data is error
        /// </summary>
        //public static readonly GeodeticOrigin HitoXVIII = new GeodeticOrigin(new Latitude(-64, 67, 7.76), new Longitude(-68, 36, 31.24), "Hito XVIII");

        /// <summary>
        /// Station 7008
        /// </summary>
        public static readonly GeodeticOrigin Station7008 = new GeodeticOrigin(new Latitude(64, 31, 6.27), new Longitude(-51, 12, 24.86), "Station 7008");

        /// <summary>
        /// St. Peter Church (old)
        /// </summary>
        public static readonly GeodeticOrigin StPeterChurchOld = new GeodeticOrigin(new Latitude(56, 56, 53.919), new Longitude(24, 6, 31.898), "St. Peter Church (old)");

        /// <summary>
        /// St. Peter Church (low)
        /// </summary>
        public static readonly GeodeticOrigin StPeterChurchLow = new GeodeticOrigin(new Latitude(56, 56, 53.935), new Longitude(24, 6, 31.937), "St. Peter Church (low)");

        /// <summary>
        /// M. Mario
        /// </summary>
        public static readonly GeodeticOrigin MMario = new GeodeticOrigin(new Latitude(41, 55, 25.51), new Longitude(12, 27, 8.4), "M. Mario");

        /// <summary>
        /// DOS Astro SLX2
        /// </summary>
        public static readonly GeodeticOrigin DOSAstroSLX2 = new GeodeticOrigin(new Latitude(8, 27, 17.6), new Longitude(-12, 49, 40.2), "DOS Astro SLX2");

        /// <summary>
        /// Chua (Brazil)
        /// </summary>
        public static readonly GeodeticOrigin ChuaBrazil = new GeodeticOrigin(new Latitude(-19, 45, 41.6527), new Longitude(-48, 6, 4.0639), "Chua (Brazil)");

        /// <summary>
        /// 1966 Secor Astro (Swallow Islands datum)
        /// </summary>
        public static readonly GeodeticOrigin SecorAstro1966_SwallowIslands = new GeodeticOrigin(new Latitude(-10, 18, 21.42), new Longitude(166, 17, 56.79), "1966 Secor Astro");

        /// <summary>
        /// Potsdam-Helmert Twr (Rauenberg datum)
        /// </summary>
        public static readonly GeodeticOrigin PotsdamHelmertTwr_Rauenberg = new GeodeticOrigin(new Latitude(52, 22, 53.954), new Longitude(13, 4, 1.1527), "Potsdam-Helmert Twr");

        /// <summary>
        /// Pulkovo Geodetic Obs. (System 1932 datum)
        /// </summary>
        public static readonly GeodeticOrigin Pulkovo1932 = new GeodeticOrigin(new Latitude(59, 46, 18.71), new Longitude(30, 19, 38.55), "Pulkovo Geodetic Obs.");

        /// <summary>
        /// Pulkovo Geodetic Obs. (System 1942 datum)
        /// </summary>
        public static readonly GeodeticOrigin Pulkovo1942 = new GeodeticOrigin(new Latitude(59, 46, 18.55), new Longitude(30, 19, 42.09), "Pulkovo Geodetic Obs.");

        /// <summary>
        /// Szolohegy
        /// </summary>
        public static readonly GeodeticOrigin Szolohegy = new GeodeticOrigin(new Latitude(47, 17, 52.6156), new Longitude(19, 36, 9.9865), "Szolohegy");

        /// <summary>
        /// Tananarive Obs. 1925 (Antananarivo)
        /// </summary>
        public static readonly GeodeticOrigin TananariveObs1925 = new GeodeticOrigin(new Latitude(-18, 55, 2.1), new Longitude(47, 33, 6.45), "Tananarive Obs. 1925");

        /// <summary>
        /// Tokyo Obs. (old)
        /// </summary>
        public static readonly GeodeticOrigin TokyoObsOld = new GeodeticOrigin(new Latitude(35, 39, 17.515), new Longitude(139, 44, 40.502), "Tokyo Obs. (old)");

        /// <summary>
        /// Voirol Observatory
        /// </summary>
        public static readonly GeodeticOrigin VoirolObservatory = new GeodeticOrigin(new Latitude(36, 45, 7.9), new Longitude(3, 2, 49.45), "Voirol Observatory");

        /// <summary>
        /// Astro 1952
        /// </summary>
        public static readonly GeodeticOrigin Astro1952 = new GeodeticOrigin(new Latitude(19, 16, 48.7), new Longitude(166, 38, 46.8), "Astro 1952");

        /// <summary>
        /// Yacare
        /// </summary>
        public static readonly GeodeticOrigin Yacare = new GeodeticOrigin(new Latitude(-30, 35, 53.68), new Longitude(-57, 25, 1.3), "Yacare");

        /// <summary>
        /// Yof Astro 1967
        /// </summary>
        public static readonly GeodeticOrigin YofAstro1967 = new GeodeticOrigin(new Latitude(14, 44, 41.62), new Longitude(-17, 29, 7.02), "Yof Astro 1967");
        #endregion
    }
}

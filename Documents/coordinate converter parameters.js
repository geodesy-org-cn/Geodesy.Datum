// FieldenMaps.info
// Co-ordinate Converter (Web) 'Parameters' JavaScript Functions
// Version 1.2.003a
// Ported from original Visual Basic code
// Revision Date: 4 May 2009
// Copyright © 2004-2009 Ed Fielden


// Initialise Pi constant
   var pi = Math.PI;


// Initialise "Foot of O1" constant
   var FO1 = 0.3048007491;


// Initialise string masks
   var numerics_d = '0123456789.';
   var numerics = '0123456789';
   var numerics_a = '0123456789ABCW';
   var alphanumerics = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
   var alphabetics = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';


// Initialise ellipsoids

    // Airy 1830 ellipsoid
       var Airy = new ellipsoid(6377563.3963534083, 6356256.909589071, 0.006670540000123428764, 0.081947147053796327738); // a = 20923713 feet of O1, b = 20853810 feet of O1

    // Airy 1830 Modified spheroid
       var AiryMod = new ellipsoid(Airy.a/1.000035, Airy.b/1.000035, Airy.e2, Airy.se);

    // GRS80/WGS84 ellipsoid
       var GRS80 = new ellipsoid(6378137, 6356752.314140356, 0.0066943800229007876, 0.0820944381519172);

    // International 1924 ellipsoid
       var Int24 = new ellipsoid(6378388, 6356911.946127946);


// Initialise Helmert 3- and 7- parameter datum transformation parameters (relative to WGS84)

    // OSGB36 datum
       var OSGB36 = new datum('OSGB36', Airy, -446.448, 125.157, -542.06, -0.1502, -0.247, -0.8421, 20.4894);

    // WGS84/ETRS89 datum
       var WGS84 = new datum('WGS84/ETRS89', GRS80, 0, 0, 0, 0, 0, 0, 0);

    // ED50 datum (EUR-G [1991] - England, Channel Islands, Scotland & Shetlands)
    //    same as (EUR-K [1991] - England, Ireland, Scotland & Shetlands)
       var ED50 = new datum('ED50', Int24, 86, 96, 120, 0, 0, 0, 0);

    // OS Ireland 65 datum
       var OSI65 = new datum('OS Ireland 65', AiryMod, -482.53, 130.596, -564.557, 1.042, 0.214, 0.631, -8.15);


// Initialise projection datums

    // OS GB National Grid (international metres)
       var OSNG = new TMgriddata(Airy, 0.9996012717, deg2rad(49), deg2rad(-2), 1, 'm', 400000, -100000, 0, 700000, 0, 1300000);

       // GB National Grid (GRS80 ellipsoid) - *** USED FOR INTERNAL COMPUTATION ONLY - not to be used otherwise ***
          var OSNGgps = new TMgriddata(GRS80, 0.9996012717, deg2rad(49), deg2rad(-2), 1, 'm', 400000, -100000, 0, 700000, 0, 1300000);

    // OS GB National Grid (British Yards)
       var OSYG = new TMgriddata(Airy, 0.9996, deg2rad(49), deg2rad(-2), FO1*3, 'yds', 1000000, 1000000, 500000, 1330000, 1000000, 2540000);

    // OS Ireland Grid (international metres)
       var OSIG = new TMgriddata(AiryMod, 1.000035, deg2rad(53.5), deg2rad(-8), 1, 'm', 200000, 250000, 0, 400000, 0, 500000);

    // Irish Transverse Mercator Grid (international metres)
       var ITMG = new TMgriddata(GRS80, 0.99982, deg2rad(53.5), deg2rad(-8), 1, 'm', 600000, 750000, 400000, 800000, 500000, 1000000);

    // Cassini (Delamere) co-ordinate system (British feet)
       var CDEL = new CSgriddata(Airy, 1, sec2rad(191597.274), sec2rad(-9663.562), FO1, 'ft', 0, 0, -1220200, 994230, -1249940, 2805100); 

    // War Office Cassini Grid (British metres)
       var WOCG = new CSgriddata(Airy, 1, sec2rad(182223.748), sec2rad(-4310.136), FO1/0.3047997333, 'm', 500000, 100000, 0, 800000, 0, 1300000);
       // WOCG.Lat0 = sec2rad(182223.7286);  modern-day location??
       // WOCG.Lon0 = sec2rad(-4310.1016);   modern-day location??

    // War Office Irish Grid (British metres)
       var WOIG = new CSgriddata(Airy, 1, deg2rad(53.5), deg2rad(-8), FO1/0.3047997333, 'm', 199990, 249975, 0, 400000, 0, 500000);

    // Bonne (Scotland) co-ordinate system (British feet)
       var BONS = new BNgriddata(Airy, 1, deg2rad(57.5), deg2rad(-4), FO1, 'ft', 0, 0, -804669, 589251, -1098523, 1277477);

    // Bonne (Ireland) co-ordinate system (British feet)
       var BONI = new BNgriddata(Airy, 1, deg2rad(53.5), deg2rad(-8), FO1, 'ft', 0, 0, -597280, 638240, -787960, 732680);

    // UTM zones 29, 30, 31 (for use with ED50 datum)
       var UTM29_ED = new TMgriddata(Int24, 0.9996, deg2rad(0), deg2rad(-9), 1, 'm', 500000, 0, 125000, 875000, 0, 9332600);
       var UTM30_ED = new TMgriddata(Int24, 0.9996, deg2rad(0), deg2rad(-3), 1, 'm', 500000, 0, 125000, 875000, 0, 9332600);
       var UTM31_ED = new TMgriddata(Int24, 0.9996, deg2rad(0), deg2rad(3), 1, 'm', 500000, 0, 125000, 875000, 0, 9332600);

    // UTM zones 29, 30, 31 (for use with WGS84 datum)
       var UTM29_WGS = new TMgriddata(GRS80, 0.9996, deg2rad(0), deg2rad(-9), 1, 'm', 500000, 0, 125000, 875000, 0, 9332300);
       var UTM30_WGS = new TMgriddata(GRS80, 0.9996, deg2rad(0), deg2rad(-3), 1, 'm', 500000, 0, 125000, 875000, 0, 9332300);
       var UTM31_WGS = new TMgriddata(GRS80, 0.9996, deg2rad(0), deg2rad(3), 1, 'm', 500000, 0, 125000, 875000, 0, 9332300);


// Initialise OSI polynomial transformation parameters

   var osik0=0.1;
   var osilatm=53.5;
   var osilonm=-7.7;
   var osiA=new Array(4); var osiB=new Array(4);
   for (ii=0; ii<=3; ii++) { osiA[ii]=new Array(4); osiB[ii]=new Array(4); }
   osiA[0][0]=0.763;  osiA[0][1]=0.123;   osiA[0][2]=0.183;   osiA[0][3]=-0.374;
   osiA[1][0]=-4.487; osiA[1][1]=-0.515;  osiA[1][2]=0.414;   osiA[1][3]=13.110;
   osiA[2][0]=0.215;  osiA[2][1]=-0.57;   osiA[2][2]=5.703;   osiA[2][3]=113.743;
   osiA[3][0]=-0.265; osiA[3][1]=2.852;   osiA[3][2]=-61.678; osiA[3][3]=-265.898;
   osiB[0][0]=-2.81;  osiB[0][1]=-4.68;   osiB[0][2]=0.170;   osiB[0][3]=2.163;
   osiB[1][0]=-0.341; osiB[1][1]=-0.119;  osiB[1][2]=3.913;   osiB[1][3]=18.867;
   osiB[2][0]=1.196;  osiB[2][1]=4.877;   osiB[2][2]=-27.795; osiB[2][3]=-284.294;
   osiB[3][0]=-0.887; osiB[3][1]=-46.666; osiB[3][2]=-95.377; osiB[3][3]=-853.95;


// initialise grid reference matrices

   // Lettered OSGB Grid Reference 100km Squares
   var OSNG_GS = new Array(7);
   for (i=0; i<OSNG_GS.length; i++) { OSNG_GS[i] = new Array(13); }
   OSNG_GS[0][0] = 'SV'; OSNG_GS[1][0] = 'SW'; OSNG_GS[2][0] = 'SX'; OSNG_GS[3][0] = 'SY'; OSNG_GS[4][0] = 'SZ'; OSNG_GS[5][0] = 'TV'; OSNG_GS[6][0] = 'TW';
   OSNG_GS[0][1] = 'SQ'; OSNG_GS[1][1] = 'SR'; OSNG_GS[2][1] = 'SS'; OSNG_GS[3][1] = 'ST'; OSNG_GS[4][1] = 'SU'; OSNG_GS[5][1] = 'TQ'; OSNG_GS[6][1] = 'TR';
   OSNG_GS[0][2] = 'SL'; OSNG_GS[1][2] = 'SM'; OSNG_GS[2][2] = 'SN'; OSNG_GS[3][2] = 'SO'; OSNG_GS[4][2] = 'SP'; OSNG_GS[5][2] = 'TL'; OSNG_GS[6][2] = 'TM';
   OSNG_GS[0][3] = 'SF'; OSNG_GS[1][3] = 'SG'; OSNG_GS[2][3] = 'SH'; OSNG_GS[3][3] = 'SJ'; OSNG_GS[4][3] = 'SK'; OSNG_GS[5][3] = 'TF'; OSNG_GS[6][3] = 'TG';
   OSNG_GS[0][4] = 'SA'; OSNG_GS[1][4] = 'SB'; OSNG_GS[2][4] = 'SC'; OSNG_GS[3][4] = 'SD'; OSNG_GS[4][4] = 'SE'; OSNG_GS[5][4] = 'TA'; OSNG_GS[6][4] = 'TB';
   OSNG_GS[0][5] = 'NV'; OSNG_GS[1][5] = 'NW'; OSNG_GS[2][5] = 'NX'; OSNG_GS[3][5] = 'NY'; OSNG_GS[4][5] = 'NZ'; OSNG_GS[5][5] = 'OV'; OSNG_GS[6][5] = 'OW';
   OSNG_GS[0][6] = 'NQ'; OSNG_GS[1][6] = 'NR'; OSNG_GS[2][6] = 'NS'; OSNG_GS[3][6] = 'NT'; OSNG_GS[4][6] = 'NU'; OSNG_GS[5][6] = 'OQ'; OSNG_GS[6][6] = 'OR';
   OSNG_GS[0][7] = 'NL'; OSNG_GS[1][7] = 'NM'; OSNG_GS[2][7] = 'NN'; OSNG_GS[3][7] = 'NO'; OSNG_GS[4][7] = 'NP'; OSNG_GS[5][7] = 'OL'; OSNG_GS[6][7] = 'OM';
   OSNG_GS[0][8] = 'NF'; OSNG_GS[1][8] = 'NG'; OSNG_GS[2][8] = 'NH'; OSNG_GS[3][8] = 'NJ'; OSNG_GS[4][8] = 'NK'; OSNG_GS[5][8] = 'OF'; OSNG_GS[6][8] = 'OG';
   OSNG_GS[0][9] = 'NA'; OSNG_GS[1][9] = 'NB'; OSNG_GS[2][9] = 'NC'; OSNG_GS[3][9] = 'ND'; OSNG_GS[4][9] = 'NE'; OSNG_GS[5][9] = 'OA'; OSNG_GS[6][9] = 'OB';
   OSNG_GS[0][10]= 'HV'; OSNG_GS[1][10]= 'HW'; OSNG_GS[2][10]= 'HX'; OSNG_GS[3][10]= 'HY'; OSNG_GS[4][10]= 'HZ'; OSNG_GS[5][10]= 'JV'; OSNG_GS[6][10]= 'JW';
   OSNG_GS[0][11]= 'HQ'; OSNG_GS[1][11]= 'HR'; OSNG_GS[2][11]= 'HS'; OSNG_GS[3][11]= 'HT'; OSNG_GS[4][11]= 'HU'; OSNG_GS[5][11]= 'JQ'; OSNG_GS[6][11]= 'JR';
   OSNG_GS[0][12]= 'HL'; OSNG_GS[1][12]= 'HM'; OSNG_GS[2][12]= 'HN'; OSNG_GS[3][12]= 'HO'; OSNG_GS[4][12]= 'HP'; OSNG_GS[5][12]= 'JL'; OSNG_GS[6][12]= 'JM';

   // Numeric OSGB Grid Reference 100km Squares
   var OSNG_NS = new Array(7);
   for (i=0; i<OSNG_NS.length; i++) { OSNG_NS[i] = new Array(13); }
   OSNG_NS[0][0] = '00'; OSNG_NS[1][0] = '10'; OSNG_NS[2][0] = '20'; OSNG_NS[3][0] = '30'; OSNG_NS[4][0] = '40'; OSNG_NS[5][0] = '50'; OSNG_NS[6][0] = '60';
   OSNG_NS[0][1] = '01'; OSNG_NS[1][1] = '11'; OSNG_NS[2][1] = '21'; OSNG_NS[3][1] = '31'; OSNG_NS[4][1] = '41'; OSNG_NS[5][1] = '51'; OSNG_NS[6][1] = '61';
   OSNG_NS[0][2] = '02'; OSNG_NS[1][2] = '12'; OSNG_NS[2][2] = '22'; OSNG_NS[3][2] = '32'; OSNG_NS[4][2] = '42'; OSNG_NS[5][2] = '52'; OSNG_NS[6][2] = '62';
   OSNG_NS[0][3] = '03'; OSNG_NS[1][3] = '13'; OSNG_NS[2][3] = '23'; OSNG_NS[3][3] = '33'; OSNG_NS[4][3] = '43'; OSNG_NS[5][3] = '53'; OSNG_NS[6][3] = '63';
   OSNG_NS[0][4] = '04'; OSNG_NS[1][4] = '14'; OSNG_NS[2][4] = '24'; OSNG_NS[3][4] = '34'; OSNG_NS[4][4] = '44'; OSNG_NS[5][4] = '54'; OSNG_NS[6][4] = '64';
   OSNG_NS[0][5] = '05'; OSNG_NS[1][5] = '15'; OSNG_NS[2][5] = '25'; OSNG_NS[3][5] = '35'; OSNG_NS[4][5] = '45'; OSNG_NS[5][5] = '55'; OSNG_NS[6][5] = '65';
   OSNG_NS[0][6] = '06'; OSNG_NS[1][6] = '16'; OSNG_NS[2][6] = '26'; OSNG_NS[3][6] = '36'; OSNG_NS[4][6] = '46'; OSNG_NS[5][6] = '56'; OSNG_NS[6][6] = '66';
   OSNG_NS[0][7] = '07'; OSNG_NS[1][7] = '17'; OSNG_NS[2][7] = '27'; OSNG_NS[3][7] = '37'; OSNG_NS[4][7] = '47'; OSNG_NS[5][7] = '57'; OSNG_NS[6][7] = '67';
   OSNG_NS[0][8] = '08'; OSNG_NS[1][8] = '18'; OSNG_NS[2][8] = '28'; OSNG_NS[3][8] = '38'; OSNG_NS[4][8] = '48'; OSNG_NS[5][8] = '58'; OSNG_NS[6][8] = '68';
   OSNG_NS[0][9] = '09'; OSNG_NS[1][9] = '19'; OSNG_NS[2][9] = '29'; OSNG_NS[3][9] = '39'; OSNG_NS[4][9] = '49'; OSNG_NS[5][9] = '59'; OSNG_NS[6][9] = '69';
   OSNG_NS[0][10]='N00'; OSNG_NS[1][10]='N10'; OSNG_NS[2][10]='N20'; OSNG_NS[3][10]='N30'; OSNG_NS[4][10]='N40'; OSNG_NS[5][10]='N50'; OSNG_NS[6][10]='N60';
   OSNG_NS[0][11]='N01'; OSNG_NS[1][11]='N11'; OSNG_NS[2][11]='N21'; OSNG_NS[3][11]='N31'; OSNG_NS[4][11]='N41'; OSNG_NS[5][11]='N51'; OSNG_NS[6][11]='N61';
   OSNG_NS[0][12]='N02'; OSNG_NS[1][12]='N12'; OSNG_NS[2][12]='N22'; OSNG_NS[3][12]='N32'; OSNG_NS[4][12]='N42'; OSNG_NS[5][12]='N52'; OSNG_NS[6][12]='N62';

   // Lettered War Office Cassini Grid Reference 100km Squares
   var WOCG_GS = new Array(8);
   for (i=0; i<WOCG_GS.length; i++) { WOCG_GS[i] = new Array(13); }
   WOCG_GS[0][0] = 'vV'; WOCG_GS[1][0] = 'vW'; WOCG_GS[2][0] = 'vX'; WOCG_GS[3][0] = 'vY'; WOCG_GS[4][0] = 'vZ'; WOCG_GS[5][0] = 'wV'; WOCG_GS[6][0] = 'wW'; WOCG_GS[7][0] = 'wX';
   WOCG_GS[0][1] = 'vQ'; WOCG_GS[1][1] = 'vR'; WOCG_GS[2][1] = 'vS'; WOCG_GS[3][1] = 'vT'; WOCG_GS[4][1] = 'vU'; WOCG_GS[5][1] = 'wQ'; WOCG_GS[6][1] = 'wR'; WOCG_GS[7][1] = 'wS';
   WOCG_GS[0][2] = 'vL'; WOCG_GS[1][2] = 'vM'; WOCG_GS[2][2] = 'vN'; WOCG_GS[3][2] = 'vO'; WOCG_GS[4][2] = 'vP'; WOCG_GS[5][2] = 'wL'; WOCG_GS[6][2] = 'wM'; WOCG_GS[7][2] = 'wN';
   WOCG_GS[0][3] = 'vF'; WOCG_GS[1][3] = 'vG'; WOCG_GS[2][3] = 'vH'; WOCG_GS[3][3] = 'vJ'; WOCG_GS[4][3] = 'vK'; WOCG_GS[5][3] = 'wF'; WOCG_GS[6][3] = 'wG'; WOCG_GS[7][3] = 'wH';
   WOCG_GS[0][4] = 'vA'; WOCG_GS[1][4] = 'vB'; WOCG_GS[2][4] = 'vC'; WOCG_GS[3][4] = 'vD'; WOCG_GS[4][4] = 'vE'; WOCG_GS[5][4] = 'wA'; WOCG_GS[6][4] = 'wB'; WOCG_GS[7][4] = 'wC';
   WOCG_GS[0][5] = 'qV'; WOCG_GS[1][5] = 'qW'; WOCG_GS[2][5] = 'qX'; WOCG_GS[3][5] = 'qY'; WOCG_GS[4][5] = 'qZ'; WOCG_GS[5][5] = 'rV'; WOCG_GS[6][5] = 'rW'; WOCG_GS[7][5] = 'rX';
   WOCG_GS[0][6] = 'qQ'; WOCG_GS[1][6] = 'qR'; WOCG_GS[2][6] = 'qS'; WOCG_GS[3][6] = 'qT'; WOCG_GS[4][6] = 'qU'; WOCG_GS[5][6] = 'rQ'; WOCG_GS[6][6] = 'rR'; WOCG_GS[7][6] = 'rS';
   WOCG_GS[0][7] = 'qL'; WOCG_GS[1][7] = 'qM'; WOCG_GS[2][7] = 'qN'; WOCG_GS[3][7] = 'qO'; WOCG_GS[4][7] = 'qP'; WOCG_GS[5][7] = 'rL'; WOCG_GS[6][7] = 'rM'; WOCG_GS[7][7] = 'rN';
   WOCG_GS[0][8] = 'qF'; WOCG_GS[1][8] = 'qG'; WOCG_GS[2][8] = 'qH'; WOCG_GS[3][8] = 'qJ'; WOCG_GS[4][8] = 'qK'; WOCG_GS[5][8] = 'rF'; WOCG_GS[6][8] = 'rG'; WOCG_GS[7][8] = 'rH';
   WOCG_GS[0][9] = 'qA'; WOCG_GS[1][9] = 'qB'; WOCG_GS[2][9] = 'qC'; WOCG_GS[3][9] = 'qD'; WOCG_GS[4][9] = 'qE'; WOCG_GS[5][9] = 'rA'; WOCG_GS[6][9] = 'rB'; WOCG_GS[7][9] = 'rC';
   WOCG_GS[0][10]= 'lV'; WOCG_GS[1][10]= 'lW'; WOCG_GS[2][10]= 'lX'; WOCG_GS[3][10]= 'lY'; WOCG_GS[4][10]= 'lZ'; WOCG_GS[5][10]= 'mV'; WOCG_GS[6][10]= 'mW'; WOCG_GS[7][10]= 'mX';
   WOCG_GS[0][11]= 'lQ'; WOCG_GS[1][11]= 'lR'; WOCG_GS[2][11]= 'lS'; WOCG_GS[3][11]= 'lT'; WOCG_GS[4][11]= 'lU'; WOCG_GS[5][11]= 'mQ'; WOCG_GS[6][11]= 'mR'; WOCG_GS[7][11]= 'mS';
   WOCG_GS[0][12]= 'lL'; WOCG_GS[1][12]= 'lM'; WOCG_GS[2][12]= 'lN'; WOCG_GS[3][12]= 'lO'; WOCG_GS[4][12]= 'lP'; WOCG_GS[5][12]= 'mL'; WOCG_GS[6][12]= 'mM'; WOCG_GS[7][11]= 'mN';

   // Lettered War Office Irish Grid Reference 100km Squares
   var WOIG_GS = new Array(4);
   for (i=0; i<WOIG_GS.length; i++) { WOIG_GS[i] = new Array(5); }
   WOIG_GS[0][0] = 'iV'; WOIG_GS[1][0] = 'iW'; WOIG_GS[2][0] = 'iX'; WOIG_GS[3][0] = 'iY';
   WOIG_GS[0][1] = 'iQ'; WOIG_GS[1][1] = 'iR'; WOIG_GS[2][1] = 'iS'; WOIG_GS[3][1] = 'iT';
   WOIG_GS[0][2] = 'iL'; WOIG_GS[1][2] = 'iM'; WOIG_GS[2][2] = 'iN'; WOIG_GS[3][2] = 'iO';
   WOIG_GS[0][3] = 'iF'; WOIG_GS[1][3] = 'iG'; WOIG_GS[2][3] = 'iH'; WOIG_GS[3][3] = 'iJ';
   WOIG_GS[0][4] = 'iA'; WOIG_GS[1][4] = 'iB'; WOIG_GS[2][4] = 'iC'; WOIG_GS[3][4] = 'iD';

   // Lettered OSi Grid Reference 100km Squares
   var OSIG_GS = new Array(4);
   for (i=0; i<OSIG_GS.length; i++) { OSIG_GS[i] = new Array(5); }
   OSIG_GS[0][0] = 'V'; OSIG_GS[1][0] = 'W'; OSIG_GS[2][0] = 'X'; OSIG_GS[3][0] = 'Y';
   OSIG_GS[0][1] = 'Q'; OSIG_GS[1][1] = 'R'; OSIG_GS[2][1] = 'S'; OSIG_GS[3][1] = 'T';
   OSIG_GS[0][2] = 'L'; OSIG_GS[1][2] = 'M'; OSIG_GS[2][2] = 'N'; OSIG_GS[3][2] = 'O';
   OSIG_GS[0][3] = 'F'; OSIG_GS[1][3] = 'G'; OSIG_GS[2][3] = 'H'; OSIG_GS[3][3] = 'J';
   OSIG_GS[0][4] = 'A'; OSIG_GS[1][4] = 'B'; OSIG_GS[2][4] = 'C'; OSIG_GS[3][4] = 'D';

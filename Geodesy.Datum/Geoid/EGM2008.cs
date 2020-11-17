using System;
using System.IO;

namespace Geodesy.Datum.Geoid
{
    public sealed class EGM2008 : GeoidModel
    {
        public EGM2008(ModelType model, string file)
        {
            if (model==ModelType.Geoid_EGM2008_M25)
            {
                Columns = 8640;
                Rows = 4321;

                GridLat = 2.5;
                GridLng = 2.5;
            }
            else if (model==ModelType.Geoid_EGM2008_M10)
            {
                Columns = 21600;
                Rows = 10801;

                GridLat = 1.0;
                GridLng = 1.0;
            }
            else
            {
                throw new Exception("Error model.");
            }

            try
            {
                // try to open the model file
                _reader = new FileStream(file, FileMode.Open);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        ~EGM2008()
        {
            if (_reader != null)
            {
                _reader.Close();
                _reader.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public override int[] GetBoundary(ref double lat, ref double lng)
        {
            int i1, i2, j1, j2;

            double col = (lng - Left) * 60 / GridLng;
            double row = (lat - Top) * 60 / -GridLat;

            i1 = (int)col;
            i2 = i1 < Columns - 1 ? i1 + 1 : 0;
            j1 = (int)row;
            j2 = j1 < Rows - 1 ? j1 + 1 : j1;

            lng = col - i1;
            lat = row - j1;

            return new int[] { i1, i2, j1, j2 };
        }

        public override double[] ReadGrid(int[] position)
        {
            double[] h = new double[4];

            h[0] = ReadFloat(4 * (position[0] + position[2] * (Columns + 2) + 1));
            h[1] = ReadFloat(4 * (position[1] + position[2] * (Columns + 2) + 1));
            h[2] = ReadFloat(4 * (position[0] + position[3] * (Columns + 2) + 1));
            h[3] = ReadFloat(4 * (position[1] + position[3] * (Columns + 2) + 1));

            return h;
        }

        /// <summary>
        /// get 4byte float from file
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private float ReadFloat(int offset)
        {
            byte[] buffer = new byte[4];
            _reader.Position = 0;

            try
            {
                _reader.Read(buffer, offset, buffer.Length);
                return BitConverter.ToSingle(buffer, 0);
            }
            catch
            {
                return 0;
            }
        }
    }
}

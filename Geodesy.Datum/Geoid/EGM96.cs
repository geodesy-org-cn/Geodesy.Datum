using System;
using System.IO;

namespace Geodesy.Datum.Geoid
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EGM96 : GeoidModel
    {
        /// <summary>
        /// Open an EGM96 geoid model
        /// </summary>
        /// <param name="file">model file</param>
        public EGM96(string file)
        {
            Model = ModelType.Geoid_EGM96_M150;
            GridLng = 15.0 / 60;
            GridLat = 15.0 / 60;

            Columns = 1440;
            Rows = 721;

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

        ~EGM96()
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override double[] ReadGrid(int[] position)
        {
            double[] h = new double[4];

            h[0] = ReadShort(2 * (position[0] + position[2] * Columns)) * 0.01;
            h[1] = ReadShort(2 * (position[1] + position[2] * Columns)) * 0.01;
            h[2] = ReadShort(2 * (position[0] + position[3] * Columns)) * 0.01;
            h[3] = ReadShort(2 * (position[1] + position[3] * Columns)) * 0.01;

            return h;
        }

        /// <summary>
        /// get 2 byte signed integer from file
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private short ReadShort(int offset)
        {
            byte[] buffer = new byte[2];
            _reader.Position = 0;

            try
            {
                _reader.Read(buffer, offset, buffer.Length);
                return BitConverter.ToInt16(buffer, 0);
            }
            catch
            {
                return 0;
            }
        }
    }
}

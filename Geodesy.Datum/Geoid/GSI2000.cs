using System;
using System.IO;

namespace Geodesy.Datum.Geoid
{
    public sealed class GSI2000 : GeoidModel
    {
        public GSI2000(string file)
        {
            Model = ModelType.Geoid_GSI2000_M15;

            Columns = 1201;
            Rows = 1801;
            GridLat = 1.0;
            GridLng = 1.5;

            Left = 120.0;
            Right = 150.0;
            Top = 50.0;
            Bottom = 20.0;

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

        ~GSI2000()
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
            double row = (lat - Bottom) * 60 / GridLat;

            i1 = (int)col;
            i2 = i1 < Columns - 1 ? i1 + 1 : i1;
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

            h[0] = ReadDouble(position[0], position[2]);
            h[1] = ReadDouble(position[1], position[2]);
            h[2] = ReadDouble(position[0], position[3]);
            h[3] = ReadDouble(position[1], position[3]);

            return h;
        }

        /// <summary>
        /// Read GSI geoid data
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private double ReadDouble(int i, int j)
        {
            int nf = 28, wf = 9, nl = nf * wf + 2;
            int nr = (Columns - 1) / nf + 1;
            int offset = nl + j * nr * nl + i / nf * nl + i % nf * wf;

            byte[] buffer = new byte[wf];
            _reader.Position = 0;

            try
            {
                _reader.Read(buffer, offset, buffer.Length);
                return BitConverter.ToDouble(buffer, 0);
            }
            catch
            {
                return 0;
            }
        }
    }
}
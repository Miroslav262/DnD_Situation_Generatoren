namespace dndsitgen.Utils
{
    public class CRStandardizer
    {
        private static float[] standartCR = {0.0f, 0.125f, 0.25f, 0.5f, 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f,
        9.0f, 10.0f, 11.0f, 12.0f, 13.0f, 14.0f, 15.0f, 16.0f, 17.0f, 18.0f, 19.0f, 20.0f, 21.0f, 22.0f, 23.0f, 24.0f,
        25.0f, 26.0f, 27.0f, 28.0f, 29.0f, 30.0f};
        private static float Closest(float x)
        {
            int idx = Array.BinarySearch(standartCR, x);

            if (idx >= 0)
                return standartCR[idx];

            idx = ~idx;

            if (idx == 0)
                return standartCR[0];
            if (idx == standartCR.Length)
                return standartCR[^1];

            float lower = standartCR[idx - 1];
            float upper = standartCR[idx];

            return Math.Abs(x - lower) < Math.Abs(x - upper) ? lower : upper;
        }

        public static float[] toStandart(float[] rawCR)
        {
            float[] result = new float[rawCR.Length];
            for (int i = 0; i < rawCR.Length; i++)
                result[i] = Closest(rawCR[i]);
            return result;
        }

    }
}

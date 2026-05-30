using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dndsitgen.Serveces.Scenaries;

namespace dndsitgen.Serveces
{
    public class CreatureCalculatorService
    {
        private int getIMeaneangful(float[] cr, int[] k)
        {

            if (cr.Length != k.Length) return -1;

            float[] presoftmax = new float[cr.Length];
            for (int i = 0; i < presoftmax.Length; i++)
            {

                presoftmax[i] = cr[i] * f(k[i]);
            }

            float[] postsoftmax = softmax(presoftmax);

            int i_meaneangful = 0;
            float m = (float)Math.Sqrt(M(cr, k, postsoftmax));

            for (int i = 0; i < postsoftmax.Length; i++)
            {
                if (presoftmax[i] >= m)
                {

                    i_meaneangful++;
                }
            }
            return i_meaneangful;
        }
        private float M(float[] cr, int[] k, float[] w)
        {

            float result = 0;

            if (cr.Length != k.Length || cr.Length != w.Length) return -1;

            for (int i = 0; i < cr.Length; i++)
            {
                result += (cr[i] * f(k[i]) * w[i]);
            }
            return result;
        }
        private float getPrimaryComplexity(float[] cr, int[] k)
        {
            float result = 0;

            if (cr.Length != k.Length) return -1;

            for (int i = 0; i < cr.Length; i++)
            {
                result += (cr[i] * k[i]);
            }
            return result;
        }
        private float[] softmax(float[] array)
        {
            float max = array.Max();

            float[] exp = new float[array.Length];
            float sum = 0;

            for (int i = 0; i < array.Length; i++)
            {
                exp[i] = (float)Math.Exp(array[i] - max);
                sum += exp[i];
            }

            float[] result = new float[array.Length];
            for (int i = 0; i < exp.Length; i++)
            {
                result[i] = exp[i] / sum;
            }

            return result;
        }

        private float f(int k_i)
        {
            // return (float)Math.Sqrt(k_i);
            return k_i;
        }

        public float getComplexity(float[] cr, int[] k)
        {
            int i = getIMeaneangful(cr, k);
            float primaryComplexity = getPrimaryComplexity(cr, k);

            return (float)Math.Sqrt(i) * primaryComplexity;
        }


        private float[] buildCRs(float c, int[] k, Scenary scenary)
        {
            float[] cr = new float[k.Length];
            for (int i = 0; i < k.Length; i++)
                cr[i] = c * scenary.g(i);
            return cr;
        }

        private float SolveC(float T_M_i, int[] k, Scenary scenary)
        {
            float left = 0f;
            float right = 1000f;
            const float eps = 0.01f;

            for (int iter = 0; iter < 50; iter++)
            {
                float mid = (left + right) * 0.5f;

                float[] cr = buildCRs(mid, k, scenary);
                float complexity = getComplexity(cr, k);

                if (Math.Abs(complexity - T_M_i) < eps)
                    return mid;

                if (complexity < T_M_i)
                    left = mid;
                else
                    right = mid;
            }

            return (left + right) * 0.5f;
        }

        public float[] getRawCRs(float T_M_i, int[] k, Scenary scenary)
        {
            float c = SolveC(T_M_i, k, scenary);
            float[] cr = buildCRs(c, k, scenary);

            var pairs = k.Zip(cr, (kk, cc) => new { k = kk, cr = cc })
                         .OrderBy(p => p.k)
                         .ToArray();

            for (int i = 0; i < k.Length; i++)
            {
                k[i] = pairs[i].k;
                cr[i] = pairs[i].cr;
            }

            return cr;
        }


        public float getHeroesComplexity(float[] cr, int[] k)
        {
            return getComplexity(cr, k);
        }


    }
}

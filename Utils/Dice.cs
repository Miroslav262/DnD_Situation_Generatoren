namespace dndsitgen.Utils
{
    public class Dice
    {
        private static Random random = new Random();
        public static int Roll(int count, int dice) {
            int result = 0;
            for (int i = 0; i<count; i++) {
                result += (random.Next() % dice + 1);
            }
            return result;
        }
    }
}

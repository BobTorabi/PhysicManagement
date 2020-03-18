using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Common
{
    public class FileID
    {
        private static Random random = new Random();
        public static string NewID()
        {
            var Now = DateTime.Now;
            return $"{Now.Year}{Now.Month}{Guid.NewGuid().ToString().Replace("-", "")}"; ;
        }
        public static int ID(short len)
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            int Min = int.Parse("1" + new string('0', len - 1));
            int Max = int.Parse(new string('9', len));
            System.Threading.Thread.Sleep(1);
            return rnd.Next(Min, Max);
        }
        public static string Scramble(string input)
        {
            char[] chars = input.ToArray();
            Random r = new Random(259);
            for (int i = 0; i < chars.Length; i++)
            {
                int randomIndex = r.Next(0, chars.Length);
                char temp = chars[randomIndex];
                chars[randomIndex] = chars[i];
                chars[i] = temp;
            }
            return new string(chars);
        }
        public static string UnScramble(string scrambled)
        {
            Random r = new Random(259);
            char[] scramChars = scrambled.ToArray();
            List<int> swaps = new List<int>();
            for (int i = 0; i < scramChars.Length; i++)
            {
                swaps.Add(r.Next(0, scramChars.Length));
            }
            for (int i = scramChars.Length - 1; i >= 0; i--)
            {
                char temp = scramChars[swaps[i]];
                scramChars[swaps[i]] = scramChars[i];
                scramChars[i] = temp;
            }

            return new string(scramChars);
        }
        public static List<int> GetUniqueNumber(int count, int min, int max)
        {
            //  initialize set S to empty
            //  for J := N-M + 1 to N do
            //    T := RandInt(1, J)
            //    if T is not in S then
            //      insert T in S
            //    else
            //      insert J in S
            //
            // adapted for C# which does not have an inclusive Next(..)
            // and to make it from configurable range not just 1.

            if (max <= min || count < 0 ||
                    // max - min > 0 required to avoid overflow
                    (count > max - min && max - min > 0))
            {
                // need to use 64-bit to support big ranges (negative min, positive max)
                throw new ArgumentOutOfRangeException("Range " + min + " to " + max +
                        " (" + ((Int64)max - (Int64)min) + " values), or count " + count + " is illegal");
            }

            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();

            // start count values before max, and end at max
            for (int top = max - count; top < max; top++)
            {
                // May strike a duplicate.
                // Need to add +1 to make inclusive generator
                // +1 is safe even for MaxVal max value because top < max
                if (!candidates.Add(random.Next(min, top + 1)))
                {
                    // collision, add inclusive max.
                    // which could not possibly have been added before.
                    candidates.Add(top);
                }
            }

            // load them in to a list, to sort
            List<int> result = candidates.ToList();

            // shuffle the results because HashSet has messed
            // with the order, and the algorithm does not produce
            // random-ordered results (e.g. max-1 will never be the first value)
            for (int i = result.Count - 1; i > 0; i--)
            {
                int k = random.Next(i + 1);
                int tmp = result[k];
                result[k] = result[i];
                result[i] = tmp;
            }
            return result;
        }
    }
}

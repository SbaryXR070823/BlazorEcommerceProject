using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    public static class StringSearch
    {
        public static bool KMPContains(string text, string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return true;
            if (string.IsNullOrEmpty(text)) return false;

            int[] lps = ComputeLPSArray(pattern);
            int i = 0, j = 0;

            while (i < text.Length)
            {
                if (char.ToLower(text[i]) == char.ToLower(pattern[j])) // Case-insensitive match
                {
                    i++;
                    j++;
                    if (j == pattern.Length) return true; // Full match found
                }
                else
                {
                    if (j != 0) j = lps[j - 1];
                    else i++;
                }
            }
            return false;
        }

        private static int[] ComputeLPSArray(string pattern)
        {
            int length = 0, i = 1;
            int[] lps = new int[pattern.Length];
            lps[0] = 0;

            while (i < pattern.Length)
            {
                if (pattern[i] == pattern[length])
                {
                    length++;
                    lps[i] = length;
                    i++;
                }
                else
                {
                    if (length != 0) length = lps[length - 1];
                    else
                    {
                        lps[i] = 0;
                        i++;
                    }
                }
            }
            return lps;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCRevitRibbonUtil
{
    public class LineBreaks
    {
        private static readonly int max = 20;
        private static readonly int min = 3;

        public static string Format(string soruceString)
        {
            if (soruceString.Length < max)
                return soruceString;

            List<string> lines = new List<string>();

            var words = soruceString.Split(' ');
            StringBuilder line = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
            {
                // Break line if with new word it will be longer than MAX
                bool doBrake = false;
                if (line.Length > 0)
                {
                    if ((line.Length + words[i].Length) > max)
                    {
                        // if word is the last and it is shorter than MIN - do nothing
                        if (!(i == words.Length - 1 && words[i].Length <= min))
                        {
                            doBrake = true;
                        }
                    }
                    // if the word is not the last, but it is an article for the over next one
                    else if (i != words.Length - 1 && words[i].Length <= min && line.Length + words[i].Length + words[i + 1].Length + 1 > max)
                    {
                        doBrake = true;
                    }

                    if (doBrake)
                    {
                        lines.Add(line.ToString());
                        line.Clear();
                    }
                }
                if (line.Length > 0)
                    line.Append(" ");
                line.Append(words[i]);
            }
            if (line.Length > 0)
            {
                lines.Add(line.ToString());
                line.Clear();
            }
            return String.Join("\n", lines);
        }
    }
}
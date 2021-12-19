using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Models.Torrents
{
    public enum PiracyAlgorithm
    {
        RATIO,
        SEEDERS,
        LEECHERS,
        FORMULA
    }

    static class PiracyAlgorithmConverter
    {
        static Dictionary<PiracyAlgorithm, string> algorithms = new Dictionary<PiracyAlgorithm, string>
        {
            {PiracyAlgorithm.RATIO, "ratio" },
            {PiracyAlgorithm.SEEDERS, "seeders" },
            {PiracyAlgorithm.LEECHERS, "leechers" },
            {PiracyAlgorithm.FORMULA, "formula" },
        };

        public static string GetText(PiracyAlgorithm type) => algorithms[type];

        public static PiracyAlgorithm GetType(string type) =>
            algorithms.FirstOrDefault(algorithm => algorithm.Value == type).Key;
    }
}

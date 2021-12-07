using System;
using System.Collections.Generic;

namespace GgCli.ValueParsers
{
    public class SemVersion
    {
        private SemVersion(
            int major,
            int minor,
            int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        private int Major { get; }
        private int Minor { get; }
        private int Patch { get; }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }

        public static SemVersion From(
            string version)
        {
            var versions = version.Split(
                ".",
                StringSplitOptions.RemoveEmptyEntries);
            if (versions.Length != 3) throw new BusinessException("Version is not in the form of semantic versions and not supported.");

            List<int> values = new();
            foreach (var ver in versions)
            {
                if (!int.TryParse(
                    ver,
                    out var value))
                    throw new BusinessException("All three semantic version parts must be numbers.");

                values.Add(value);
            }

            return new SemVersion(
                values[0],
                values[1],
                values[2]);
        }
    }
}
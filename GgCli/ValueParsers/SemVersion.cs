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
            var versionParts = version.Split(
                ".",
                StringSplitOptions.RemoveEmptyEntries);
            if (versionParts.Length != 3) throw new BusinessException("Provided version must be in the semantic version format");

            List<int> parsedVersionParts = new();
            foreach (var versionPart in versionParts)
            {
                if (!int.TryParse(
                    versionPart,
                    out var parsedVersionPart))
                    throw new BusinessException("All three semantic version parts must be numbers.");

                parsedVersionParts.Add(parsedVersionPart);
            }

            return new SemVersion(
                parsedVersionParts[0],
                parsedVersionParts[1],
                parsedVersionParts[2]);
        }
    }
}
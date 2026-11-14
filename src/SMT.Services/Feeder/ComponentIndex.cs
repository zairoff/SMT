using SMT.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SMT.Services.Feeder
{
    public enum ComponentMatch
    {
        None = 0,
        RCode = 1,
        PrimaryPartNumber = 2,
        AlternatePartNumber = 3,
        // The code is a part number of more than one component - can't tell which one is meant.
        Ambiguous = 4,
    }

    public class ComponentLookup
    {
        public Component Component { get; set; }

        public ComponentMatch Match { get; set; }

        public string MatchedCode { get; set; }
    }

    // In-memory lookup over a set of components by RCode and every part number (alternates included).
    // The repository's GetByPartNumberAsync only sees the primary part number, which isn't enough to
    // resolve codes copied from the machine program.
    public class ComponentIndex
    {
        private readonly Dictionary<string, Component> _byRCode = new Dictionary<string, Component>();
        private readonly Dictionary<string, List<(Component Component, bool IsPrimary)>> _byPartNumber =
            new Dictionary<string, List<(Component, bool)>>();

        public ComponentIndex(IEnumerable<Component> components)
        {
            foreach (var component in components)
            {
                var rcode = FeederCodes.NormalizeCode(component.RCode);
                if (rcode.Length > 0 && !_byRCode.ContainsKey(rcode))
                    _byRCode[rcode] = component;

                var partNumbers = component.PartNumber ?? new List<string>();
                for (var i = 0; i < partNumbers.Count; i++)
                {
                    var partNumber = FeederCodes.NormalizeCode(partNumbers[i]);
                    if (partNumber.Length == 0)
                        continue;

                    if (!_byPartNumber.TryGetValue(partNumber, out var matches))
                        _byPartNumber[partNumber] = matches = new List<(Component, bool)>();

                    if (matches.All(m => m.Component.Id != component.Id))
                        matches.Add((component, i == 0));
                }
            }
        }

        // Tries each candidate as an RCode first, then as a part number; the first hit wins.
        public ComponentLookup Resolve(IEnumerable<string> candidates)
        {
            ComponentLookup ambiguous = null;

            foreach (var candidate in candidates)
            {
                var code = FeederCodes.NormalizeCode(candidate);
                if (code.Length == 0)
                    continue;

                if (_byRCode.TryGetValue(code, out var byRCode))
                    return new ComponentLookup { Component = byRCode, Match = ComponentMatch.RCode, MatchedCode = code };

                if (_byPartNumber.TryGetValue(code, out var matches))
                {
                    if (matches.Count == 1)
                    {
                        return new ComponentLookup
                        {
                            Component = matches[0].Component,
                            Match = matches[0].IsPrimary ? ComponentMatch.PrimaryPartNumber : ComponentMatch.AlternatePartNumber,
                            MatchedCode = code,
                        };
                    }

                    ambiguous ??= new ComponentLookup { Match = ComponentMatch.Ambiguous, MatchedCode = code };
                }
            }

            return ambiguous ?? new ComponentLookup { Match = ComponentMatch.None };
        }

        public ComponentLookup Resolve(string code)
        {
            return Resolve(new[] { code });
        }
    }
}

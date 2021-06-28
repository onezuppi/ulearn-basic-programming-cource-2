using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree
{
    public static class DiskTreeTask
    {
        public static List<string> Solve(List<string> input) =>
            new Directory(input).GetFormattedTree(new List<string>(), -1);
    }

    public class Directory
    {
        private readonly string name;

        private readonly SortedDictionary<string, Directory> directories =
            new SortedDictionary<string, Directory>(StringComparer.Ordinal);

        public Directory(List<string> directories)
        {
            foreach (var directory in directories)
            {
                var node = this;
                directory.Split('\\').Aggregate(node, (current, item) =>
                    current.MakeSubDirectory(item));
            }
        }

        private Directory(string name)
        {
            this.name = name;
        }

        public List<string> GetFormattedTree(List<string> list, int indent)
        {
            if (indent >= 0)
                list.Add($"{new string(' ', indent)}{name}");
            return directories.Values.Aggregate(list, (current, child) =>
                child.GetFormattedTree(current, indent + 1));
        }

        private Directory MakeSubDirectory(string subDirectory) =>
            directories.ContainsKey(subDirectory)
                ? directories[subDirectory]
                : directories[subDirectory] = new Directory(subDirectory);
    }
}
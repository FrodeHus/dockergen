using DockerGen.Container;
using System.Text;

namespace DockerGen.Infrastructure
{
    public static class StringExtensions
    {
        public static string[] SplitOnInstructions(this string value)
        {
            var values = value.Split('\n');

            var builder = new StringBuilder();
            foreach (var line in values)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line.IndexOf(' ') != -1)
                {
                    var prefix = line.Substring(0, line.IndexOf(' '));
                    if (ContainerService.GetValidPrefixes().Contains(prefix))
                    {
                        builder.Append('$');
                        builder.Append(line);
                    }
                    else
                    {
                        builder.AppendLine(line);
                    }
                }
            }
            return builder.ToString().Split('$', System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}

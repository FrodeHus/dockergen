using DockerGen.Container;
using System.Linq;
using System.Text;

namespace DockerGen.Helpers
{
    public static class StringExtensions
    {
        private static readonly string[] _allowedInstructions = new string[]
        {
            "ADD","FROM","CMD","ENV","ARG","EXPOSE","RUN","ENTRYPOINT","HEALTHCHECK","COPY","USER","WORKDIR","LABEL","MAINTAINER","VOLUME","ONBUILD","STOPSIGNAL","SHELL"
        };
        public static string[] SplitOnInstructions(this string value)
        {
            var values = value.Split('\n');

            var builder = new StringBuilder();
            foreach (var line in values)
            {
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                {
                    continue;
                }

                if (line.IndexOf(' ') != -1)
                {
                    var prefix = line[..line.IndexOf(' ')];
                    if (_allowedInstructions.Contains(prefix.ToUpper()))
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

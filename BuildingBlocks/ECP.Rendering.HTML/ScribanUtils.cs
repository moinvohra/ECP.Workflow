using Scriban;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECP.Rendering.HTML
{
    internal class ScribanUtils
    {
        public static async Task<string> Render(string templateStr, object obj = null)
        {
            var template = Template.Parse(templateStr);
            if (template.HasErrors)
                throw new Exception(string.Join("\n", template.Messages.Select(x => $"{x.Message} at {x.Span.ToStringSimple()}")));
            return await template.RenderAsync(obj, member => LowerFirstCharacter(member.Name));
        }
        private static string LowerFirstCharacter(string value)
        {
            if (value.Length > 1)
            {
                return char.ToLower(value[0]) + value.Substring(1);
            }
            return value;
        }
    }
}

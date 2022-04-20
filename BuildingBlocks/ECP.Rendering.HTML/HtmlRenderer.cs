using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Dynamic;
using System.Threading.Tasks;

namespace ECP.Rendering.HTML
{
    public class HtmlRenderer : IHtmlRenderer
    {
        public async Task<string> GenrateHtmlAsync(HtmlParameters htmlParameters)
        {
            string generatedOutput = htmlParameters.templateContents;
            if (!string.IsNullOrEmpty(htmlParameters.templatePlaceholderValues) && !string.IsNullOrEmpty(htmlParameters.templateContents))
            {
                generatedOutput = ReplaceTemplatePlaceholders(htmlParameters.templateContents,htmlParameters.templatePlaceholderValues);
            }

            // bind json list in html content
            if (!string.IsNullOrEmpty(generatedOutput) && !string.IsNullOrEmpty(Convert.ToString(htmlParameters.tableRecords)))
            {
                object records = JsonConvert.DeserializeObject<ExpandoObject>(Convert.ToString(htmlParameters.tableRecords));
                generatedOutput = await ScribanUtils.Render(generatedOutput, new { records });
            }
            return generatedOutput;
        }

        private string ReplaceTemplatePlaceholders(string templateContent, string templatePlaceholderValues)
        {
            try
            {
                foreach (var pair in JObject.Parse(templatePlaceholderValues))
                {
                    templateContent = templateContent.Replace(pair.Key.ToString(), pair.Value.ToString());
                }
                return templateContent;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}

using System.Threading.Tasks;

namespace ECP.Rendering.HTML
{
    public interface IHtmlRenderer
    {
        Task<string> GenrateHtmlAsync(HtmlParameters htmlParameters);
    }
}

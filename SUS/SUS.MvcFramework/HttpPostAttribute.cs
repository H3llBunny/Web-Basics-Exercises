
namespace SUS.MvcFramework
{
    public class HttpPostAttribute : BaseHttpAttribute
    {
        public HttpPostAttribute()
        {
        }

        public HttpPostAttribute(string url)
        {
            this.Url = url;
        }

        public override HTTP.HttpMethod Method => HTTP.HttpMethod.Post;
    }
}

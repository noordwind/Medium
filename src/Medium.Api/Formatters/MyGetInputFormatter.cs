using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;

namespace Medium.Api.Formatters
{
    public class MyGetInputFormatter : InputFormatter
    {
        public MyGetInputFormatter()
        {
            SupportedMediaTypes.Add("application/vnd.myget.webhooks.v1+json");
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            using(var reader = new StreamReader(context.HttpContext.Request.Body))
            {
                var request = reader.ReadToEnd();

                return await InputFormatterResult.SuccessAsync(JsonConvert.DeserializeObject(request));
            }
        }
    }
}
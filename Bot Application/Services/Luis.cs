using Bot_Application.Serialization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Application.Services
{
    public class Luis
    {
        public static async Task<Utterance> GetResponse(string message)
        {
            using (var client = new HttpClient())
            {
                const string authKey = "7b43f732a37f46f1a8b9322eed920ba3"; // "8f0dd8e1728b4ae6b79323a1a3c14292";

                var url = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/78e1b8a4-c4cd-4a7c-a02b-68ed8f6d0fb7?subscription-key={authKey}&q={message}";
               /* var url = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/b0ec4e41-364d-448a-9412-f8ef00d9333d?subscription-key={authKey}&q={message}"; */
                /*var url = $"https://api.projectoxford.ai/luis/v1/application?id=3310fdde-a8ad-43a4-824a-9577f91c2fb8&subscription-key={authKey}&q={message}";
                https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/de8abd60-88ca-4559-8e60-faa5ef96ad14?subscription-key=8f0dd8e1728b4ae6b79323a1a3c14292&verbose=true&timezoneOffset=0&q= */
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode) return null;
                var result = await response.Content.ReadAsStringAsync();

                var js = new DataContractJsonSerializer(typeof(Utterance));
                var ms = new MemoryStream(Encoding.ASCII.GetBytes(result));
                var list = (Utterance)js.ReadObject(ms);

                return list;
            }
        }
    }
}
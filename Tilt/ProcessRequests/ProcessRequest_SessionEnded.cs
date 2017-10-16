using Alexa.NET;
using Alexa.NET.Response;
using System.Threading.Tasks;

namespace Tilt
{
    public static partial class ProcessRequest
    {
        /// <summary>
        /// Closes out the session
        /// </summary>
        /// <returns>Bunch of words</returns>
        public static async Task<SkillResponse> SessionEnded()
        {
            await Utils.Log("SessionEnded request made");

            var speech = new PlainTextOutputSpeech();
            speech.Text = "Okay cheerio";

            var response = ResponseBuilder.Tell(speech);

            return response;
        }
    }
}

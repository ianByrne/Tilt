using Alexa.NET;
using Alexa.NET.Response;
using System.Threading.Tasks;

namespace Tilt
{
    public static partial class ProcessRequest
    {
        /// <summary>
        /// Informs the user of an error
        /// </summary>
        /// <returns>Bunch of words</returns>
        public static async Task<SkillResponse> Error()
        {
            await Utils.Log("Error request made");

            var speech = new PlainTextOutputSpeech();
            speech.Text = "Sorry, I have no idea what is going on, so am just going to go ahead and exit now.";

            var response = ResponseBuilder.Tell(speech);

            return response;
        }
    }
}

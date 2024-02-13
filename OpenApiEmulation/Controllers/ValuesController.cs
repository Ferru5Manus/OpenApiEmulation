using Microsoft.AspNetCore.Mvc;
using OpenApiEmulation.Controllers.Models;
using OpenApiEmulation.Utils.AntiCsrf;


namespace OpenApiEmulation.Controllers
{
    [Route("/VtbEmulation")]
    [ApiController]
    public class AuthEmulation : ControllerBase
    {
        [HttpGet]
        [Route("AuthEmulation")]
        public async Task EmulateWeb2Web([FromQuery] Web2Webauthmodel model)
        {
           CodeGenerator generator = new CodeGenerator();
           AntiCsrfGenerator geb = new AntiCsrfGenerator();
            HttpContext.Response.Redirect(model.redirect_uri+"?code="+generator.GenerateCode()+ "&state="+ await geb.GenerateToken(model.client_id,model.scope));
        }
    }
    //TODO Implement Required methods required by the app.
    //[Route()]
    public class CodeGenerator
    {
        private const int NumberOfLettersOrNumbers = 9;

        private string[] letters = new string[27] { "A", "B", "C", "D", "E", "F", "G", "H", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private string[] numbers = new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        private static Random r = new Random();

        public string GenerateCode()
        {
            string[] randomLetters = new string[NumberOfLettersOrNumbers];
            string[] randomNumbers = new string[NumberOfLettersOrNumbers];

            for (int i = 0; i < NumberOfLettersOrNumbers; i++)
            {
                randomLetters[i] = letters[r.Next(0, letters.Length - 1)];
                randomNumbers[i] = numbers[r.Next(0, numbers.Length - 1)];
            }

            return String.Format("{0}{1}{2}{3}{15}{13}{8}{5}-{4}{5}{6}{7}-{8}{9}{10}{11}-{12}{13}{14}{15}-{13}{8}{9}{10}{11}{5}{1}{14}{11}{3}{6}{7}", randomNumbers[0], randomLetters[1], randomNumbers[1], randomLetters[2], randomNumbers[2], randomLetters[3], randomNumbers[3], randomLetters[4], randomNumbers[4], randomLetters[5], randomNumbers[5], randomLetters[6], randomNumbers[6], randomLetters[7], randomNumbers[7], randomLetters[8], randomNumbers[8]);
        }

       
    }
}

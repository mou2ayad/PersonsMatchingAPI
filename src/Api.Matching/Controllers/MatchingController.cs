using System.Threading.Tasks;
using Api.Matching.Models;
using Component.Matching.Contracts;
using Component.Matching.Models;
using Component.Utilities.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Matching.Controllers
{

    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService<Person> _matchingService;

        public MatchingController(IMatchingService<Person> matchingService) => 
            _matchingService = matchingService;


        /// <param name="request">request contains First Person and last Person</param>
        /// <returns>Matching Score</returns>
        [HttpPost("api/v1/persons/Match")]
        [SwaggerOperation(
            Summary = "Matching two Persons",
            Description = "Match two persons based on the Matching rules and return matching score",
            OperationId = "MatchingPersonsEndpoint")]
        [ProducesResponseType(typeof(ExceptionDetails), 400)]
        [ProducesResponseType(typeof(ExceptionDetails), 500)]
        public async Task<ActionResult<MatchingResponse>> Match([FromBody] MatchingRequest request)
        {
            ValidateMatchRequest(request);
            var result = await _matchingService.Match(request.First,request.Second);
            return MatchingResponse.From(result);
        }

        private static void ValidateMatchRequest(MatchingRequest request)
        {
            if (request == null || request.First == null || request.Second == null)
                throw new InvalidRequestException("Both First and Second Person can't be null");
            if (string.IsNullOrEmpty(request.First.FirstName) 
                || string.IsNullOrEmpty(request.First.LastName) 
                || string.IsNullOrEmpty(request.Second.FirstName) 
                || string.IsNullOrEmpty(request.Second.LastName))
                throw new InvalidRequestException("FirstName and Last Name can't not be null of empty");
        }
    }
}

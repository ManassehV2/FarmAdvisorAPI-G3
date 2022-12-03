using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Linq;
using FarmAdvisor.Models.Models;

namespace FarmAdvisor.HttpFunctions.Functions
{
    public class FarmFieldFunctions
    {
        private readonly ILogger<FarmFieldFunctions> _logger;

        public FarmFieldFunctions(ILogger<FarmFieldFunctions> logger)
        {
            _logger = logger;
        }
        [FunctionName("GetAllFarmFields")]
        [OpenApiOperation(operationId: "GetAllFarmFields", tags: new[] { "FarmFields" }, Summary = "Gets all fields in a farm.", Description = "Gets all fields in a farm.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "farmId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The fields farm Id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<FarmFieldModel>), Description = "List of fields in a farm")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        public async Task<IActionResult> GetAllFarmFields(
             [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/farms/fields")] HttpRequest req)
        {
            _logger.LogInformation("Executing {method}", nameof(GetAllFarmFields));
            string farmId = req.Query["farmId"];

            if (Guid.TryParse(farmId, out Guid id))
            {
                var result = new FarmFieldModel();
                return new OkObjectResult(result);
            }

            return new BadRequestObjectResult("Invalid Farm Id");
        }


    }
}
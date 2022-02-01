using route = SimpleAPIGateway.Models.Route;

namespace SimpleAPIGateway
{
    /// <summary>
    /// Custom Router
    /// </summary>
    public class Router
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Router"/> class.
        /// </summary>
        /// <param name="routeConfigFilePath">The route configuration file path.</param>
        public Router(string routeConfigFilePath)
        {
            dynamic router = RouteLoader.LoadFromFile<dynamic>(routeConfigFilePath);

            Routes = RouteLoader.Deserialize<List<route>>(Convert.ToString(router.routes));
            AuthenticationService = RouteLoader.Deserialize<Destination>(Convert.ToString(router.authenticationService));
        }

        /// <summary>
        /// Gets or sets the authentication service.
        /// </summary>
        /// <value>
        /// The authentication service.
        /// </value>
        public Destination AuthenticationService { get; set; }

        /// <summary>
        /// Gets or sets the routes.
        /// </summary>
        /// <value>
        /// The routes.
        /// </value>
        public List<route> Routes { get; set; }

        /// <summary>
        /// Routes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> RouteRequest(HttpRequest request)
        {
            string path = request.Path.ToString();
            string basePath = '/' + path.Split('/')[1];

            Destination destination;
            try
            {
                destination = Routes.First(r => r.Endpoint.Equals(basePath)).Destination;
            }
            catch
            {
                return ConstructErrorMessage("The path could not be found.");
            }

            if (destination.RequiresAuthentication)
            {
                string token = request.Headers["token"];
                request.Query.Append(new KeyValuePair<string, StringValues>("token", new StringValues(token)));
                HttpResponseMessage authResponse = await AuthenticationService.SendRequest(request);
                if (!authResponse.IsSuccessStatusCode) return ConstructErrorMessage("Authentication failed.");
            }

            return await destination.SendRequest(request);
        }

        /// <summary>
        /// Constructs the error message.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        private HttpResponseMessage ConstructErrorMessage(string error)
        {
            HttpResponseMessage errorMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(error)
            };
            return errorMessage;
        }
    }
}
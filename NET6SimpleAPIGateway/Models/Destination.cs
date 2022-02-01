namespace SimpleAPIGateway.Models
{
    /// <summary>
    /// Destination
    /// </summary>
    public class Destination
    {
        /// <summary>
        /// The client
        /// </summary>
        private static HttpClient client = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="Destination"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="requiresAuthentication">if set to <c>true</c> the authentication is required.</param>
        public Destination(string uri, bool requiresAuthentication)
        {
            Path = uri;
            RequiresAuthentication = requiresAuthentication;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Destination"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public Destination(string path)
            : this(path, false)
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Destination"/> class from being created.
        /// </summary>
        private Destination()
        {
            Path = "/";
            RequiresAuthentication = false;
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the authentication is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the authentication is required; otherwise, <c>false</c>.
        /// </value>
        public bool RequiresAuthentication { get; set; }

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendRequest(HttpRequest request)
        {
            string requestContent;

            using (Stream receiveStream = request.Body)
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    requestContent = readStream.ReadToEnd();

            using (var newRequest = new HttpRequestMessage(new HttpMethod(request.Method), CreateDestinationUri(request)))
            {
                newRequest.Content = new StringContent(requestContent, Encoding.UTF8, request.ContentType);
                using (var response = await client.SendAsync(newRequest))
                {
                    return response;
                }
            }
        }

        /// <summary>
        /// Creates the destination URI.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private string CreateDestinationUri(HttpRequest request)
        {
            string requestPath = request.Path.ToString();
            string queryString = request.QueryString.ToString();

            string endpoint = "";
            string[] endpointSplit = requestPath.Substring(1).Split('/');

            if (endpointSplit.Length > 1)
                endpoint = endpointSplit[1];

            return Path + endpoint + queryString;
        }
    }
}
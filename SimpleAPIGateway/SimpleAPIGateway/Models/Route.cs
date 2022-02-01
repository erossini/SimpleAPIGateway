namespace SimpleAPIGateway.Models
{
    /// <summary>
    /// Route
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        /// <value>
        /// The endpoint.
        /// </value>
        public string Endpoint { get; set; }
        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        public Destination Destination { get; set; }
    }
}
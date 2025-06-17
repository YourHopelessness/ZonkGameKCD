using ZonkGame.DB.Enum;

namespace ZonkGameCore.ApiUtils.Requests
{
    /// <summary>
    /// Parameters for a request for access rights
    /// </summary>
    public class HasAccessRequest
    {
        /// <summary>API resource</summary>
        public ApiEnumRoute Api { get; set; }

        /// <summary>ID resource</summary>
        public Guid? ResourceId { get; set; }

        /// <summary>The path to the method</summary>
        public string? ResourceRoute { get; set; }

        /// <summary>User ID</summary>
        public Guid? UserId { get; set; }

        /// <summary>User name</summary>
        public string? UserName { get; set; }
    }
}

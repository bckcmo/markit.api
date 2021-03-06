namespace Markit.Api.Models.Statics
{
    public static class ErrorMessages
    {
        public static string UserDenied = "The resource does not belong to this user.";
        public static string MissingPrivileges = "The user must be a Super User to preform this action.";
        public static string ResourceNotFound = "The resource was not found.";
        public static string PriceAnalysisIncomplete = "Not enough price data to complete the analysis.";
    }
}
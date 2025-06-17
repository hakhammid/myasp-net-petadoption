namespace petmanagement.Constants
{
    public static class AppConstants
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }

        public static class AdoptionStatus
        {
            public const string Pending = "Pending";
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
        }

        public static class Gender
        {
            public const string Male = "Male";
            public const string Female = "Female";
        }

        public static class FileUpload
        {
            public const long MaxFileSize = 5 * 1024 * 1024; // 5MB
            public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        }
    }
}
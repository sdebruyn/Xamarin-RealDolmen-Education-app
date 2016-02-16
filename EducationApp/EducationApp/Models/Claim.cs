namespace EducationApp.Models
{
    public class Claim
    {
        public const string AccessTokenName = "access_token";
        public const string RoleName = "role";
        public const string UniqueUserKeyName = "unique_user_key";
        public const string EmailName = "email";
        public const string GivenNameName = "given_name";
        public const string FamilyNameName = "family_name";

        public Claim(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}
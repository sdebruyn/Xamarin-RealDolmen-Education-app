using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;

namespace EducationApp.Models
{
    public class Identity : ObservableObject
    {
        private List<Claim> _claims;

        public Identity()
        {
            _claims = new List<Claim>();
        }

        public string FirstName => GetClaim(Claim.GivenNameName)?.Value;
        public string LastName => GetClaim(Claim.FamilyNameName)?.Value;
        public string Email => GetClaim(Claim.EmailName)?.Value;

        /// <summary>
        ///     List of claims.
        /// </summary>
        /// <remarks>Required for serialization. Use <see cref="AddClaims" /> instead.</remarks>
        public IEnumerable<Claim> Claims
        {
            get { return _claims; }
            set { _claims = value.ToList(); }
        }

        public override string ToString()
            => $"FirstName: {FirstName}, LastName: {LastName}, Email: {Email}, Claims: {Claims}";

        public void AddClaims(params Claim[] claims)
        {
            foreach (var claim in claims)
            {
                _claims.Add(claim);
                NotifyNewClaim(claim.Name);
            }
        }

        private void NotifyNewClaim(string name)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            if (name == Claim.GivenNameName)
            {
                RaisePropertyChanged(nameof(FirstName));
            }
            if (name == Claim.FamilyNameName)
            {
                RaisePropertyChanged(nameof(LastName));
            }
            if (name == Claim.EmailName)
            {
                RaisePropertyChanged(nameof(Email));
            }
            // ReSharper restore ExplicitCallerInfoArgument
        }

        public Claim GetClaim(string name) => _claims.FirstOrDefault(c => c.Name == name);

        public void ExtractFromIdentityToken(string token)
        {
            var parts = token.Split('.');

            var partToConvert = parts[1];
            partToConvert = partToConvert.Replace('-', '+');
            partToConvert = partToConvert.Replace('_', '/');
            switch (partToConvert.Length%4)
            {
                case 2:
                    partToConvert += "==";
                    break;
                case 3:
                    partToConvert += "=";
                    break;
            }

            var partAsBytes = Convert.FromBase64String(partToConvert);
            var partAsUtf8String = Encoding.UTF8.GetString(partAsBytes, 0, partAsBytes.Length);

            var jToken = JObject.Parse(partAsUtf8String);
            JToken sub, iss;

            if (jToken.TryGetValue("sub", out sub)
                && jToken.TryGetValue("iss", out iss))
            {
                var userKey = iss + "_" + sub;
                _claims.Add(new Claim(Claim.UniqueUserKeyName, userKey));
            }
        }
    }
}
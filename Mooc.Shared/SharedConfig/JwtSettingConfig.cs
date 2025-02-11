using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Shared.SharedConfig
{
    public class JwtSettingConfig
    {
        public const string Section = "JwtSetting";

        [Required]
        public string Issuer { get; set; }

        [Required]
        public string Audience { get; set; }

        [Required]
        public double ExpireSeconds { get; set; }

        [Required]
        public string ENAlgorithm { get; set; }

        [Required]
        public string SecurityKey { get; set;}

    }
}

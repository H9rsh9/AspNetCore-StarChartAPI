using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StarChart.Models
{
    public class CelestialObject
    {
        public int Id;

        [Required]
        public string Name;

        public int OrbitalObjectId;

        public TimeSpan OrbitalPeriod;

        [NotMapped]
        public List<CelestialObject> Satellites;
    }
}

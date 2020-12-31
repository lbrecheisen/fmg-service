using System;
using Newtonsoft.Json;

namespace Fmg.Models.Common
{
    public class Entity
    {
        [JsonProperty("id")]
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();
        public virtual Moment Created { get; set; } = new Moment();
        public virtual Moment? Updated { get; set; }
        public virtual bool IsActive { get; set; } = true;
        public virtual bool IsRemoved { get; set; } = false;
    }
}
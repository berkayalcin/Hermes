using System;
using Hermes.API.Advertisement.Extensions;

namespace Hermes.API.Advertisement.Domain.Entities
{
    public class DocumentEntityBase<T>
    {
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public string Type => typeof(T).Name.ToCamelCase();
    }
}
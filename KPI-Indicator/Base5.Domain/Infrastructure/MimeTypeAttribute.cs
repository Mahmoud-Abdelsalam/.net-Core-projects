using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base5.Domain.Infrastructure
{
    public class MimeTypeAttribute:Attribute
    {
        public List<string> MimeTypes { get; set; }
        public MimeTypeAttribute(params string[] mimeTypes)
        {
            MimeTypes = mimeTypes.ToList();
        }
    }
}

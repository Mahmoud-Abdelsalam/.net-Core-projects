using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base5.Application.Infrastructure
{
    public class CommandResponse
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }
    public class JsonRespone<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
    }

    public class GenaricResponse<IRequest>
    {
        public string Message { get; set; }
        public IRequest Data { get; set; }
        public int StatusCode { get; set; }
    }
}

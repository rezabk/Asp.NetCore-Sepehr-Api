using Newtonsoft.Json;

namespace Data
{
    public class StandardResult
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public List<string> Messages { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class StandardResult<T> : StandardResult
    {
        public T? Result { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


}


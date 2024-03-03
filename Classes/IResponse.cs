namespace prod_server.Classes
{
    public class IResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Payload { get; set; }

    }
}

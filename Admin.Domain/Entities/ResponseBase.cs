namespace Admin.Domain.Entities
{
    public class ResponseBase
    {
        public ResponseBase(int return_code, string return_chav)
        {
            this.return_code = return_code;
            this.return_chav = return_chav;
        }

        public int return_code { get; set; }
        public string return_chav { get; set; }        
    }
}

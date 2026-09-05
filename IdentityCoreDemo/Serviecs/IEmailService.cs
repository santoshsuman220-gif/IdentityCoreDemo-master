namespace IdentityCoreDemo.Serviecs
{
    public interface IEmailServices
    {
        public bool SendMail(string email,string subject,string message);
    }
}

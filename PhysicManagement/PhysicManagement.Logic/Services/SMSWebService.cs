namespace PhysicManagement.Logic.Services
{
    public class SMSWebService
    {
        public static void SendSMS(string mobile, string text)
        {
            var SmsClient = new NikSms.Library.Net.WebService.PublicServiceV1("09121491071", "387387");
            SmsClient.SendOne("blacklist", mobile, text);
        }
    }
}

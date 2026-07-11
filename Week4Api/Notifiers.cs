namespace Week4Api
{
    public interface INotifier { string Send(string message); }
    public class EmailNotifier : INotifier
    {
        public string Send(string message) => $"[Email] {message}";
    }

    public class SlackNotifier : INotifier
    {
        public string Send(string message) => $"[Slack] {message}";
    }

    public class SmsNotifier : INotifier
    {
        public string Send(string message) => $"[SMS] {message}";
    }
}

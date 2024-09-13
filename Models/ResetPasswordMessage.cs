namespace Models;

public class ResetPasswordMessage
{
    public Guid Id { get; set; }

    public string To { get; set; }
    public DateTime Date { get; set; }
}

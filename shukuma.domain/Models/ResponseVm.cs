namespace shukuma.domain.Models;

public class ResponseVm
{
    public bool IsSuccess { get; set; }
    public UserModel? User { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}

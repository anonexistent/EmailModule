using EmailTestModule.Services;
using InputContracts;
using Microsoft.AspNetCore.Mvc;
using OutputContracts;

namespace EmailTestModule.Controllers;

[ApiController]
[Route("emailTest")]
public class ResetPasswordEmailController : ControllerBase
{
    private readonly ILogger<ResetPasswordEmailController> _logger;
    private readonly ResetPasswordEmailService _resetPasswordEmailService;

    public ResetPasswordEmailController(ILogger<ResetPasswordEmailController> logger,
        ResetPasswordEmailService resetPasswordEmailService)
    {
        _logger = logger;
        _resetPasswordEmailService = resetPasswordEmailService;
    }

    [HttpPost("sendTestMessageTo")]
    public async Task<IActionResult> Send([FromQuery] ToMessageQuery q)
    {
        var result = await _resetPasswordEmailService.Send(q.To, q.Message);

        if (!result.Ok || result.Answer is null) return BadRequest(result.Errors);

        return Ok(new OutputResetPasswordMessage(result.Answer));
    }

    [HttpPost("sendMessageCache")]
    public async Task<IActionResult> SendInCache([FromQuery] ToMessageQuery q)
    {
        var result = await _resetPasswordEmailService.SendInCache(q.To, q.Message);

        if (!result.Ok || result.Answer is null) return BadRequest(result.Errors);

        return Ok(new OutputResetPasswordMessage(result.Answer));
    }
        
}

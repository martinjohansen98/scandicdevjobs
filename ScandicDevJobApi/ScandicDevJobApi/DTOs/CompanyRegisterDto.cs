public record CompanyRegisterDto(
    string Email,
    string Password,
    string CompanyName,
    string? Website,
    string? Description
);
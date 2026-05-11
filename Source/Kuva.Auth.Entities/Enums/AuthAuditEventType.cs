namespace Kuva.Auth.Entities.Enums;

public enum AuthAuditEventType
{
    UserRegistered,
    LoginSucceeded,
    LoginFailed,
    LogoutSucceeded,
    RefreshTokenSucceeded,
    RefreshTokenFailed,
    RefreshTokenRevoked,
    PasswordResetRequested,
    PasswordResetSucceeded,
    UserBlocked,
    StoreOperatorCreated,
    StoreOperatorBlocked
}

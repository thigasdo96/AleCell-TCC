namespace AleCell.API.Helpers;

public static class TranslateIdentityErrors
{
    public static string TranslateErrorMessage(string codeError)
    {
        return codeError switch
        {
            "DefaultError" => "Ocorreu um erro desconhecido.",
            "ConcurrencyFailure" => "Falha de concorrência, o objeto foi modificado.",
            "InvalidToken" => "Token inválido.",
            "LoginAlreadyAssociated" => "Já existe um usuário com este login.",
            "InvalidUserName" => "Login inválido: use apenas letras ou dígitos.",
            "InvalidEmail" => "E-mail inválido.",
            "DuplicateUserName" => "Este login já está sendo utilizado.",
            "DuplicateEmail" => "Este e-mail já está cadastrado.",
            "InvalidRoleName" => "Permissão inválida.",
            "DuplicateRoleName" => "Esta permissão já existe.",
            "UserAlreadyInRole" => "Usuário já possui esta permissão.",
            "UserNotInRole" => "Usuário não tem esta permissão.",
            "UserAlreadyHasPassword" => "Usuário já possui uma senha definida.",
            "PasswordMismatch" => "Senha incorreta.",
            "PasswordTooShort" => "Senha muito curta. Mínimo 6 caracteres.",
            "PasswordRequiresNonAlphanumeric" => "A senha deve conter ao menos um caractere especial.",
            "PasswordRequiresDigit" => "A senha deve conter ao menos um número.",
            "PasswordRequiresLower" => "A senha deve conter ao menos uma letra minúscula.",
            "PasswordRequiresUpper" => "A senha deve conter ao menos uma letra maiúscula.",
            _ => "Ocorreu um erro desconhecido.",
        };
    }
}
